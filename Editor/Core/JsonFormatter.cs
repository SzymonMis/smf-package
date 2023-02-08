using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMF.Editor.Core
{
	/// <summary>
	/// A utility to format JSON in pretty text.
	/// </summary>
	public static class JsonFormatter
	{
		private const string INDENT_STRING = "    ";

		/// <summary>
		/// Format the JSON to pretty text. 
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string FormatJson(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				Console.WriteLine($"Passed string: {str} was null or empty. Cannot format. Returning empty string.");
				return string.Empty;
			}
			var indent = 0;
			var quoted = false;
			var sb = new StringBuilder();
			for (var i = 0; i < str.Length; i++)
			{
				var ch = str[i];
				switch (ch)
				{
					case '{':
					case '[':
						sb.Append(ch);
						if (!quoted)
						{
							sb.AppendLine();
							Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
						}

						break;
					case '}':
					case ']':
						if (!quoted)
						{
							sb.AppendLine();
							Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
						}

						sb.Append(ch);
						break;
					case '"':
						sb.Append(ch);
						bool escaped = false;
						var index = i;
						while (index > 0 && str[--index] == '\\')
							escaped = !escaped;
						if (!escaped)
							quoted = !quoted;
						break;
					case ',':
						sb.Append(ch);
						if (!quoted)
						{
							sb.AppendLine();
							Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
						}

						break;
					case ':':
						sb.Append(ch);
						if (!quoted)
							sb.Append(" ");
						break;
					default:
						sb.Append(ch);
						break;
				}
			}

			return sb.ToString();
		}
	}

	static class Extensions
	{
		public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
		{
			foreach (var i in ie)
			{
				action(i);
			}
		}
	}
}