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
        public bool init = false;
        /// <summary>
        /// RAM array
        /// </summary>
        public long[] RAM { get; private set; }
        ///<summary>
        ///Size of RAM in KB.
        ///</summary>
        public long bitsize { get; private set; }

        ///<summary>
        ///Program counter.
        ///</summary>
        long pc = 0;

        ///<summary>
        ///Stack pointer.
        ///</summary>
        long sp;

        ///<summary>
        ///CPU clock cycle.
        ///</summary>
        public void ClockCycle() {
            if(!init){
                Console.WriteLine("Tried to cycle while CPU isn't running");
                throw new RunningWhileUnintializedException();
            }
            long CurrentInstruction = RAM[pc];
            switch(CurrentInstruction) {
                case 0x00:
                    Debug.WriteLine("nop");
                    pc++;
                    break;
                case 0x01:
                    Debug.WriteLine("hlt");
                    hlt();
                    break;
                case 0x02:
                    add((int)RAM[pc + 1], (int)RAM[pc + 2], (int)RAM[pc + 3]);
                    Debug.WriteLine("add");
                    pc += 6;
                    break;
                case 0x03:
                    sub((int)RAM[pc +1], (int)RAM[pc + 2], (int)RAM[pc + 3]);
                    Debug.WriteLine("sub");
                    pc += 6;
                    break;
                case 0x04:
                    mul((int)RAM[pc + 1], (int)RAM[pc + 2], (int)RAM[pc + 3]);
                    Debug.WriteLine("mul");
                    pc += 6;
                    break;
                case 0x05:
                    div((int)RAM[pc + 1], (int)RAM[pc + 2], (int)RAM[pc + 3]);
                    Debug.WriteLine("div");
                    pc += 6;
                    break;
                case 0x06:
                    inc((int)RAM[pc] + 1);
                    Debug.WriteLine("inc");
                    pc += 2;
                case 0x07:
                    dec((int)RAM[pc + 1]);
                    Debug.WriteLine("dec");
                    pc += 2;
                case 0x08:
                    RAM[pc++] = RAM[pc + 2] >> RAM[pc + 3];
                    Debug.WriteLine("rsh");
                    pc += 6;
                case 0x09:
                    RAM[pc++] = RAM[pc + 2] << RAM[pc + 3];
                    Debug.WriteLine("lsh");
                    pc += 6;
                case 0x0a:
                    RAM[sp++] = RAM[pc++];
                    Debug.WriteLine("push");
                    pc++;
                case 0x0b:
                    RAM[sp--] = RAM[pc++];
                    Debug.WriteLine("pop");
                    pc++;
                default:
                    Debug.WriteLine("Uknown instruction " + pc + ".");
                    throw new InvalidInstructionException();
            }
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="bitSystem">Size of RAM in KB</param>
        /// <param name = "FileName">ROM to run</param>
        public CPU(long bitSystem, string FileName, bool Gfx)
        {
            initd(bitSystem * 1024, FileName, Gfx);
        }

        /// <summary>
        /// Internal init function, Can only be run once.
        /// </summary>
        /// <param name="Memory">Amount of memory to allocate</param>
        /// <param name = "ROM">ROM filename</param>
        /// <param name = "Gfx">Toggle graphics mode</param>
        private void initd(long Memory, string ROM, bool Gfx)
        {
            if (init)
            {
                Console.WriteLine("Attempted to run init while emulator is already initialized.");
                Environment.Exit(1);
            }
                      
            RAM = new long[Memory];
            for(int i = 0; i < File.ReadAllBytes(ROM); i++) 
            {
                RAM[i] = File.ReadAllBytes(ROM)[i];
            }
            init = true;
            ClockCycle();
        }
    }
}