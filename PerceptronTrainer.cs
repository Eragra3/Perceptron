using System;
using System.Collections.Generic;
using static ConsoleHelper;

public static class PerceptronTrainer
{
    public static IList<TrainObject> andTraingData;
    static PerceptronTrainer()
    {
        andTraingData = new List<TrainObject>(10);
        TrainObject t1 = new TrainObject(new double[] { 0, 0 }, 0);
        TrainObject t2 = new TrainObject(new double[] { 0, 1 }, 0);
        TrainObject t3 = new TrainObject(new double[] { 1, 0 }, 0);
        TrainObject t4 = new TrainObject(new double[] { 1, 1 }, 1);
        andTraingData.Add(t1);
        andTraingData.Add(t2);
        andTraingData.Add(t3);
        andTraingData.Add(t4);
    }
    public static Perceptron CreatePerceptron(double learningRate, double initialWeightLimit, int inputsCount)
    {
        Random rng = new Random();
        double[] initialWeights = new double[inputsCount];
        for (int i = 0; i < initialWeights.Length; i++)
        {
            initialWeights[i] = initialWeightLimit * (rng.NextDouble() * 2 - 1);
        }
        double bias = initialWeightLimit * (rng.NextDouble() * 2 - 1);

        Perceptron perceptron = new Perceptron(initialWeights, learningRate, bias);

        return perceptron;
    }

    public static int TrainPerceptron_And(Perceptron perceptron)
    {
        bool isTrained = false;
        int epoch = 0;
        while (!isTrained)
        {
            WriteResponseLine($"Learning epoch - {++epoch}");
            isTrained = true;
            foreach (var trainObject in andTraingData)
            {
                var error = perceptron.Train(trainObject.Input, trainObject.Solution);
                if (error != 0)
                {
                    isTrained = false;
                }
            }
            if (epoch > 10000)
            {
                WriteError("Stopping!");
                WriteError("Did not learn anything in 10000 epochs!");
                throw new Exception();
            }
        }
        return epoch;
    }

    public static bool Check_And(Perceptron perceptron)
    {
        bool isTrained = true;
        Console.WriteLine($"x1 | x2 | y | decision");
        foreach (var to in andTraingData)
        {
            var decision = perceptron.Feedforward(to.Input);
            WriteResponse(to.Input[0].ToString().PadRight(3) + "|");
            WriteResponse(" " + to.Input[1].ToString().PadRight(3) + "|");
            WriteResponse(" " + to.Solution.ToString().PadRight(2) + "|");
            WriteResponse(" " + decision);
            WriteResponse(decision == to.Solution ? "" : " [x]");
            Console.WriteLine();
            if (isTrained) isTrained = false;
        }
        return isTrained;
    }
}