using System.Collections.Generic;

namespace SMF.Extensions
{
	public static class ListExtension
	{
		/// <summary>
		/// This function easily allows you to swap the location of two items in the list
		/// </summary>
		/// <typeparam name="T">stands for list generic type</typeparam>
		/// <param name="list">reference to the list being changed</param>
		/// <param name="first">element index</param>
		/// <param name="second">element index</param>
		/// <returns></returns>
		public static bool SwapElements<T>(this List<T> list, int first, int second)
		{
			if (first != second && 0 <= first && first < list.Count && 0 <= second && second < list.Count)
			{
				T f = list[first];

				list[first] = list[second];
				list[second] = f;
			}

			return false;
		}
	}
}