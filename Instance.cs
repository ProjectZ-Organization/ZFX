using System;
using System.Threading;
using System.IO;
using ZFX.CPU;

namespace ZFX {
    class Instance {
        public Instance(int Cores, int Memory, string ROM) {
            CPU[] Processors = new CPU {Cores};
            foreach(var Processor in Processors) {
                //FIXME: Not all cores execute the same program.
                Thread CurrentProcessorThread = new Thread(new ThreadStart(new CPU(Memory, ROM)));
                CurrentProcessorThread.Start();
            }
        }
    }
}