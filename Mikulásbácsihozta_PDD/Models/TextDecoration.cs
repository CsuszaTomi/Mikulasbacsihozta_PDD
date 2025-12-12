using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mikulásbácsihozta_PDD.Models
{
    /// <summary>
    /// Olyan függvényeket tartalmaz amelyek a konzol szövegek dekorálására szolgálnak
    /// </summary>
    internal class TextDecoration
    {
        /// <summary>
        /// A függvény a Console.WriteLine középre íratását valósítja meg
        /// </summary>
        /// <param name="text">A megadott szöveget írja ki középre</param>
        public static void WriteLineCentered(string text)
        // Console.WriteLine középre íratása
        {
            int width = Console.WindowWidth;
            int leftPadding = (width - text.Length) / 2;
            if (leftPadding < 0)
            {
                leftPadding = 0;
            }
            Console.WriteLine(new string(' ', leftPadding) + text);
        }
        public static void WriteCentered(string text)
        // Console.Write középre íratása
        {
            int width = Console.WindowWidth;
            int leftPadding = (width - text.Length) / 2;
            if (leftPadding < 0)
            {
                leftPadding = 0;
            }
            Console.Write(new string(' ', leftPadding) + text);
        }

        public static string CenterText(string text, int width)
        {
            if (text.Length > width)
                text = text.Substring(0, width);

            int left = (width - text.Length) / 2;
            int right = width - text.Length - left;

            return new string(' ', left) + text + new string(' ', right);
        }
        /// <summary>
        /// Menü kezelő függvény
        /// </summary>
        /// <param name="menupoints">Menüpontok amiket létrehoz</param>
        /// <param name="title">A menü címe</param>
        /// <returns></returns>
        public static int ArrowMenu(string[] menupoints, string title)
        {
            int currentPoint = 0;
            bool selected = false;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                TextDecoration.WriteLineCentered(title);
                Console.ForegroundColor = ConsoleColor.White;
                TextDecoration.WriteLineCentered("--------------------");
                for (int i = 0; i < menupoints.Length; i++)
                {
                    if (i == currentPoint)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        TextDecoration.WriteLineCentered($"> {menupoints[i]}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        TextDecoration.WriteLineCentered($"  {menupoints[i]}");
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                TextDecoration.WriteLineCentered("--------------------");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        selected = true;
                        break;
                    case ConsoleKey.E:
                        selected = true;
                        break;
                    case ConsoleKey.UpArrow:
                        if (currentPoint > 0) currentPoint--;
                        break;
                    case ConsoleKey.W:
                        if (currentPoint > 0) currentPoint--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentPoint < menupoints.Length - 1) currentPoint++;
                        break;
                    case ConsoleKey.S:
                        if (currentPoint < menupoints.Length - 1) currentPoint++;
                        break;
                    default:
                        Console.Beep();
                        break;
                }
            } while (!selected);
            return currentPoint;
        }
    }
}
