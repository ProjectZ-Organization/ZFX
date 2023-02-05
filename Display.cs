using System;

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

        string Name = "Display";

        public void ChangeResolution(int Height, int Width)
        {
            ScreenHeight = Height;
            ScreenWidth = Width;
        }
    }
}