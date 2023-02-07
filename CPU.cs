using System;
using System.IO;
using System.Diagnostics;

namespace ZFX
{
    public class CPU
    {
        ///<summary>
        ///CPU initialized.
        //</summary>
        private bool Initialized = false;
        /// <summary>
        /// Internal CPU L1 cache.
        /// </summary>
        private long[] Cache = new long[0x9820];

        ///<summary>
        ///Program counter.
        ///</summary>
        long pc = 0;

        ///<summary>
        ///Stack pointer.
        ///</summary>
        long sp = 0;

        ///<summary>
        ///Flags register.
        ///</summary>
        long fl = 0;

        ///<summary>
        ///CPU clock cycle.
        ///</summary>
        private void ClockCycle() {
            if(!Initialized)
            {
                Console.WriteLine("Tried to cycle while CPU isn't running");
                throw new RunningWhileUnintializedException();
            }
            long CurrentInstruction = SystemMemory.Read(pc);
            if(pc == 0x4999)
            {
                Console.WriteLine("Done executing program!");
                Environment.Exit(0);
            }
            switch(CurrentInstruction) {
                case 0x00:
                    Debug.WriteLine("nop");
                    pc++;
                    break;
                case 0x01:
                    pc = 0;
                    sp = 0;
                    Debug.WriteLine("hlt");
                    Environment.Exit(0);
                    break;
                case 0x02:
                    SystemMemory.Write(SystemMemory.Read(pc + 1), (int)SystemMemory.Read(pc + 2) + (int)SystemMemory.Read(pc + 3));
                    if(SystemMemory.Read(pc + 1) == 0) fl |= 2L << 0;
                    Debug.WriteLine("add");
                    pc += 6;
                    break;
                case 0x03:
                    SystemMemory.Write(SystemMemory.Read(pc + 1), (int)SystemMemory.Read(pc + 2) + (int)SystemMemory.Read(pc + 3));
                    if(SystemMemory.Read(pc + 1) == 0) fl |= 2L << 0;
                    Debug.WriteLine("sub");
                    pc += 6;
                    break;
                case 0x04:
                    SystemMemory.Write(SystemMemory.Read(pc + 1), (int)SystemMemory.Read(pc + 2) + (int)SystemMemory.Read(pc + 3));
                    if(SystemMemory.Read(pc + 1) == 0) fl |= 2L << 0;
                    Debug.WriteLine("mul");
                    pc += 6;
                    break;
                case 0x05:
                    SystemMemory.Write(SystemMemory.Read(pc + 1) = (int)SystemMemory.Read(pc + 2) + (int)SystemMemory.Read(pc + 3));
                    if(SystemMemory.Read(pc + 1) == 0) fl |= 2L << 0;
                    Debug.WriteLine("div");
                    pc += 6;
                    break;
                case 0x06:
                    SystemMemory.Write(SystemMemory.Read(pc + 1), (int)SystemMemory.Read(pc + 2) & (int)SystemMemory.Read(pc + 3));
                    if(SystemMemory.Read(pc + 1) == 0) fl |= 2L << 0;
                    Debug.WriteLine("and");
                    pc += 6;
                    break;
                case 0x07:
                    SystemMemory.Write(SystemMemory.Read(pc + 1) , (int)SystemMemory.Read(pc + 2) | (int)SystemMemory.Read(pc + 3));
                    if(SystemMemory.Read(pc + 1) == 0) fl |= 2L << 0;
                    Debug.WriteLine("or");
                    pc += 6;
                    break;
                case 0x08:
                    SystemMemory.Write(SystemMemory.Read(pc + 1), ~((int)SystemMemory.Read(pc + 2) ^ (int)SystemMemory.Read(pc + 3)));
                    if(SystemMemory.Read(pc + 1) == 0) fl |= 2L << 0;
                    Debug.WriteLine("not");
                    pc += 6;
                    break;
                case 0x09:
                    SystemMemory.Write(SystemMemory.Read(pc + 1), (int)SystemMemory.Read(pc + 2) ^ (int)SystemMemory.Read(pc + 3));
                    if(SystemMemory.Read(pc + 1) == 0) fl |= 2L << 0;
                    Debug.WriteLine("xor");
                    pc += 6;
                    break;
                case 0x0a:
                    SystemMemory.Write(SystemMemory.Read(pc + 1), 1);
                    Debug.WriteLine("inc");
                    pc += 2;
                    break;
                case 0x0b:
                    SystemMemory.Write(SystemMemory.Read(pc + 1), pc - 1);
                    if(SystemMemory.Read(pc + 1) == 0) fl |= 2L << 0;
                    Debug.WriteLine("dec");
                    pc += 2;
                    break;
                case 0x0c:
                    SystemMemory.Write(SystemMemory.Read(pc+ 1), (int)SystemMemory.Read(pc + 2) >> (int)SystemMemory.Read(pc + 3));
                    if(SystemMemory.Read(pc + 1) == 0) fl |= 2L << 0;
                    Debug.WriteLine("rsh");
                    pc += 6;
                    break;
                case 0x0d:
                    SystemMemory.Write(SystemMemory.Read(pc + 1), (int)SystemMemory.Read(pc + 2) << (int)SystemMemory.Read(pc + 3));
                    if(SystemMemory.Read(pc + 1) == 0) fl |= 2L << 0;
                    Debug.WriteLine("lsh");
                    pc += 6;
                    break;
                case 0x0e:
                    SystemMemory.Write(SystemMemory.Read(pc + 1), ((SystemMemory.Read(pc + 2) ^ SystemMemory.Read(pc + 3)) & 0x1f) >> (SystemMemory.Read(pc + 2) ^ SystemMemory.Read(pc + 3)) | ((SystemMemory.Read(pc + 2) ^ SystemMemory.Read(pc + 3)) & 0x1f) << ( 32 - (SystemMemory.Read(pc + 2) ^ SystemMemory.Read(pc + 3))));
                    if(SystemMemory.Read(pc + 1) == 0) fl |= 2L << 0;
                    Debug.WriteLine("ror");
                    pc += 6;
                    break;
                case 0x0f:
                    SystemMemory.Write(SystemMemory.Read(pc + 1), ((SystemMemory.Read(pc + 2) ^ SystemMemory.Read(pc + 3)) & 0x1f) << (SystemMemory.Read(pc + 2) ^ SystemMemory.Read(pc + 3)) | ((SystemMemory.Read(pc + 2) ^ SystemMemory.Read(pc + 3)) & 0x1f) >> ( 32 - (SystemMemory.Read(pc + 2) ^ SystemMemory.Read(pc + 3))));
                    if(SystemMemory.Read(pc + 1) == 0) fl |= 2L << 0;
                    Debug.WriteLine("rol");
                    pc += 6; 
                    break;
                case 0x11:
                    if(sp == 0x7000) throw new StackOverflowException();
                    SystemMemory.Write(sp++, SystemMemory.Read(pc++));
                    Debug.WriteLine("push");
                    pc++;
                    break;
                case 0x12:
                    if(sp == 0x5000) throw new StackUnderflowException();
                    SystemMemory.Write(sp--, SystemMemory.Read(pc++));
                    Debug.WriteLine("pop");
                    pc++;
                    break;
                case 0x13:
                    if(SystemMemory.Read(pc++) == SystemMemory.Read(pc + 2)) fl |= 1L << 0;
                    pc += 3;
                    Debug.WriteLine("cmp");
                    break;
                case 0x14:
                    pc = SystemMemory.Read(pc++);
                    Debug.WriteLine("jmp");
                    break;
                case 0x15:
                    if(SystemMemory.Read(pc++) & (1L << 0)) pc = SystemMemory.Read(pc++);
                    else pc += 2;
                    Debug.WriteLine("jeq");
                    break;
                case 0x16:
                    if(!SystemMemory.Read(pc++) & (1L << 0)) pc = SystemMemory.Read(pc++);
                    else pc += 2;
                    Debug.WriteLine("jneq");
                    break;
                case 0x17:
                    if(!SystemMemory.Read(pc++) & (2L << 0)) pc = SystemMemory.Read(pc++);
                    else pc += 2;
                    Debug.WriteLine("jz");
                    break;
                case 0x18:
                    if(SystemMemory.Read(pc++) & (2L << 0)) pc = SystemMemory.Read(pc++);
                    else pc += 2;
                    Debug.WriteLine("jnz");
                    break;
                case 0x19:
                    SystemMemory.Write(sp++, pc + 2);
                    pc = SystemMemory.Read(pc++);
                    Debug.WriteLine("call");
                    break;
                case 0x20:
                    pc = SystemMemory.Read(sp--);
                    Debug.WriteLine("ret");
                    break;
                case 0x21:
                    SystemMemory.Write(pc++, pc + 2);
                    pc += 3;
                    Debug.WriteLine("copy");
                    break;
                case 0x22:
                    SystemMemory.Write(pc++, pc + 2);
                    Debug.WriteLine("load");
                    break;
                default:
                    Debug.WriteLine("Uknown instruction " + pc + ".");
                    throw new InvalidInstructionException();
            }
        }

        /// <summary>
        /// Create a new CPU instance.
        /// </summary>
        /// <param name= "FileName">ROM to run.</param>
        /// <param name="Gfx">Toggle graphics mode.</param>
        public CPU(string FileName, bool Gfx)
        {
            if (Initialized)
            {
                Console.WriteLine("Attempted to initialize while processor is already initialized.");
                Environment.Exit(1);
            }
                      
            for(int i = 0; i < File.ReadAllBytes(ROM)[i]; i++)
            {
                if(i < 0x4999) continue;
                else break;
                SystemMemory.Write(i, File.ReadAllBytes(ROM)[i]);
            }
            this.sp = 0x5000;
            this.Initialized = true;
            ClockCycle();
        }
    }
}