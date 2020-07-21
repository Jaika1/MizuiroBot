using System;
using System.Collections.Generic;
using System.Text;

namespace MizuiroBot.Tools
{
    public static class ASCIIUI
    {
        public static string CreateTextBar(int value, int max, int chars = 10)
        {
            int barFillSize = (int)Math.Min(Math.Floor(((float)value / (float)max) * (float)chars), chars);
            return '\x2551' + new string('\x2588', barFillSize) + new string('\x2591', chars - barFillSize) + '\x2551';
        }
    }
}
