using System;
using System.Collections.Generic;
using static ConsoleHelper;

public static class PerceptronTrainer
{
    private static Random _rng = new Random();

    public static IList<TrainObject> andTraingData;
    static PerceptronTrainer()
    {
        Func<double, double, double> andFunction = (x, y) => x + y - 1;
        andTraingData = new List<TrainObject>(10);
        TrainObject t1 = new TrainObject(new double[] { 0, 0 }, 0, andFunction(0, 0));
        TrainObject t2 = new TrainObject(new double[] { 0, 1 }, 0, andFunction(0, 1));
        TrainObject t3 = new TrainObject(new double[] { 1, 0 }, 0, andFunction(1, 0));
        TrainObject t4 = new TrainObject(new double[] { 1, 1 }, 1, andFunction(1, 1));
        andTraingData.Add(t1);
        andTraingData.Add(t2);
        andTraingData.Add(t3);
        andTraingData.Add(t4);
    }

    public static Perceptron CreatePerceptron(
        double learningRate,
        double initialWeightLimit,
        int inputsCount,
        StepFunctionEnum stepFunction,
        bool useAdaline)
    {
        if (learningRate <= 0) throw new ArgumentException($"{nameof(learningRate)} cannot be 0! Nor negative. Faggot");

        double[] initialWeights = new double[inputsCount];
        for (int i = 0; i < initialWeights.Length; i++)
        {
            initialWeights[i] = initialWeightLimit * (_rng.NextDouble() * 2 - 1);
        }
        double bias = initialWeightLimit * (_rng.NextDouble() * 2 - 1);

        Perceptron perceptron = new Perceptron(initialWeights, learningRate, bias, stepFunction, useAdaline);

        return perceptron;
    }
    public static Perceptron CreatePerceptron(
        double learningRate,
        double initialWeightLimit,
        int inputsCount,
        StepFunctionEnum stepFunction)
    {
        return CreatePerceptron(learningRate, initialWeightLimit, inputsCount, stepFunction, false);
    }

    public static Perceptron CreatePerceptron(double learningRate, double initialWeightLimit, int inputsCount)
    {
        return CreatePerceptron(learningRate, initialWeightLimit, inputsCount, StepFunctionEnum.Unipolar);
    }

    public static int TrainPerceptron_And(Perceptron perceptron, double adalineThreshold = 1, bool silent = false)
    {
        bool isTrained = false;
        int epoch = 0;
        if (perceptron.IsAdaline)
        {
            if (!silent) WriteResponseLine($"Using adaline, error treshold - {adalineThreshold}");
            double errorSum;
            do
            {
                epoch++;
                errorSum = 0;
                if (!silent) WriteResponse($"Learning epoch - {epoch}");
                foreach (var trainObject in andTraingData)
                {
                    errorSum += Math.Abs(perceptron.Train(trainObject));
                }

                if (!silent) WriteResponseLine($" current error - {errorSum}");

                if (epoch > 10000)
                {
                    WriteErrorLine("Stopping!");
                    WriteErrorLine("Did not learn nothing in 10000 epochs!");
                    WriteErrorLine("Using adaline, current values: error-{error} > threshold-{adalineThreshold}");
                    throw new Exception();
                }
            }
            while (errorSum > adalineThreshold);
        }
        else
        {
            while (!isTrained)
            {
                epoch++;
                if (!silent) WriteResponseLine($"Learning epoch - {epoch}");
                isTrained = true;
                foreach (var trainObject in andTraingData)
                {
                    var error = perceptron.Train(trainObject);
                    if (error != 0)
                    {
                        isTrained = false;
                    }
                }
                if (epoch > 10000)
                {
                    WriteErrorLine("Stopping!");
                    WriteErrorLine("Did not learn nothing in 10000 epochs!");
                    throw new Exception();
                }
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
            var solution = to.Solution;
            if (perceptron.IsBipolar && solution == 0) solution = -1;
            var decision = perceptron.Feedforward(to.Input);
            if (!silent)
            {
                WriteResponse(to.Input[0].ToString().PadRight(3) + "|");
                WriteResponse(" " + to.Input[1].ToString().PadRight(3) + "|");
                WriteResponse(" " + solution.ToString().PadRight(2) + "|");
                WriteResponse(" " + decision);
                WriteResponse(decision == solution ? "" : " [x]");
                Console.WriteLine();
            }
            if (isTrained) isTrained = decision == solution;
        }
        return isTrained;
    }
}