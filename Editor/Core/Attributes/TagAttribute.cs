using System;

namespace SMF.Editor.Core
{
	/// <summary>
	/// Tag attribute.
	/// Attribute is use to display and select one of tags. It changes string variable in the inspector to tag enum.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class TagAttribute : Attribute { }
}