namespace MenuSystem
{
    /// <summary>
    /// Represents a single option in a menu, including its text, description, and associated action.
    /// </summary>
    public class Option
    {
        /// <summary>
        /// Gets the text to display for the option.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the description of the option, providing additional information.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the action to execute when the option is selected.
        /// </summary>
        public Action Action { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Option"/> class with the specified text, description, and action.
        /// </summary>
        /// <param name="text">The text to display for the option, shown to the user as the selectable menu option.</param>
        /// <param name="description">The description of the option, providing additional context or details about the option.</param>
        /// <param name="action">The action to execute when this option is selected. This should be a method or delegate to run when the option is chosen.</param>
        public Option(string text, string description, Action action)
        {
            Text = text; // Assign the display text for the option.
            Description = description; // Assign the description for the option.
            Action = action; // Assign the action to be executed for this option.
        }
    }
}