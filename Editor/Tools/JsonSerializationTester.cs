using System;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using SMF.Editor.Core;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif

namespace SMF.Editor.Tools
{
	/// <summary>
	/// An editor window that can test if a certain string can be deserialized in a data type.
	/// </summary>
	public class JsonSerializationTester : OdinEditorWindow
	{
		[OnInspectorGUI, TitleGroup("JSON Deserialization Tester", "", TitleAlignments.Centered), PropertyOrder(-10)]
		[DetailedInfoBox("You can input a JSON string in the field... ", "You can input a JSON string in the field and specify the type you want to deserialize into and click!\nIt uses Newtonsoft.Json.JsonConvert.DeserializeObject to deserialize the object.\nIt also tries to figure out if a list of objects is supplied or just one object.")]
		public void DummyInfoFunction() { }

		/// <summary>
		/// Object type in which we're trying to deserialize into.
		/// </summary>
		[ShowInInspector, LabelWidth(75)]
		[HorizontalGroup("Object Type", width: 0.75f), PropertyOrder(0)]
		public Type objectType;

		/// <summary>
		/// Is the string a list of the object types?
		/// </summary>
		[LabelWidth(50)]
		[HorizontalGroup("Object Type"), PropertyOrder(0)]
		public bool isList = false;

		/// <summary>
		/// Input JSON string.
		/// </summary>
		[Title("Deserialization String", bold: false)]
		[HideLabel]
		[MultiLineProperty(20), PropertyOrder(2)]
		[OnValueChanged("InputStringValueChanged")]
		public string inputString;

		/// <summary>
		/// Message shown to the user. Either error or deserialized fine.
		/// </summary>
		private static string _infoBoxMessage = "";
		/// <summary>
		/// A flag to record if an exception was thrown on deserialize.
		/// </summary>
		private static bool _hadError = false;

		/// <summary>
		/// The model that is created from deserialization. Stored for easy browsing.
		/// </summary>
		[ShowInInspector, ShowIf("ShowDeserializedModel"), PropertyOrder(11), NonSerialized]
		// [ReadOnly]
		private static dynamic _deserializedObject;

		/// <summary>
		/// Flag used to skip clearing info box when value of input field is changed.
		/// </summary>
		private static bool _updatingInputString = false;

		/// <summary>
		/// Regex applied on the message of the exception. Used to figure out if a list of object JSON was provided but were
		/// expecting a object.
		/// </summary>
		private static readonly Regex ExpectingListRegex = new Regex(@"Cannot deserialize the current JSON array \(e\.g\. \[1,2,3\]\) into type '.+' because the type requires a JSON object \(e\.g\. \{""name"":""value""\}\) to deserialize correctly\.");

		/// <summary>
		/// Regex applied on the message of the exception. Used to figure out if a JSON object was provided but were
		/// expecting a list of objects.
		/// </summary>
		private static readonly Regex ExpectingObjectRegex = new Regex(@"Cannot deserialize the current JSON object \(e\.g\. \{""name"":""value""\}\) into type 'System\.Collections\.Generic\.List`1\[.+\]' because the type requires a JSON array \(e\.g\. \[1,2,3\]\) to deserialize correctly\.");

		/// <summary>
		/// A counter to stop infinite recursion.
		/// </summary>
		private static int _recursionDepth = 0;

		/// <summary>
		/// Flag used to stop adding json formatting to already formatter string.
		/// It added new lines on already formatted string.
		/// </summary>
		private static bool _jsonFormatted = false;

		private static JsonSerializationTester _instance;

		/// <summary>
		/// Open this window.
		/// </summary>
		[MenuItem("SMF Tools/Json Deserialization")]
		private static void OpenWindow()
		{
			_instance = GetWindow<JsonSerializationTester>();
			_instance.Show();
		}

		protected override void Initialize()
		{
			base.Initialize();
			_instance = this;
		}

		/// <summary>
		/// We use this proxy because we cannot update _deserializedObject without setting it null first and waiting.
		/// The editor renders it faster than code runs so it throws errors if the type of variable changes while it is trying to render it.
		/// </summary>
		[Button(ButtonSizes.Large), PropertyOrder(1)]
		public void TestDeserializationButton()
		{
			_deserializedObject = null;
			EditorApplication.delayCall += TestDeserialization;
		}

		/// <summary>
		/// Tries to deserialize the input string to the object type. Informs user of success or failure.
		/// </summary>
		public void TestDeserialization()
		{
			if (string.IsNullOrEmpty(inputString) || string.IsNullOrWhiteSpace(inputString))
			{
				_infoBoxMessage = "That is an empty or white space string. Water you trina pull here?";
				return;
			}

			if (objectType == null)
			{
				_infoBoxMessage = "Object type is not set.";
				return;
			}

			_recursionDepth++;
			try
			{
				_hadError = false;
				_infoBoxMessage = string.Empty;
				_deserializedObject = null;
				var deserializationType = isList ? typeof(List<>).MakeGenericType(objectType) : objectType;
				// since it's 21st century, we try to figure out if the user forgot to check the isList flag and fix it.
				try
				{

					dynamic buffer = JsonConvert.DeserializeObject(inputString, deserializationType);
					_deserializedObject = Convert.ChangeType(buffer, deserializationType);
				}
				catch (JsonSerializationException jsonSerializationException)
				{
					if (_recursionDepth > 10)
					{
						Debug.LogError("Recursed the deserialize function more than 10 times. Stopping loop. ");
						throw;
					}
					if (ExpectingListRegex.IsMatch(jsonSerializationException.Message))
					{
						// when user forgot to enable isList.
						isList = true;
						TestDeserialization();
					}
					else if (ExpectingObjectRegex.IsMatch(jsonSerializationException.Message))
					{
						// when user left the isList on and it's not.
						isList = false;
						TestDeserialization();
					}
					else
					{
						throw;
					}

					return;
				}
				if (_jsonFormatted == false)
				{
					UpdateInputField(JsonFormatter.FormatJson(inputString));
					_jsonFormatted = true;
				}
			}
			catch (Exception e)
			{
				_hadError = true;
				_infoBoxMessage = $"Failed to deserialize. Exception: {e}";
				Debug.LogError(_infoBoxMessage);
				_recursionDepth = 0;
			}
			finally
			{
				if (_hadError == false)
				{
					_infoBoxMessage = "No exceptions thrown when deserializing.";
					_recursionDepth = 0;
				}
			}
		}

		/// <summary>
		/// This function stop the on value changed function from being invoked.
		/// </summary>
		/// <param name="newString"></param>
		private void UpdateInputField(string newString)
		{
			_updatingInputString = true;
			inputString = newString;
			_updatingInputString = false;
		}


		/// <summary>
		/// We use this dummy method because we don't want show the _infoBoxMessage field in the inspector. So InfoBoxes attributes are put on this method.
		/// </summary>
		[OnInspectorGUI, PropertyOrder(10)]
		[InfoBox("$_infoBoxMessage", InfoMessageType.Info, "ShowNormalInfoBox")]
		[InfoBox("$_infoBoxMessage", InfoMessageType.Error, "ShowErrorInfoBox")]
		[ShowIf("ShowInfoBox")]
		private static void DummyMethodForInfoBox() { }

		/// <summary>
		/// Show it show info box. 
		/// </summary>
		private static bool ShowInfoBox => string.IsNullOrEmpty(_infoBoxMessage) == false;

		/// <summary>
		/// Should the info box be of error type?
		/// </summary>
		private static bool ShowErrorInfoBox => ShowInfoBox && _hadError;

		/// <summary>
		/// Should the info box be of normal type?
		/// </summary>
		private static bool ShowNormalInfoBox => ShowInfoBox && _hadError == false;

		/// <summary>
		/// Should we show the deserialized model?. We want to keep this shown until Test Deserialization 
		/// is pressed.
		/// </summary>
		private static bool ShowDeserializedModel => _deserializedObject != null;

		/// <summary>
		/// This is called when value of input field changes.
		/// It always returns true but it clears the info box message if the value changes.
		/// </summary>
		private static void InputStringValueChanged()
		{
			if (_updatingInputString) return;
			_infoBoxMessage = string.Empty;
			_jsonFormatted = false;
		}
	}
}