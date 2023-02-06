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
        long sp = 0;

        ///<summary>
        ///Flags register.
        ///</summary>
        long fl = 0;

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
                    pc = 0;
                    sp = 0;
                    Debug.WriteLine("hlt");
                    Environment.Exit(0);
                    break;
                case 0x02:
                    RAM[pc++] = (int)RAM[pc + 2] + (int)RAM[pc + 3];
                    if(RAM[pc++] == 0) fl |= 2L << 0;
                    Debug.WriteLine("add");
                    pc += 6;
                    break;
                case 0x03:
                    RAM[pc++] = (int)RAM[pc + 2] + (int)RAM[pc + 3];
                    if(RAM[pc++] == 0) fl |= 2L << 0;
                    Debug.WriteLine("sub");
                    pc += 6;
                    break;
                case 0x04:
                    RAM[pc++] = (int)RAM[pc + 2] + (int)RAM[pc + 3];
                    if(RAM[pc++] == 0) fl |= 2L << 0;
                    Debug.WriteLine("mul");
                    pc += 6;
                    break;
                case 0x05:
                    RAM[pc++] = (int)RAM[pc + 2] + (int)RAM[pc + 3];
                    if(RAM[pc++] == 0) fl |= 2L << 0;
                    Debug.WriteLine("div");
                    pc += 6;
                    break;
                case 0x06:
                    RAM[pc++] = (int)RAM[pc + 2] & (int)RAM[pc + 3];
                    if(RAM[pc++] == 0) fl |= 2L << 0;
                    Debug.WriteLine("and");
                    pc += 6;
                    break;
                case 0x07:
                    RAM[pc++] = (int)RAM[pc + 2] | (int)RAM[pc + 3];
                    if(RAM[pc++] == 0) fl |= 2L << 0;
                    Debug.WriteLine("or");
                    pc += 6;
                    break;
                case 0x08:
                    RAM[pc++] = ~((int)RAM[pc + 2] ^ (int)RAM[pc + 3]);
                    if(RAM[pc++] == 0) fl |= 2L << 0;
                    Debug.WriteLine("not");
                    pc += 6;
                    break;
                case 0x09:
                    RAM[pc++] = (int)RAM[pc + 2] ^ (int)RAM[pc + 3];
                    if(RAM[pc++] == 0) fl |= 2L << 0;
                    Debug.WriteLine("xor");
                    pc += 6;
                    break;
                case 0x0a:
                    RAM[pc++]++;
                    Debug.WriteLine("inc");
                    pc += 2;
                    break;
                case 0x0b:
                    RAM[pc++]--;
                    if(RAM[pc++] == 0) fl |= 2L << 0;
                    Debug.WriteLine("dec");
                    pc += 2;
                    break;
                case 0x0c:
                    RAM[pc++] = (int)RAM[pc + 2] >> (int)RAM[pc + 3];
                    if(RAM[pc++] == 0) fl |= 2L << 0;
                    Debug.WriteLine("rsh");
                    pc += 6;
                    break;
                case 0x0d:
                    RAM[pc++] = (int)RAM[pc + 2] << (int)RAM[pc + 3];
                    if(RAM[pc++] == 0) fl |= 2L << 0;
                    Debug.WriteLine("lsh");
                    pc += 6;
                    break;
                case 0x0e:
                    RAM[pc++] = ((RAM[pc + 2] ^ RAM[pc + 3]) & 0x1f) >> (RAM[pc + 2] ^ RAM[pc + 3]) | ((RAM[pc + 2] ^ RAM[pc + 3]) & 0x1f) << ( 32 - (RAM[pc + 2] ^ RAM[pc + 3]));
                    if(RAM[pc++] == 0) fl |= 2L << 0;
                    Debug.WriteLine("ror");
                    pc += 6;
                    break;
                case 0x0f:
                    RAM[pc++] = ((RAM[pc + 2] ^ RAM[pc + 3]) & 0x1f) << (RAM[pc + 2] ^ RAM[pc + 3]) | ((RAM[pc + 2] ^ RAM[pc + 3]) & 0x1f) >> ( 32 - (RAM[pc + 2] ^ RAM[pc + 3]));
                    if(RAM[pc++] == 0) fl |= 2L << 0;
                    Debug.WriteLine("rol");
                    pc += 6; 
                    break;
                case 0x11:
                    RAM[sp++] = RAM[pc++];
                    Debug.WriteLine("push");
                    pc++;
                    break;
                case 0x12:
                    RAM[sp--] = RAM[pc++];
                    Debug.WriteLine("pop");
                    pc++;
                    break;
                case 0x13:
                    if(RAM[pc++] == RAM[pc + 2]) fl |= 1L << 0;
                    pc += 3;
                    Debug.WriteLine("cmp");
                    break;
                case 0x14:
                    pc = RAM[pc++];
                    Debug.WriteLine("jmp");
                    break;
                case 0x15:
                    if(RAM[pc++] & (1L << 0)) pc = RAM[pc++];
                    else pc += 2;
                    Debug.WriteLine("jeq");
                    break;
                case 0x16:
                    if(!RAM[pc++] & (1L << 0)) pc = RAM[pc++];
                    else pc += 2;
                    Debug.WriteLine("jneq");
                    break;
                case 0x17:
                    if(!RAM[pc++] & (2L << 0)) pc = RAM[pc++];
                    else pc += 2;
                    Debug.WriteLine("jz");
                    break;
                case 0x18:
                    if(RAM[pc++] & (2L << 0)) pc = RAM[pc++];
                    else pc += 2;
                    Debug.WriteLine("jnz");
                    break;
                case 0x19:
                    RAM[sp++] = pc + 2;
                    pc = RAM[pc++];
                    Debug.WriteLine("call");
                    break;
                case 0x20:
                    pc = RAM[sp--];
                    Debug.WriteLine("ret");
                    break;
                default:
                    Debug.WriteLine("Uknown instruction " + pc + ".");
                    throw new InvalidInstructionException();
            }
        }

        /// <summary>
        /// Create a new CPU instance.
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
        private void initd(long Memory, string ROM, bool Gfx)
        {
            if (init)
            {
                Console.WriteLine("Attempted to init while processor is already initialized.");
                Environment.Exit(1);
            }
                      
            this.RAM = new long[Memory];
            for(int i = 0; i < File.ReadAllBytes(ROM)[i]; i++)
            {
                if(i < 0x4999) continue;
                this.RAM[i] = File.ReadAllBytes(ROM)[i];
            }
            this.sp = 0x5000;
            this.init = true;
            ClockCycle();
        }
    }
}