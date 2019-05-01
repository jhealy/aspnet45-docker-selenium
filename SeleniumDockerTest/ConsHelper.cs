using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumDockerTest
{
    public static class ConsHelper
    {
        static public ConsoleColor AlertColor { get; set; } = ConsoleColor.Yellow;
        static public ConsoleColor ErrColor { get; set; } = ConsoleColor.Red;
        static public ConsoleColor MsgColor { get; set; } = ConsoleColor.Green;

        public static void Info(string key, string val)
        {
            if (String.IsNullOrWhiteSpace(key) && String.IsNullOrWhiteSpace(val)) return;
            key = String.IsNullOrWhiteSpace(key) ? "<empty>" : key;
            val = String.IsNullOrWhiteSpace(val) ? "<empty>" : val;
            Info($"[{key}] =[{val}]");
        }

        public static void Info( string msg )
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            if (String.IsNullOrWhiteSpace(msg)) return;
            Console.ForegroundColor = ConsHelper.AlertColor;
            Console.WriteLine(msg);
            Console.ForegroundColor = oldColor;
        }

        public static void Err(string key, string val)
        {
            if (String.IsNullOrWhiteSpace(key) && String.IsNullOrWhiteSpace(val)) return;
            key = String.IsNullOrWhiteSpace(key) ? "<empty>" : key;
            val = String.IsNullOrWhiteSpace(val) ? "<empty>" : val;

            Err($"[{key}] =[{val}]");
        }

        public static void Err( string msg )
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            if (String.IsNullOrWhiteSpace(msg)) return;
            Console.ForegroundColor = ConsHelper.ErrColor;
            Console.WriteLine(msg);
            Console.ForegroundColor = oldColor;
        }

        public static void Msg(string key, string val)
        {
            if (String.IsNullOrWhiteSpace(key) && String.IsNullOrWhiteSpace(val)) return;
            key = String.IsNullOrWhiteSpace(key) ? "<empty>" : key;
            val = String.IsNullOrWhiteSpace(val) ? "<empty>" : val;

            Msg($"[{key}] =[{val}]");
        }

        public static void Msg(string msg)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsHelper.MsgColor;
            if (String.IsNullOrWhiteSpace(msg)) return;
            Console.WriteLine(msg);
            Console.ForegroundColor = oldColor;
        }

        public static void NL()
        {
            Console.WriteLine();
        }

        public static string Pause()
        {
            ConsHelper.Msg("Hit any key to continue");
            return Console.ReadLine();
        }
    }
}
