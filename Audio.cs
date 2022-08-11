using System;

namespace ZFX { 
    class Audio
    {
        /// <summary>
        /// Play a certain Frequency
        /// </summary>
        /// <paramref name="Frequency">Frequency of the beep</paramref>
        /// <paramref name="Duration">Duration of the beep</paramref>
        public void PlayFrequency(int Frequency, int Duration)
        {
            Console.Beep(Frequency, Duration);
        }
    } 
}