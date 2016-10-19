using System;

namespace Perceptron
{
    public static class ConsoleHelper
    {
        public static void WriteErrorLine(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteYellowLine(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void WriteYellow(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void WriteYellowLine(int msg)
        {
            WriteYellowLine(msg.ToString());
        }

        public static string Prompt()
        {
            Console.Write(">");
            return Console.ReadLine();
        }
        public static void WriteExperimentLine(int msg)
        {
            WriteExperimentLine(msg.ToString());
        }

        public static void WriteExperimentLine(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void WriteExperiment(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void WriteLine(double msg)
        {
            Console.WriteLine(msg);
        }
        public static void WriteLine(string msg)
        {
            Console.WriteLine(msg);
        }
        public static void WriteLine()
        {
            Console.WriteLine();
        }
        public static void Write(double msg)
        {
            Console.Write(msg);
        }
        public static void Write(string msg)
        {
            Console.Write(msg);
        }
    }
}