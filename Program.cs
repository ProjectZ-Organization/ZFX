using System;
using System.Collections.Generic;
using ZCPU;
using System.IO;
namespace ZOS
{
    public class Program
    {



        /// <summary>
        /// Start the OS
        /// </summary>
        /// <param name="c">CPU instance.</param>
        public static void _start(CPU c, string hn)
        {
            c.Display = new Display();
            c.Network = new Network();
            Console.Clear();
            ZOS.frontend.Kernel.kmain(c, hn);
        }
        static void Main(string[] args)
        {
            if(!Directory.Exists(Directory.GetCurrentDirectory() + "\\ZOS"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\ZOS");
            }
            if(!Directory.Exists(Directory.GetCurrentDirectory() + "\\ZOS\\etc"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\ZOS\\etc");
            }
            if(!File.Exists(Directory.GetCurrentDirectory() + "\\ZOS\\etc\\hostname.cfg"))
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\ZOS\\etc\\hostname.cfg", "ZOS");
            }
            string hn = File.ReadAllText(Directory.GetCurrentDirectory() + "\\ZOS\\etc\\hostname.cfg");
            Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + "\\ZOS");
            // loading screen
            {
                string startingmsg = "Z is starting...";
                int x = Console.BufferHeight / 2;
                int y = Console.BufferWidth / 2 - (startingmsg.Length / 2);
                Console.SetCursorPosition(y, x - 2);
                Console.Write(startingmsg);
            }
            CPU c = new CPU(512);
            _start(c, hn);
        }
    }
}
