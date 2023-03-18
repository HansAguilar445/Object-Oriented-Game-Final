using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Menu
    {
        private List<string> _options { get; set; } = new List<string>();
        private int _selectedOption { get; set; } = 0;

        public Menu(List<string> options)
        {
            _options = options;
        }

        public string RunMenu(string previousDialogue)
        {
            bool isLooping = true;

            do
            {
                Clear();

                WriteLine(previousDialogue);
                CycleMenu();

                ConsoleKey consoleKey = ReadKey().Key;

                switch (consoleKey)
                {
                    case ConsoleKey.UpArrow:
                        if (_selectedOption - 1 < 0)
                        {
                            _selectedOption = _options.Count - 1;
                        }
                        else
                        {
                            _selectedOption--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (_selectedOption + 1 == _options.Count)
                        {
                            _selectedOption = 0;
                        }
                        else
                        {
                            _selectedOption++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        isLooping = false;
                        break;
                }
            } while (isLooping);

            return _options[_selectedOption];
        }

        private void CycleMenu()
        {
            ConsoleColor initialBackground = BackgroundColor;
            ConsoleColor initialForeground = ForegroundColor;

            for (int i = 0; i < _options.Count; i++)
            {
                if (i == _selectedOption)
                {
                    BackgroundColor = initialForeground;
                    ForegroundColor = initialBackground;
                }

                WriteLine($"{i + 1}. {_options[i]}");

                ResetColor();
            }

        }
    }
}
