using System;
using System.Drawing;

namespace ZFX
{
    public class Display : Device
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

        void Tick() {
            
        }

        byte Send() {

        }

        void Receive(byte Data) {

        }

        public void ChangeResolution(int Height, int Width)
        {
            ScreenHeight = Height;
            ScreenWidth = Width;
        }
    }
}