namespace MenuSystem
{
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
        public Option(string text, string description, Action action)
        {
            Text = text;
            Description = description;
            Action = action;
        }
    }
}