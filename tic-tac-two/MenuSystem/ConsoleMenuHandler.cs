namespace MenuSystem
{
    public class ConsoleMenuHandler
    {
        private int _selectedIndex;
        private readonly string[] _options;
        private readonly string[] _descriptions;
        private readonly string _prompt;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMenuHandler"/> class.
        /// </summary>
        public ConsoleMenuHandler(string prompt, string[] options, string[] descriptions)
        {
            _prompt = prompt;
            _options = options;
            _descriptions = descriptions;
            _selectedIndex = 0;
        }

        /// <summary>
        /// Displays the menu options to the console with the currently selected option highlighted.
        /// The description of the selected option is shown next to it.
        /// </summary>
        private void DisplayOptions()
        {
            Console.WriteLine(_prompt);
            for (int i = 0; i < _options.Length; i++)
            {
                string currentOption = _options[i];
                string description = _descriptions[i];
                string prefix;

                if (i == _selectedIndex)
                {
                    prefix = "*"; // Selected option prefix.
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write($" {prefix} << {currentOption} >> ");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"- {description}");
                }
                else
                {
                    prefix = " ";
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($" {prefix} << {currentOption} >> ");
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Runs the menu, allowing user interaction to navigate through options and select one.
        /// </summary>
        public int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                DisplayOptions();
                
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;
                
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    _selectedIndex--;
                    if (_selectedIndex == -1)
                    {
                        _selectedIndex = _options.Length - 1;
                    }
                }
             
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    _selectedIndex++; 
                    if (_selectedIndex == _options.Length)
                    {
                        _selectedIndex = 0;
                    }
                }
            } while (keyPressed != ConsoleKey.Enter);
            
            return _selectedIndex;
        }
    }
}
