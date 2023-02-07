using System;
using System.Threading;
using System.IO;
using ZFX.CPU;

namespace ZFX {
    class Instance {
        /// <summary>
        /// System Memory.
        /// </summary>
        private Memory SystemMemory;
        public Instance(int Cores, int MemoryCount, string ROM) {
            CPU[] Processors = new CPU {Cores};
            foreach(var Processor in Processors) {
                //FIXME: Not all cores execute the same program.
                SystemMemory = new Memory(MemoryCount);
                Thread CurrentProcessorThread = new Thread(new ThreadStart(Processor(ROM)));
                CurrentProcessorThread.Start();
            }
        }
    }
}