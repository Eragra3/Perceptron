using System;

public static class ConsoleHelper
{
    public static void WriteErrorLine(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void WriteResponseLine(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(msg);
        Console.ForegroundColor = ConsoleColor.White;
    }
    public static void WriteResponse(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(msg);
        Console.ForegroundColor = ConsoleColor.White;
    }
    public static void WriteResponseLine(int msg)
    {
        WriteResponseLine(msg.ToString());
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
    public static void WriteLine()
    {
        Console.WriteLine();
    }
}