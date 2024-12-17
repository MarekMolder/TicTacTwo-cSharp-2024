namespace MenuSystem
{
    public class Menu(string prompt, List<Option> options)
    {
        /// <summary>
        /// Displays the menu to the user and executes the action associated with the selected option.
        /// This method combines displaying the menu and handling the user's selection.
        /// </summary>
        public void Run()
        {
            int selectedIndex = DisplayMenu();
            ExecuteOption(selectedIndex);
        }

        /// <summary>
        /// Displays the menu with available options and returns the index of the selected option.
        /// This method also handles user input through a ConsoleMenuHandler to capture the user's choice.
        /// </summary>
        private int DisplayMenu()
        {
            string[] descriptions = options.ConvertAll(option => option.Description).ToArray();
            ConsoleMenuHandler menuHandler = new ConsoleMenuHandler(prompt, options.ConvertAll(option => option.Text).ToArray(), descriptions);
            return menuHandler.Run();
        }

        /// <summary>
        /// Executes the action associated with the selected menu option, if the index is valid.
        /// This method ensures the index is within the bounds of the options list before invoking the action.
        /// </summary>
        private void ExecuteOption(int selectedIndex)
        {
            if (selectedIndex >= 0 && selectedIndex < options.Count)
            {
                options[selectedIndex].Action.Invoke();
            }
        }
    }
}
