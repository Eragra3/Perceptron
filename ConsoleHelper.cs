using System;

public static class ConsoleHelper
{
    public static void WriteError(string msg)
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

    public static string ReadLine()
    {
        Console.Write(">");
        return Console.ReadLine();
    }
}