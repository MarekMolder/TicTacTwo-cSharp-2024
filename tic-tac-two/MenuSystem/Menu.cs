using System.Collections.Generic;

namespace MenuSystem
{
    /// <summary>
    /// Represents a menu that can display options and execute corresponding actions.
    /// This class manages the display of the menu, handles user input, and invokes actions based on the user's selection.
    /// </summary>
    public class Menu
    {
        private string prompt; // The prompt to display at the top of the menu
        private List<Option> options; // The list of options available in the menu

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="prompt">The prompt to display with the menu.</param>
        /// <param name="options">The list of menu options.</param>
        public Menu(string prompt, List<Option> options)
        {
            this.prompt = prompt;
            this.options = options;
        }

        /// <summary>
        /// Displays the menu to the user and executes the action associated with the selected option.
        /// This method combines displaying the menu and handling the user's selection.
        /// </summary>
        public void Run()
        {
            int selectedIndex = DisplayMenu(); // Display the menu and get the selected index
            ExecuteOption(selectedIndex); // Execute the action for the selected option
        }

        /// <summary>
        /// Displays the menu with available options and returns the index of the selected option.
        /// This method also handles user input through a ConsoleMenuHandler to capture the user's choice.
        /// </summary>
        /// <returns>The index of the selected option.</returns>
        private int DisplayMenu()
        {
            // Create a ConsoleMenuHandler to manage user input
            string[] descriptions = options.ConvertAll(option => option.Description).ToArray();
            ConsoleMenuHandler menuHandler = new ConsoleMenuHandler(prompt, options.ConvertAll(option => option.Text).ToArray(), descriptions);
            return menuHandler.Run(); // Run the menu and return the selected index
        }

        /// <summary>
        /// Executes the action associated with the selected menu option, if the index is valid.
        /// This method ensures the index is within the bounds of the options list before invoking the action.
        /// </summary>
        /// <param name="selectedIndex">The index of the selected option.</param>
        private void ExecuteOption(int selectedIndex)
        {
            // Ensure the selected index is within the bounds of the options list
            if (selectedIndex >= 0 && selectedIndex < options.Count)
            {
                options[selectedIndex].Action.Invoke(); // Invoke the action for the selected option
            }
        }
    }
}
