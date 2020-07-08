using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace MizuiroBot.Tools
{
    public static class CVTS
    {

#if Windows
        [DllImport("kernel32.dll")]
        static extern IntPtr GetStdHandle(int dword);

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr handle, out int flags);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr handle, int flags);

        const int ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        public const char EscapeChar = '\x1B';

        static IntPtr consoleOutputBuffer = GetStdHandle(-11);
#endif

        public static bool TryEnable()
        {
#if Windows
            try
            {
                int currentFlags;
                GetConsoleMode(consoleOutputBuffer, out currentFlags);
                SetConsoleMode(consoleOutputBuffer, currentFlags | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
                return true;
            } catch
            {
                return false;
            }
#endif
#if Linux
            CVTS.WriteLineInfo("No need to enable CVTS, terminal already supports ANSI codes.");
            return true;
#endif
        }

        public static string RgbForeColorString(byte r, byte g, byte b)
        {
#if Windows
            return $"{EscapeChar}[38;2;{r};{g};{b}m";
#endif
#if Linux
            // Maybe i'll come back to this later, bash doesn't support full-colour ANSI escape sequences :(
            return $"";
#endif
        }

        public static string RgbForeColorString(Color color)
        {
            return RgbForeColorString(color.R, color.G, color.B);
        }

        public static string RgbBackColorString(byte r, byte g, byte b)
        {
#if Windows
            return $"{EscapeChar}[48;2;{r};{g};{b}m";
#endif
#if Linux
            // Maybe i'll come back to this later, bash doesn't support full-colour ANSI escape sequences :(
            return $"";
#endif
        }

        public static string RgbBackColorString(Color color)
        {
            return RgbBackColorString(color.R, color.G, color.B);
        }

        public static void WriteLine(string text, Color foreColor, string prefix)
        {
            Console.WriteLine($"[{RgbForeColorString(foreColor)}{prefix}{RgbForeColorString(Color.White)}] {text}");
        }

        public static void WriteLineInfo(string text)
        {
            WriteLine(text, Color.FromArgb(3, 198, 252), "Info");
        }

        public static void WriteLineOk(string text)
        {
            WriteLine(text, Color.FromArgb(140, 220, 80), "OK");
        }

        public static void WriteLineWarn(string text)
        {
            WriteLine(text, Color.FromArgb(220, 220, 80), "Warn");
        }

        public static void WriteLineError(string text)
        {
            WriteLine(text, Color.FromArgb(220, 80, 25), "Error");
        }

        public static void WriteLineTwitch(string text)
        {
            WriteLine(text, Color.FromArgb(140, 80, 220), "Twitch");
        }

        public static void WriteLineDiscord(string text)
        {
            WriteLine(text, Color.FromArgb(107, 147, 255), "Discord");
        }

        public static void WriteLineShared(string text)
        {
            WriteLine(text, Color.FromArgb(255, 0, 132), "Shared");
        }
    }
}
