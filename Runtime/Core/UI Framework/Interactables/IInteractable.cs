namespace SMF.Core
{
	/// <summary>
	/// IInteractable is an interface for objects that can respond to various user interactions
	/// </summary>
	public interface IInteractable
	{
		// Clicked is called when the object is clicked by the user
		void Clicked();

		// Hovered is called when the user's cursor hovers over the object
		void Hovered();

		// Unhovered is called when the user's cursor stops hovering over the object
		void Unhovered();

		// Selected is called when the object becomes the active selection
		void Selected();

		// Unselected is called when the object is no longer the active selection
		void Unselected();

		// Submit is called when the user submits/activates the object, such as pressing Enter while it is selected
		void Submit();
	}

	/// <summary>
	/// Enum with every interaction type
	/// </summary>
	public enum InteractionType
	{
		Clicked,
		Hovered,
		Selected,
		Unhovered,
		Unselected,
		Submit,
		None
	}
}