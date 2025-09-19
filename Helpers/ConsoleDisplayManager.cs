using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindingSimulator.Helpers
{
    public static class ConsoleDisplayManager
    {
        /// <summary>
        /// Выводит цветное сообщение
        /// </summary>
        public static void PrintColoredMessage(string message, ConsoleColor color, bool newLine = false)
        {
            Console.ForegroundColor = color;

            if (newLine)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.Write(message);
            }

            Console.ResetColor();
        }

        /// <summary>
        /// Выводит пару ключ-значение с цветами
        /// </summary>
        public static void PrintKeyValue(string key, string value, ConsoleColor keyColor, ConsoleColor valueColor)
        {
            Console.Write(" ");
            PrintColoredMessage(key + ": ", keyColor);
            PrintColoredMessage(value, valueColor, true);
        }

        /// <summary>
        /// Выводит заголовок секции
        /// </summary>
        public static void PrintSectionHeader(string header)
        {
            Console.WriteLine();
            PrintColoredMessage(header, ConsoleColor.Magenta, true);
            PrintColoredMessage(new string('═', header.Length), ConsoleColor.DarkMagenta, true);
            Console.WriteLine();
        }
    }
}
