using System;

namespace ZFX {
    ///<summary>
    /// Device interface
    ///</summary>
    public interface Device {
        string Name {}
        sbyte Send();
        void Receive(sbyte Data);
        void Tick();
        void Destroy();
    }
}