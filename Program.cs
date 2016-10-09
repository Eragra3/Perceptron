using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Perceptron starting!");
        Console.WriteLine($"params: {args.Aggregate("", (acc, a) => acc += " " + a)}");
        double learningRate = double.Parse(args[0].Replace('.', ','));
        Console.WriteLine($"learning rate: {learningRate}");
        Console.WriteLine("-".PadLeft(20));
        Console.WriteLine("-".PadLeft(20));
        Console.WriteLine("-".PadLeft(20));
        Console.WriteLine("\n".PadLeft(3));

        #region INITIALIZE PERCEPTRON 
        int inputCount = 2;
        Random rng = new Random();
        double[] initialWeights = new double[inputCount];
        for (int i = 0; i < initialWeights.Length; i++)
        {
            initialWeights[i] = rng.NextDouble() * 2 - 1;
        }
        double bias = rng.NextDouble() * 2 - 1;

        Perceptron perceptron = new Perceptron(initialWeights, learningRate, bias);
        #endregion

        #region TRAIN DATA
        IList<TrainObject> trainData = new List<TrainObject>(10);
        TrainObject t1 = new TrainObject(new double[] { 0, 0 }, 0);
        TrainObject t2 = new TrainObject(new double[] { 0, 1 }, 0);
        TrainObject t3 = new TrainObject(new double[] { 1, 0 }, 0);
        TrainObject t4 = new TrainObject(new double[] { 1, 1 }, 1);
        trainData.Add(t1);
        trainData.Add(t2);
        trainData.Add(t3);
        trainData.Add(t4);
        #endregion

        #region TRAIN PERCEPTRON
        var learnFunction = false;
        if (learnFunction)
        {
            Func<double, double, int> f = (x1, x2) => x1 * 1337 + x2 + 69 > 0 ? 1 : 0;
            for (int i = 0; i < 100000; i++)
            {
                var x1 = (rng.NextDouble() - 0.5) * 1000;
                var x2 = (rng.NextDouble() - 0.5) * 1000;
                var s = f(x1, x2);
                var trainObject = new TrainObject(new[] { x1, x2 }, s);
                perceptron.Train(trainObject);
            }
        }
        else
        {
            bool isTrained = false;
            int epoch = 1;
            while (!isTrained)
            {
                Console.WriteLine($"Learning epoch - {epoch++}");
                isTrained = true;
                foreach (var trainObject in trainData)
                {
                    var error = perceptron.Train(trainObject.Input, trainObject.Solution);
                    if (error != 0)
                    {
                        isTrained = false;
                    }
                }
                if (epoch > 10000)
                {
                    throw new Exception();
                }
            }
        }
        #endregion

        Console.WriteLine("Starting REPL");
        Console.WriteLine("-".PadRight(20, '-'));

        string line;
        while (true)
        {
            line = Console.ReadLine();
            line = line.Replace('.', ',');
            var input = line.Split(' ');
            var command = input[0];

            if (command == "q") break;
            if (command == "exit") break;
            if (command == "") break;

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
                        Console.WriteLine(decision);
                    }
                    catch (System.Exception)
                    {
                        Console.WriteLine("Wrong command");
                    }
                    break;
                case "p":
                    Console.WriteLine(perceptron.Dump());
                    break;
                case "d":
                    Console.WriteLine(perceptron.Dump());
                    break;
                case "test":
                    Console.WriteLine($"x1 | x2 | y | decision");
                    foreach (var to in trainData)
                    {
                        var decision = perceptron.Feedforward(to.Input);
                        Console.Write(to.Input[0].ToString().PadRight(3) + "|");
                        Console.Write(" " + to.Input[1].ToString().PadRight(3) + "|");
                        Console.Write(" " + to.Solution.ToString().PadRight(2) + "|");
                        Console.Write(" " + decision);
                        Console.Write(decision == to.Solution ? "" : " [x]");
                        Console.WriteLine();
                    }
                    break;
                default:
                    break;
            }
            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine("\n".PadLeft(3));
        Console.WriteLine("Perceptron end");
    }
}