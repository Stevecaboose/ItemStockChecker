using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace ItemStockChecker
{
    public static class ConsoleOutputHelper
    {

        private static readonly object ConsoleWriterLock = new object();

        public static void Write(string message)
        {
            WriteColor(message, ConsoleColor.White);
        }

        public static void Write(string message, ConsoleColor color)
        {
            WriteColor(message, color);
        }

        public static void Write(string message, ConsoleColor color, Thread thread)
        {
            WriteColor(message, color, thread);
        }

        public static void Write(string message, Thread thread)
        {
            WriteColor(message, ConsoleColor.White, thread);
        }

        public static void Write(Exception message)
        {
            WriteSolidColor(message.ToString(), ConsoleColor.DarkRed);
        }

        public static void Write(string preMessage, Exception message)
        {
            WriteSolidColor(message.ToString(), ConsoleColor.DarkRed, preMessage);
        }

        private static void WriteSolidColor(string message, ConsoleColor color, string extraMessage = "")
        {
            lock (ConsoleWriterLock)
            {
                Console.ForegroundColor = color;

                if (String.IsNullOrWhiteSpace(extraMessage))
                {
                    Console.Write(message);
                }
                else
                {
                    Console.Write(extraMessage + Environment.NewLine + message);
                }

                Console.ResetColor();
                Console.WriteLine();
            }
        }

        private static void WriteColor(string message, ConsoleColor color, Thread thread = null)
        {
            lock (ConsoleWriterLock)
            {
                var pieces = Regex.Split(message, @"(\[[^\]]*\])");

                if (thread == null)
                {
                    Console.Write(DateTime.Now + " ");
                }
                else
                {
                    Print(thread);
                }


                foreach (var t in pieces)
                {
                    string piece = t;

                    if (piece.StartsWith("[") && piece.EndsWith("]"))
                    {
                        Console.ForegroundColor = color;
                        piece = piece.Substring(1, piece.Length - 2);
                    }

                    Console.Write(piece);

                    Console.ResetColor();
                }

                Console.WriteLine();
            }

        }

        private static void Print(Thread thread, string message = "")
        {
            lock (ConsoleWriterLock)
            {
                Console.Write("Item ID: " + thread.ManagedThreadId + " " + DateTime.Now + " " + message);
            }
        }
    }
}
