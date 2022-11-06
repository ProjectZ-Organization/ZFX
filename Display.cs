using System;
using System.Drawing;

namespace ZFX
{
    public class Display
    {
        /// <summary>
        /// Defualt screen height.
        /// </summary>
        int ScreenHeight = 800;
        /// <summary>
        /// Default screen width.
        /// </summary>
        int ScreenWidth = 600;
        /// <summary>
        /// Sets the current pixel
        /// </summary>
        /// <param name="c">Color to set the pixel to</param>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void setPixel(Color c, int x, int y)
        {
            int index = (c.R > 128 | c.G > 128 | c.B > 128) ? 8 : 0;
            index |= (c.R > 64) ? 4 : 0;
            index |= (c.G > 64) ? 2 : 0;
            index |= (c.B > 64) ? 1 : 0;
            Console.BackgroundColor = (System.ConsoleColor)index;
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            ConsoleColor bgc = Console.BackgroundColor;
            Console.SetCursorPosition(x, y);
            Console.Write(" ");
            Console.SetCursorPosition(left, top);
            Console.BackgroundColor = bgc;
        }

        public void ChangeResolution(int Height, int Width)
        {
            ScreenHeight = Height;
            ScreenWidth = Width;
        }
    }
}