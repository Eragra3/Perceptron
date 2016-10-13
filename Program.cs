using System;
using System.Collections.Generic;
using System.Linq;
using static ConsoleHelper;

public class Program
{
    public static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Perceptron starting!");
        Console.WriteLine($"params: {args.Aggregate("", (acc, a) => acc += " " + a)}");
        double learningRate;
        try
        {
            learningRate = double.Parse(args[0].Replace('.', ','));
        }
        catch (System.Exception)
        {
            WriteError("Invalid learning rate param - setting as 0.2");
            learningRate = 0.2;
        }
        Console.WriteLine($"learning rate: {learningRate}");

        double initialWeightLimit;
        try
        {
            initialWeightLimit = double.Parse(args[1].Replace('.', ','));
        }
        catch (System.Exception)
        {
            WriteError("Invalid initial weights limit - setting as 1");
            initialWeightLimit = 0.2;
        }
        Console.WriteLine($"initial weight range: -{initialWeightLimit}:{initialWeightLimit}");

        Console.WriteLine("\n".PadLeft(3));

        var perceptron = PerceptronTrainer.CreatePerceptron(learningRate, initialWeightLimit, 2);

        Console.WriteLine("Starting REPL");
        Console.WriteLine("-".PadRight(20, '-'));
        Console.WriteLine($"{"t".PadRight(10)} (t {{x1}} {{x2}}) - test for input");
        Console.WriteLine($"{"p".PadRight(10)} (p) - print perceptron data");
        Console.WriteLine($"{"d".PadRight(10)} (d) - print perceptron data");
        Console.WriteLine($"{"test".PadRight(10)} (test) - test and function");
        Console.WriteLine($"{"change".PadRight(10)} (change) - create new perceptron");
        Console.WriteLine($"{"e".PadRight(10)} (e) - run experiment");
        Console.WriteLine("-".PadRight(20, '-'));

        string line;
        while (true)
        {
            line = Prompt();
            line = line.Replace('.', ',');
            var input = line.Split(' ');
            var command = input[0];

            if (command == "q") break;
            if (command == "exit") break;

            Console.WriteLine();
            switch (command)
            {
                case "t":
                    try
                    {
                        var decision = perceptron.Feedforward(
                                double.Parse(input[1]),
                                double.Parse(input[2])
                                );
                        WriteResponseLine(decision);
                    }
                    catch (System.Exception)
                    {
                        WriteError("Wrong command");
                    }
                    break;
                case "p":
                    WriteResponseLine(perceptron.Dump());
                    break;
                case "d":
                    WriteResponseLine(perceptron.Dump());
                    break;
                case "test":
                    PerceptronTrainer.Check_And(perceptron);
                    break;
                case "train":
                    PerceptronTrainer.TrainPerceptron_And(perceptron);
                    break;
                case "change":
                    WriteResponseLine("learning-rate weights-range");
                    try
                    {
                        var parameters = Prompt().Split(' ');
                        learningRate = double.Parse(parameters[0].Replace('.', ','));
                        initialWeightLimit = double.Parse(parameters[1].Replace('.', ','));

                        perceptron = PerceptronTrainer.CreatePerceptron(learningRate, initialWeightLimit, 2);
                    }
                    catch (System.Exception)
                    {
                        WriteError("Invalid parameters");
                    }
                    break;
                case "e":
                    WriteResponseLine("learning-rate(lr) weights-range(wr)");
                    var experiment = Prompt();
                    if (experiment == "lr" || experiment == "learning-rate")
                    {

                    }
                    else if (experiment == "wr" || experiment == "weights-range")
                    {

                    }
                    break;
                default:
                    WriteError("No such command");
                    break;
            }
            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine("\n".PadLeft(3));
        Console.WriteLine("Perceptron end");
    }

}