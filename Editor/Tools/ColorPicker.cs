/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using UnityEditor;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif

namespace SMF.Editor.Tools
{
	#if ODIN_INSPECTOR
	public class ColorPicker : OdinEditorWindow
	{
		private static ColorPicker _instance;

		[MenuItem("SMF Tools/Color Picker")]
		public static void OpenWindow()
		{
			_instance = GetWindow<ColorPicker>();
			_instance.minSize = new Vector2(400, 200);
			_instance.maxSize = new Vector2(800, 400);
			_instance.Show();
		}

		protected override void Initialize()
		{
			base.Initialize();
			_instance = this;
		}
		
		[OnValueChanged("OnColorChanged")] public Color color = Color.white;

		[OnValueChanged("OnColor32Changed")]
		private Color32 color32 = new Color32(0xFF, 0xFF, 0xFF, 0xFF);

		[OnValueChanged("OnHexCodeChanged")]
		[SuffixLabel("supports literals e.g. red, blue w/o #")]
		public string hexCode = "#FFFFFFFF";

		public void OnColorChanged(Color newColor)
		{
			color32 = newColor;
			hexCode = $"#{ColorUtility.ToHtmlStringRGBA(newColor)}";

			r = color32.r;
			g = color32.g;
			b = color32.b;
			a = color32.a;

			_instance.RemoveNotification();
		}

		public void OnColor32Changed(Color32 newColor32)
		{
			color = newColor32;
			hexCode = ColorUtility.ToHtmlStringRGBA(color);

			OnColorChanged(color);
		}

		public void OnHexCodeChanged(string newHexCode)
		{
			var successful = ColorUtility.TryParseHtmlString(newHexCode, out var newColor);
			if (newHexCode.StartsWith("#") == false && successful == false)
			{
				successful = ColorUtility.TryParseHtmlString($"#{newHexCode}", out newColor);
			}

			if (successful == false)
			{
				_instance.ShowNotification(
					new GUIContent(
						$"Problem parsing hex code: \n#{newHexCode}.\n" +
						$"Also supports literals \ne.g. yellow, red."));
				return;
			}

			color = newColor;
			color32 = color;

			OnColorChanged(color);
		}


		[Range(0, 255)]
		[OnValueChanged("OnRGBSliderChange")]
		public float r = 255;

		[Range(0, 255)]
		[OnValueChanged("OnRGBSliderChange")]
		public float g = 255;

		[Range(0, 255)]
		[OnValueChanged("OnRGBSliderChange")]
		public float b = 255;

		[Range(0, 255)]
		[OnValueChanged("OnRGBSliderChange")]
		public float a = 255;

		public void OnRGBSliderChange()
		{
			color.r = r / 255;
			color.g = g / 255;
			color.b = b / 255;
			color.a = a / 255;

			OnColorChanged(color);
		}

		[Title("Color Code")]
		[ShowInInspector, HideLabel, InlineButton("CopyColor", "Copy Color Code")]
		public string ColorCode
		{
			get => $"new Color({color.r}f, {color.g}f, {color.b}f, {color.a}f);";
			set => _ = value;
		}

		[ShowInInspector, HideLabel, InlineButton("CopyColor32", "Copy Color32 Code")]
		public string Color32Code
		{
			get => $"new Color32({color32.r}, {color32.g}, {color32.b}, {color32.a});";
			set => _ = value;
		}

		public void CopyColor32() => EditorGUIUtility.systemCopyBuffer = Color32Code;
		public void CopyColor() => EditorGUIUtility.systemCopyBuffer = ColorCode;
	}
	#else
	public class ColorPickerWindow : EditorWindow
	{
		private Color _color = Color.white;
		private Color32 _color32 = new Color32(255, 255, 255, 255);
		private string _hexCode = "FFFFFF";

		[MenuItem("SMF Tools/Color Picker")]
		public static void OpenWindow()
		{
			var window = GetWindow<ColorPickerWindow>("Color Picker");
			window.Show();
		}

		protected virtual void OnGUI()
		{
			_color = EditorGUILayout.ColorField("Color", _color);
			if (GUI.changed)
			{
				_color32 = _color;
				_hexCode = ColorUtility.ToHtmlStringRGB(_color);
			}

			_hexCode = EditorGUILayout.TextField("Hex Code", _hexCode);
			if (GUI.changed)
			{
				ColorUtility.TryParseHtmlString(_hexCode, out _color);
			}

			_color32.r = (byte)EditorGUILayout.IntSlider("Red", _color32.r, 0, 255);
			_color32.g = (byte)EditorGUILayout.IntSlider("Green", _color32.g, 0, 255);
			_color32.b = (byte)EditorGUILayout.IntSlider("Blue", _color32.b, 0, 255);
			_color32.a = (byte)EditorGUILayout.IntSlider("Alpha", _color32.a, 0, 255);
			if (GUI.changed)
			{
				_color = _color32;
				_hexCode = ColorUtility.ToHtmlStringRGB(_color);
			}

			EditorGUILayout.TextField(
				"Color Code",
				$"new Color ({_color.r}f, {_color.g}f, {_color.b}f, {_color.a}f)");
			EditorGUILayout.TextField(
				"Color32 Code",
				$"new Color32 ({_color32.r}, {_color32.g}, {_color32.b}, {_color32.a})");
		}
	}
	#endif
}