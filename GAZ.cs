using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCPU
{
    public class GAZ
    {
        public long pc = 0;
        public bool cmpequalresult = false;
        public bool cmpgreaterthanresult = false;
        public void InterpretLine(string line, CPU c)
        {
            string arg1 = "";
            try
            {
                arg1 = line.Split(' ')[1];
                if(arg1.StartsWith("%"))
                {
                    arg1 = arg1.Replace("%", "");
                    arg1 = c.RAM[Convert.ToInt32(arg1)].ToString();
                }
            }
            catch { }
            //Console.WriteLine(arg1);
            string arg2 = "";
            try
            {
                arg2 = line.Split(' ')[2];
                if (arg1.StartsWith("%"))
                {
                    arg1 = arg1.Replace("%", "");
                    arg1 = c.RAM[Convert.ToInt32(arg1)].ToString();
                }
            }
            catch { }
            string arg3 = "";
            try
            {
                arg3 = line.Split(' ')[3];
                if (arg1.StartsWith("%"))
                {
                    arg1 = arg1.Replace("%", "");
                    arg1 = c.RAM[Convert.ToInt32(arg1)].ToString();
                }
            }
            catch { }
            switch (line.Split(' ')[0])
            {
                case "add":
                    c.add(Convert.ToInt32(arg1), Convert.ToInt32(arg2), Convert.ToInt32(arg3));
                    break;
                case "sub":
                    c.sub(Convert.ToInt32(arg1), Convert.ToInt32(arg2), Convert.ToInt32(arg3));
                    break;

                case "mul":
                    c.mul(Convert.ToInt32(arg1), Convert.ToInt32(arg2), Convert.ToInt32(arg3));
                    break;

                case "div":
                    c.div(Convert.ToInt32(arg1), Convert.ToInt32(arg2), Convert.ToInt32(arg3));
                    break;
                case "hlt":
                    c.hlt();
                    break;
                case "mmcl":
                    c.memclean(Convert.ToInt32(arg1), Convert.ToInt32(arg2));
                    break;
                case "copy":
                    c.setMemLoc(Convert.ToInt32(arg2), (int)c.rdI(Convert.ToInt32(arg1)));
                    break;
                case "isr":
                    c.setMemLoc(Convert.ToInt32(arg2), Convert.ToInt32(c.IsReserved(Convert.ToInt32(arg1))));
                    break;
                case "inc":
                    c.inc(Convert.ToInt32(arg1));
                    break;
                case "clr":
                    c.clear();
                    break;
                case "swap":
                    c.swp(Convert.ToInt32(arg1), Convert.ToInt32(arg2));
                    break;
                case "rde":
                    c.rde(Convert.ToInt32(arg1));
                    break;
                case "mov":
                    c.setMemLoc(Convert.ToInt32(arg2), Convert.ToInt32(arg1));
                    break;
                case "prnt":
                    line = line.Replace("prnt ", "");
                    string oldline = line;
                    line = "";
                    foreach (var i in oldline.Split(' '))
                    {
                        var b = i;
                        if(b.StartsWith("%"))
                        {
                            b = b.Replace("%", "");
                            b = c.RAM[Convert.ToInt32(b)].ToString();
                        }
                        line += b + " ";
                    }
                    c.prnt(line);
                    break;
                case "sqrt":
                    c.sqrt(Convert.ToInt32(arg1), Convert.ToInt32(arg2));
                    break;
                case "pow":
                    c.pow(Convert.ToInt32(arg1), Convert.ToInt32(arg2), Convert.ToInt32(arg3));
                    break;
                case "jmp":
                    pc = Convert.ToInt32(arg1) - 1;
                    break;
                case "jg":
                    if (cmpgreaterthanresult)
                    {
                        pc = Convert.ToInt32(arg1) - 1;
                    }
                    break;
                case "jl":
                    if (!cmpgreaterthanresult)
                    {
                        pc = Convert.ToInt32(arg1) - 1;
                    }
                    break;
                case "je":
                    if (cmpequalresult)
                    {
                        pc = Convert.ToInt32(arg1) - 1;
                    }
                    break;
                case "jne":
                    if (!cmpequalresult)
                    {
                        pc = Convert.ToInt32(arg1) - 1;
                    }
                    break;
                case "cmp":
                    cmpequalresult = Convert.ToInt32(arg1) == Convert.ToInt32(arg2);
                    cmpgreaterthanresult = Convert.ToInt32(arg1) > Convert.ToInt32(arg2);
                    break;
                default:
                    if (!line.StartsWith(";"))
                    {
                        c.panic(CPU.PanicType.invalidinstruction);
                    }
                    break;
            }
        }
        public void Run(string file, CPU c)
        {
            string[] lines = File.ReadAllLines(file);
            while (pc<lines.Length)
            {
                pc++;
                InterpretLine(lines[pc-1], c);
            }
        }
    }
}
