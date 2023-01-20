namespace SMF.Extensions
{
	public static class StringExtension
	{
		/// <summary>
		/// Returns a value between two strings
		/// </summary>
		/// <param name="value">of sentence</param>
		/// <param name="start">after</param>
		/// <param name="end">before</param>
		/// <returns></returns>
		public static string GetValueBetween(string value, string start, string end)
		{
			int startPosition = value.IndexOf(start);
			int endPosition = value.LastIndexOf(end);

			if (startPosition == -1)
				return value;

			if (endPosition == -1)
				return value;

			int adjustedStartPosition = startPosition + start.Length;

			if (adjustedStartPosition >= endPosition)
				return "";

			return value.Substring(adjustedStartPosition, endPosition - adjustedStartPosition);
		}

		/// <summary>
		/// Returns a value before string 
		/// </summary>
		/// <param name="value">of sentence</param>
		/// <param name="start">before</param>
		/// <returns></returns>
		public static string GetValueBefore(string value, string start)
		{
			int startPosition = value.IndexOf(start);

			if (startPosition == -1)
				return value;

			return value.Substring(0, startPosition);
		}

		/// <summary>
		/// Returns a value after string 
		/// </summary>
		/// <param name="value">of sentence</param>
		/// <param name="start">after</param>
		/// <returns></returns>
		public static string GetValueAfter(string value, string start)
		{
			int startPosition = value.IndexOf(start);

			if (startPosition == -1)
				return value;

			int adjustedStartPosition = startPosition + start.Length;

			if (adjustedStartPosition >= value.Length)
				return "";

			return value.Substring(adjustedStartPosition);
		}
	}
}