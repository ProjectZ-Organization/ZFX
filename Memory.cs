using System;

namespace ZFX {
    class Memory {

        private long[] RAM;
        private void Initialized = false;

        ///<summary>
        ///Read an index from memory.
        ///</summary>
        public long Read(int Index) {
            return RAM[Index];
        }

        ///<summary>
        ///Write to a memory index.
        ///</summary>
        public void Write(int Index, long Value) {
            RAM[Index] = Value;
        }

        public Memory(int Memory) {
            if(Initialized) return;
            RAM = Memory;
            Initialized = true;
        }
    }
}