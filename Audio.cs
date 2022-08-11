using System;

namespace ZFX { 
    class Audio
    {
        /// <summary>
        /// Play a certain Frequency
        /// </summary>
        /// <param name="Frequency"></param>
        /// <param name="Duration"></param>
        public void PlayFrequency(int Frequency, int Duration)
        {
            Console.Beep(Frequency, Duration);
        }
    } 
}