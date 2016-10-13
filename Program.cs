using System;
using System.Collections.Generic;
using System.Linq;
using static ConsoleHelper;
using static ExperimentRunner;

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
            WriteErrorLine("Invalid learning rate param - setting as 0.2");
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
            WriteErrorLine("Invalid initial weights limit - setting as 1");
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
        Console.WriteLine($"{"train".PadRight(10)} (train) - train perceptron and function");
        Console.WriteLine($"{"change".PadRight(10)} (change) - create new perceptron");
        Console.WriteLine($"{"e".PadRight(10)} (e) - run experiment");
        Console.WriteLine($"{"es".PadRight(10)} (es) - toggle silent mode for experiments");
        Console.WriteLine($"{"octave".PadRight(10)} (octave) - generate octave function plot");
        Console.WriteLine("-".PadRight(20, '-'));

        string line;
        ExperimentRunner.Silent = false;
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
                        WriteErrorLine("Wrong command");
                    }
                    break;
                case "p":
                    WriteResponseLine(perceptron.Dump());
                    break;
                case "d":
                    WriteResponseLine(perceptron.Dump());
                    break;
                case "test":
                    PerceptronTrainer.Test_And(perceptron);
                    break;
                case "train":
                    PerceptronTrainer.TrainPerceptron_And(perceptron);
                    break;
                case "change":
                    WriteResponseLine("learning-rate weights-range step-function(uni,bi) use-adaline(y,n or nothing)");
                    try
                    {
                        var parameters = Prompt().Split(' ');
                        learningRate = double.Parse(parameters[0].Replace('.', ','));
                        initialWeightLimit = double.Parse(parameters[1].Replace('.', ','));
                        StepFunctionEnum stepFunction;
                        if (parameters[2] == "uni") stepFunction = StepFunctionEnum.Unipolar;
                        else if (parameters[2] == "bi") stepFunction = StepFunctionEnum.Bipolar;
                        else throw new ArgumentException();

                        bool useAdaline = false;
                        if (parameters[3] == "y") useAdaline = true;


                        perceptron = PerceptronTrainer.CreatePerceptron(learningRate, initialWeightLimit, 2, stepFunction, useAdaline);
                    }
                    catch (PerceptronLearnException)
                    {

                    }
                    catch (System.Exception)
                    {
                        WriteErrorLine("Invalid parameters");
                    }
                    break;
                case "e":
                    WriteResponseLine("step-function(uni,bi)");
                    {
                        StepFunctionEnum stepFunction;
                        var userInput = Prompt();
                        if (userInput == "uni") stepFunction = StepFunctionEnum.Unipolar;
                        else if (userInput == "bi") stepFunction = StepFunctionEnum.Bipolar;
                        else throw new ArgumentException();

                        WriteResponseLine("learning-rate(lr) weights-range(wr)");
                        userInput = Prompt();
                        if (userInput == "lr" || userInput == "learning-rate")
                        {
                            try
                            {
                                WriteResponseLine("type in {d} to use defaults 0.1 1 0.1 10 1");
                                WriteResponseLine("start end step repetitions initialWeightLimit");
                                var parameters = Prompt().Split(' ');

                                if (parameters[0] == "d")
                                {
                                    ExperimentRunner.LearningRate(0.1, 1, 0.1, 10, 1, stepFunction);
                                }
                                else
                                {
                                    var start = double.Parse(parameters[0].Replace('.', ','));
                                    var end = double.Parse(parameters[1].Replace('.', ','));
                                    var step = double.Parse(parameters[2].Replace('.', ','));
                                    var repetitions = int.Parse(parameters[3]);
                                    initialWeightLimit = double.Parse(parameters[4].Replace('.', ','));
                                    ExperimentRunner.LearningRate(start, end, step, repetitions, initialWeightLimit, stepFunction);
                                }
                            }
                            catch (PerceptronLearnException)
                            {
                                WriteErrorLine("Oops. Learning exception.");
                            }
                            catch (System.Exception)
                            {
                                WriteErrorLine("Invalid parameters");
                            }
                        }
                        else if (userInput == "wr" || userInput == "weights-range")
                        {
                            try
                            {
                                WriteResponseLine("type in {d} to use defaults 0.1 1 0.1 10 1");
                                WriteResponseLine("start end step repetitions learningRate");
                                var parameters = Prompt().Split(' ');

                                if (parameters[0] == "d")
                                {
                                    ExperimentRunner.WeightsRange(0.1, 1, 0.1, 10, 1, stepFunction);
                                }
                                else
                                {
                                    var start = double.Parse(parameters[0].Replace('.', ','));
                                    var end = double.Parse(parameters[1].Replace('.', ','));
                                    var step = double.Parse(parameters[2].Replace('.', ','));
                                    var repetitions = int.Parse(parameters[3]);
                                    learningRate = double.Parse(parameters[4].Replace('.', ','));
                                    ExperimentRunner.WeightsRange(start, end, step, repetitions, learningRate, stepFunction);
                                }
                            }
                            catch (PerceptronLearnException)
                            {
                                WriteErrorLine("Oops. Learning exception.");
                            }
                            catch (System.Exception)
                            {
                                WriteErrorLine("Invalid parameters");
                            }
                        }
                    }
                    break;
                case "es":
                    ExperimentRunner.Silent = !ExperimentRunner.Silent;
                    WriteResponseLine($"Experiment runner mode - {(ExperimentRunner.Silent ? "silent" : "verbose")}");
                    break;
                case "octave":
                    var octaveCode = perceptron?.GenerateOctaveCode();
                    WriteResponseLine(octaveCode);
                    break;
                default:
                    WriteErrorLine("No such command");
                    break;
            }
            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine("\n".PadLeft(3));
        Console.WriteLine("Perceptron end");
    }

}