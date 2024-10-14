namespace MenuSystem
{
    /// <summary>
    /// Represents a text-based menu handler that allows users to navigate through options using keyboard input.
    /// </summary>
    public class ConsoleMenuHandler
    {
        private int SelectedIndex; // The index of the currently selected option in the menu.
        private string[] Options; // The array of options available in the menu.
        private string[] Descriptions; // The array of descriptions for each option.
        private string Prompt; // The prompt message displayed at the top of the menu.

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMenuHandler"/> class.
        /// </summary>
        /// <param name="prompt">The prompt to display above the menu options.</param>
        /// <param name="options">An array of strings representing the menu options.</param>
        /// <param name="descriptions">An array of strings representing the descriptions for the options.</param>
        public ConsoleMenuHandler(string prompt, string[] options, string[] descriptions)
        {
            Prompt = prompt; // Set the prompt message.
            Options = options; // Set the menu options.
            Descriptions = descriptions; // Set the descriptions for the options.
            SelectedIndex = 0; // Initialize the selected index to the first option.
        }

        /// <summary>
        /// Displays the menu options to the console with the currently selected option highlighted.
        /// The description of the selected option is shown next to it.
        /// </summary>
        private void DisplayOptions()
        {
            Console.WriteLine(Prompt); // Display the prompt message.
            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i]; // Get the current option text.
                string description = Descriptions[i]; // Get the description for the current option.
                string prefix; // Prefix to indicate selection.

                // Determine the appearance of the current option based on selection.
                if (i == SelectedIndex)
                {
                    prefix = "*"; // Selected option prefix.
                    Console.ForegroundColor = ConsoleColor.Black; // Set text color for selected option.
                    Console.BackgroundColor = ConsoleColor.White; // Set background color for selected option.
                    Console.Write($" {prefix} << {currentOption} >> "); // Display the current option.
                    Console.ForegroundColor = ConsoleColor.Black; // Change to gray for the description.
                    Console.WriteLine($"- {description}"); // Display the description next to the selected option.
                }
                else
                {
                    prefix = " "; // Non-selected option prefix.
                    Console.ForegroundColor = ConsoleColor.White; // Set text color for non-selected options.
                    Console.BackgroundColor = ConsoleColor.Black; // Set background color for non-selected options.
                    Console.Write($" {prefix} << {currentOption} >> "); // Display the current option.
                    Console.ResetColor(); // Reset colors for the description.
                    Console.WriteLine(); // New line for non-selected options.
                }
            }
            Console.ResetColor(); // Reset the console colors to default after displaying all options.
        }

        /// <summary>
        /// Runs the menu, allowing user interaction to navigate through options and select one.
        /// </summary>
        /// <returns>The index of the selected option.</returns>
        public int Run()
        {
            ConsoleKey keyPressed; // Variable to store the key pressed by the user.
            do
            {
                Console.Clear(); // Clear the console for a fresh display.
                DisplayOptions(); // Display the menu options.
                
                ConsoleKeyInfo keyInfo = Console.ReadKey(true); // Read a key press without displaying it.
                keyPressed = keyInfo.Key; // Store the key pressed.
                
                // Handle Up Arrow key press to navigate up in the menu.
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex--; // Decrease the selected index.
                    if (SelectedIndex == -1) // Wrap around to the last option if at the top.
                    {
                        SelectedIndex = Options.Length - 1;
                    }
                }
                // Handle Down Arrow key press to navigate down in the menu.
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex++; // Increase the selected index.
                    if (SelectedIndex == Options.Length) // Wrap around to the first option if at the bottom.
                    {
                        SelectedIndex = 0;
                    }
                }
            } while (keyPressed != ConsoleKey.Enter); // Continue until Enter key is pressed.
            
            return SelectedIndex; // Return the index of the selected option.
        }
    }
}
