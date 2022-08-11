using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ZFX
{
    public class Disk
    {
        public static void MapDrive(char letter, string path)
        {
            if (!DefineDosDevice(0, DevName(letter), path))
                throw new Win32Exception();
        }
        public static void UnMapDrive(char letter)
        {
            if (!DefineDosDevice(2, DevName(letter), null))
                throw new Win32Exception();
        }
        public static string GetDriveMapping(char letter)
        {
            var sb = new StringBuilder(259);
            if (QueryDosDevice(DevName(letter), sb, sb.Capacity) == 0)
            {
                int err = Marshal.GetLastWin32Error();
                if (err == 2) return "";
                throw new Win32Exception();
            }
            return sb.ToString().Substring(4);
        }


        private static string DevName(char letter)
        {
            return new string(char.ToUpper(letter), 1) + ":";
        }
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool DefineDosDevice(int flags, string devname, string path);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int QueryDosDevice(string devname, StringBuilder buffer, int bufSize);
    }
}