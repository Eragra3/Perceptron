using System;
using System.Collections.Generic;
using static ConsoleHelper;

public static class PerceptronTrainer
{
    private static Random _rng = new Random();

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
        if (learningRate <= 0) throw new ArgumentException($"{nameof(learningRate)} cannot be 0! Nor negative. Faggot");

        double[] initialWeights = new double[inputsCount];
        for (int i = 0; i < initialWeights.Length; i++)
        {
            initialWeights[i] = initialWeightLimit * (_rng.NextDouble() * 2 - 1);
        }
        double bias = initialWeightLimit * (_rng.NextDouble() * 2 - 1);

        Perceptron perceptron = new Perceptron(initialWeights, learningRate, bias);

        return perceptron;
    }

    public static int TrainPerceptron_And(Perceptron perceptron, bool silent = false)
    {
        bool isTrained = false;
        int epoch = 0;
        while (!isTrained)
        {
            epoch++;
            if (!silent) WriteResponseLine($"Learning epoch - {epoch}");
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
                WriteErrorLine("Stopping!");
                WriteErrorLine("Did not learn anything in 10000 epochs!");
                throw new Exception();
            }
        }
        return epoch;
    }

    public static bool Test_And(Perceptron perceptron, bool silent = false)
    {
        bool isTrained = true;
        if (!silent) Console.WriteLine($"x1 | x2 | y | decision");
        foreach (var to in andTraingData)
        {
            var decision = perceptron.Feedforward(to.Input);
            if (!silent)
            {
                WriteResponse(to.Input[0].ToString().PadRight(3) + "|");
                WriteResponse(" " + to.Input[1].ToString().PadRight(3) + "|");
                WriteResponse(" " + to.Solution.ToString().PadRight(2) + "|");
                WriteResponse(" " + decision);
                WriteResponse(decision == to.Solution ? "" : " [x]");
                Console.WriteLine();
            }
            if (isTrained) isTrained = decision == to.Solution;
        }
        return isTrained;
    }
}