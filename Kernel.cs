using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZCPU;

namespace ZOS.frontend
{
    public class Kernel
    {
        /// <summary>
        /// The Hostname
        /// </summary>
        public static string hostname = "";
        /// <summary>
        /// Name of kernel
        /// </summary>
        public static string kernel = "ZOS Kernel";
        /// <summary>
        /// Version
        /// </summary>
        public static string zos_ver = "0.9.8";
        /// <summary>
        /// Run a GAZ File
        /// </summary>
        /// <param name="c">the cpu</param>
        /// <param name="file">the file</param>
        public static void RunShellFile(CPU c, string file)
        {
            string[] f = File.ReadAllLines(file);
            foreach (var line in f)
            {
                kint(c, line);
            }
        }
        //kernel write
        //basically console.writeline but it can write to files
        //if the second argument is empty it writes to stdout
        public static void kwrite(string a)
        {
            if (to == "")
            {
                Console.WriteLine(a);
                return;
            }
            else
            {
                if (to == "/dev/null") return;
                if (!File.Exists(to)) to = Directory.GetCurrentDirectory() + "\\" + to;
                File.AppendAllText(to, a + "\n");
            }
        }
        public static string to = "";
        /// <summary>
        /// Interpret Kernel Command
        /// </summary>
        /// <param name="c">the CPU</param>
        /// <param name="input">The Input</param>
        public static void kint(CPU c, string input)
        {
            to = "";
            if (input.Contains(">>"))
            {
                to = get_text_after_last_occurance_of_string(input, ">>").Replace(">>", "");
                if (to.StartsWith(" "))
                {
                    to = to.Substring(1);
                }
                input = input.Replace(">>" + to, "");
                input = input.Replace(">>" + " " + to, "");
            }
            else if (input.Contains(">"))
            {
                to = get_text_after_last_occurance_of_string(input, ">").Replace(">", "");
                if (to.StartsWith(" "))
                {
                    to = to.Substring(1);
                }
                input = input.Replace(">" + to, "");
                input = input.Replace(">" + " " + to, "");
            }
            if (to.StartsWith(">"))
            {
                File.WriteAllText(to, "");
                to = to.Substring(1);
            }
            if (input.EndsWith(" "))
            {
                input = input.Substring(0, input.Length - 1);
            }
            if (input.StartsWith(" "))
            {
                input = input.Substring(1);
            }
            //string totest = input.Split(' ')[0];
            if (input.StartsWith("echo"))
            {
                kwrite(input.Replace("echo ", ""));
            }
            //else if (input.StartsWith("ri")) c.initd(c.bitsize);
            else if (input.StartsWith("math"))
            {
                try
                {
                    //buggy
                    var result = Convert.ToUInt64((new DataTable()).Compute(input.Replace("math ", ""), ""));
                    kwrite((result).ToString());
                    c.nl();
                }
                catch
                {
                    c.panic(CPU.PanicType.matherror);
                }
            }
            else if (input == "shutdown") Environment.Exit(0);
            else if (input.StartsWith("shutdown"))
            {
                if (input.Replace("shutdown ", "") == "-r")
                {
                    c.clear();
                    kmain(c, hostname);
                }
                else Environment.Exit(0);
            }
            //Cringe code
            //else if (input.StartsWith("ver"))
            //{
            //    Console.WriteLine("Z beta 1 codename ready");
            //    c.nl();
            //}
            else if (input.StartsWith("curl")) kwrite(c.Network.get(input.Replace("curl ", "")));
            else if (input.StartsWith("clear")) c.clear();
            else if (input.StartsWith("exit")) Environment.Exit(0);
            else if (input.StartsWith("run"))
            {
                GAZ gi = new GAZ();
                gi.Run(input.Replace("run ", ""), c);
            }
            else if (input.StartsWith("wget"))
            {
                kwrite("Getting content...");
                byte[] content = c.Network.getBytes(input.Split(' ')[1]);
                kwrite("Saving...");
                File.WriteAllBytes(input.Split(' ')[2], content);
                kwrite("Done!");
            }
            else if (input == ("hostnamectl")) kwrite("Hostname: " + hostname);
            else if (input.StartsWith("hostnamectl set-hostname "))
            {
                hostname = input.Replace("hostnamectl set-hostname ", "");
                if (File.Exists(Directory.GetCurrentDirectory() + "\\etc\\hostname.cfg")) File.WriteAllText(Directory.GetCurrentDirectory() + "\\etc\\hostname.cfg", hostname);
            }
            else if (input.StartsWith("nano"))
            {
                //Nano n = new Nano(c, input.Replace("nano ", ""));
                //Console.Clear();

                //Very alpha function, not implemented fully yet.
            }
            else if (input.StartsWith("ls"))
            {
                foreach (var i in Directory.GetFiles(Directory.GetCurrentDirectory()))
                {
                    kwrite(new FileInfo(i).Name);
                }
                var old = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                foreach (var i in Directory.GetDirectories(Directory.GetCurrentDirectory()))
                {
                    kwrite(new DirectoryInfo(i).Name);
                }
                Console.ForegroundColor = old;
            }
            else if (input.StartsWith("cat"))
            {
                if (input != "cat /dev/null")
                    kwrite(File.ReadAllText(Directory.GetCurrentDirectory() + "\\" + input.Replace("cat ", "")));
            }
            else if (input.StartsWith("mkdir"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + input.Replace("mkdir ", ""));
            }
            else if (input.StartsWith("touch"))
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\" + input.Replace("touch ", ""), "");
            }
            else if (input.StartsWith("sh"))
            {
                RunShellFile(c, Directory.GetCurrentDirectory() + "\\" + input.Replace("sh ", ""));
            }
            else if (input.StartsWith("uname"))
            {
                kwrite(kernel + " " + zos_ver);
            }
            else if (input.StartsWith("rm -rf"))
            {
                try
                {
                    if (Directory.GetFiles(Directory.GetCurrentDirectory() + "\\" + input.Replace("rm -rf ", "")).Length + Directory.GetDirectories(Directory.GetCurrentDirectory() + "\\" + input.Replace("rm -rf ", "")).Length > 0)
                    {
                        foreach (var i in Directory.GetFiles(Directory.GetCurrentDirectory() + "\\" + input.Replace("rm -rf ", "")))
                        {
                            File.Delete(i);
                        }
                        foreach (var i in Directory.GetDirectories(Directory.GetCurrentDirectory() + "\\" + input.Replace("rm -rf ", "")))
                        {
                            Directory.Delete(i);
                        }
                    }
                    Directory.Delete(Directory.GetCurrentDirectory() + "\\" + input.Replace("rm -rf ", ""));
                }
                catch (DirectoryNotFoundException)
                {
                    kwrite("rm: directory not found");
                }
            }
            else if (input.StartsWith("rm"))
            {
                //file not found exception handling does not work for some reason
                File.Delete(Directory.GetCurrentDirectory() + "\\" + input.Replace("rm ", ""));
            }
            else if (input.StartsWith("cp"))
            {
                File.WriteAllText(input.Split()[2], File.ReadAllText(Directory.GetCurrentDirectory() + "\\" + input.Split()[1]));

            }
            else if (input.StartsWith("mv"))
            {
                File.WriteAllText(input.Split()[2], File.ReadAllText(Directory.GetCurrentDirectory() + "\\" + input.Split()[1]));
                File.Delete(Directory.GetCurrentDirectory() + "\\" + input.Split()[1]);
            }
            else if (input.StartsWith("cd"))
            {
                if (Directory.Exists(Directory.GetCurrentDirectory() + "\\" + input.Replace("cd ", "")))
                {
                    Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + "\\" + input.Replace("cd ", ""));
                }
                else if (input.StartsWith("cd .."))
                {
                    Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
                }
                else
                {
                    Directory.SetCurrentDirectory(input.Replace("cd ", ""));
                }
            }
            else if (input.StartsWith("about"))
            {
                Console.WriteLine(kernel + "@" + zos_ver);
            }
        }
        /// <summary>
        /// Gets the text after the last ocurance of a string
        /// </summary>
        /// <param name="a">The string to search</param>
        /// <param name="b">The filter String</param>
        /// <returns></returns>
        public static string get_text_after_last_occurance_of_string(string a, string b)
        {

            return a.Substring(a.LastIndexOf(b));
        }
        /// <summary>
        /// Kernel main function
        /// </summary>
        /// <param name="c">the CPU</param>
        /// <param name="hn">the Hostname</param>
        public static void kmain(CPU c, string hn)
        {
            hostname = hn;
            Console.WriteLine("Welcome to Z!\n");

            c.nl();
            while (true)
            {
                var old = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("root@" + hostname);
                Console.ForegroundColor = old;
                Console.Write(":");
                Console.ForegroundColor = ConsoleColor.Blue;
                string e;
                if (Directory.GetCurrentDirectory().Contains("\\ZOS")) e = get_text_after_last_occurance_of_string(Directory.GetCurrentDirectory(), "\\ZOS").Replace("\\ZOS", "").Replace("\\", "/");
                else e = Directory.GetCurrentDirectory();
                if (e.Length > 0)
                {
                    if (e[0] == '/')
                    {
                        var k = e.ToList();
                        k.RemoveAt(0);
                        e = new string(k.ToArray());
                    }
                }
                if (!e.Contains(":")) e = "/" + e;
                Console.Write(e);
                Console.ForegroundColor = old;
                Console.Write("$ ");
                //Memclean is not needed here as c.prnt is deprecated
                c.rde();
                string rd = intarrtostr(c.rd());
                foreach (var command in rd.Split(new string[] { "&&" }, StringSplitOptions.None))
                {
                    kint(c, command);
                    //IMPORTANT! If the memclean wasnt there, the new command would overwrite the new one.
                    c.memclean(0, command.Length);
                }
            }
        }
        /// <summary>
        /// Converts an Int[] To a String
        /// </summary>
        /// <param name="intarr">The Int[]</param>
        /// <returns>The String</returns>
        public static string intarrtostr(long[] intarr)
        {
            string a = "";
            foreach (long i in intarr) a += (char)i;
            return a;
        }
    }
}
