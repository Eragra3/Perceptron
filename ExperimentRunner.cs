using System;
using System.Collections.Generic;
using System.Reflection;

namespace Perceptron
{
    public static class ExperimentRunner
    {
        ///<summary>
        ///Both start and end are inclusive
        ///</summary>
        public static void LearningRate(
            double start,
            double end,
            double step,
            int repetitions,
            double initialWeightsLimit,
            StepFunction stepFunction,
            int inputsCount,
            bool useAdaline = false,
            double adalineThreshold = 1,
            bool verbose = false
            )
        {
            if (start > end || step > Math.Abs(end - start) || step == 0) throw new ArgumentException();

            CsvPrinter.DumpParams(
                new KeyValuePair<string, object>("start", start),
                new KeyValuePair<string, object>("end", end),
                new KeyValuePair<string, object>("step", step),
                new KeyValuePair<string, object>("repetitions", repetitions),
                new KeyValuePair<string, object>("initialWeightsLimit", initialWeightsLimit),
                new KeyValuePair<string, object>("stepFunction", stepFunction),
                new KeyValuePair<string, object>("inputsCount", inputsCount),
                new KeyValuePair<string, object>("useAdalvine", useAdaline),
                new KeyValuePair<string, object>("adalineThreshold", adalineThreshold),
                new KeyValuePair<string, object>("verbose", verbose)
                );
            CsvPrinter.DumpHeaderLine("n", "avg epochs");

            int experimentIndex = 0;
            for (double learningRate = start; learningRate <= end; learningRate += step)
            {
                experimentIndex++;
                var epochsSum = 0;
                var run = 0;
                Perceptron p = null;
                if (verbose) ConsoleHelper.WriteYellowLine($"Experiment - {experimentIndex}");
                try
                {
                    for (int j = 0; j < repetitions; j++)
                    {
                        run++;
                        p = PerceptronTrainer.CreatePerceptron(learningRate, initialWeightsLimit, inputsCount, stepFunction, useAdaline);
                        var epochs = PerceptronTrainer.TrainPerceptron_And(p, adalineThreshold, verbose);

                        epochsSum += epochs;
                    }
                }
                catch (PerceptronLearnException)
                {
                    ConsoleHelper.WriteErrorLine("Perceptron can't Test_And params:");
                    p?.Dump();
                }
                var avarageEpochs = epochsSum / run;
                CsvPrinter.DumpLine(experimentIndex, avarageEpochs);
            }
        }

        public static void WeightsRange(
            double start,
            double end,
            double step,
            int repetitions,
            double learningRate,
            StepFunction stepFunction,
            int inputsCount,
            bool useAdaline = false,
            double adalineThreshold = 1,
            bool verbose = false
            )
        {
            if (start > end || step > Math.Abs(end - start) || step == 0) throw new ArgumentException();

            CsvPrinter.DumpParams(
                new KeyValuePair<string, object>("start", start),
                new KeyValuePair<string, object>("end", end),
                new KeyValuePair<string, object>("step", step),
                new KeyValuePair<string, object>("repetitions", repetitions),
                new KeyValuePair<string, object>("learningRate", learningRate),
                new KeyValuePair<string, object>("stepFunction", stepFunction),
                new KeyValuePair<string, object>("inputsCount", inputsCount),
                new KeyValuePair<string, object>("useAdalvine", useAdaline),
                new KeyValuePair<string, object>("adalineThreshold", adalineThreshold),
                new KeyValuePair<string, object>("verbose", verbose)
                );
            CsvPrinter.DumpHeaderLine("n", "avg epochs");

            int experimentIndex = 0;
            for (double weightsLimit = start; weightsLimit <= end; weightsLimit += step)
            {
                experimentIndex++;
                var epochsSum = 0;
                var run = 0;
                Perceptron p = null;
                if (verbose) ConsoleHelper.WriteYellowLine($"Experiment - {experimentIndex}");
                try
                {
                    for (int j = 0; j < repetitions; j++)
                    {
                        run++;
                        p = PerceptronTrainer.CreatePerceptron(learningRate, weightsLimit, inputsCount, stepFunction, useAdaline);
                        var epochs = PerceptronTrainer.TrainPerceptron_And(p, adalineThreshold, verbose);

                        epochsSum += epochs;
                    }
                }
                catch (PerceptronLearnException)
                {
                    ConsoleHelper.WriteErrorLine("Perceptron can't learn with params:");
                    p?.Dump();
                }
                var avarageEpochs = epochsSum / run;
                CsvPrinter.DumpLine(experimentIndex, avarageEpochs);
            }

        }

        public static void AdalineTreshold(
            double start,
            double end,
            double step,
            int repetitions,
            double learningRate,
            double weightsLimit,
            StepFunction stepFunction,
            int inputsCount,
            bool verbose = false)
        {
            if (start > end || step > Math.Abs(end - start) || step == 0) throw new ArgumentException();

            CsvPrinter.DumpParams(
                new KeyValuePair<string, object>("start", start),
                new KeyValuePair<string, object>("end", end),
                new KeyValuePair<string, object>("step", step),
                new KeyValuePair<string, object>("repetitions", repetitions),
                new KeyValuePair<string, object>("weightsLimit", weightsLimit),
                new KeyValuePair<string, object>("learningRate", learningRate),
                new KeyValuePair<string, object>("stepFunction", stepFunction),
                new KeyValuePair<string, object>("inputsCount", inputsCount),
                new KeyValuePair<string, object>("verbose", verbose)
                );
            CsvPrinter.DumpHeaderLine("n", "avg epochs", "final error");

            int experimentIndex = 0;
            for (double adalineTreshold = start; adalineTreshold <= end; adalineTreshold += step)
            {
                experimentIndex++;
                var epochsSum = 0;
                var run = 0;
                Perceptron p = null;
                if (verbose) ConsoleHelper.WriteYellowLine($"Experiment - {experimentIndex}");
                try
                {
                    for (int j = 0; j < repetitions; j++)
                    {
                        run++;
                        p = PerceptronTrainer.CreatePerceptron(learningRate, weightsLimit, inputsCount, stepFunction, true);
                        var epochs = PerceptronTrainer.TrainPerceptron_And(p, adalineTreshold, verbose);

                        epochsSum += epochs;
                    }
                }
                catch (PerceptronLearnException)
                {
                    ConsoleHelper.WriteErrorLine("Perceptron can't learn with params:");
                    p?.Dump();
                }
                var avarageEpochs = epochsSum / run;
                var currentError = PerceptronTrainer.GetAdalineError(p);
                CsvPrinter.DumpLine(experimentIndex, avarageEpochs, currentError);
            }

        }

        public class PerceptronLearnException : Exception
        {

        }

        public enum ExperimentType
        {
            LearningRate,
            InitialWeights,
            AdalineThreshold
        }
    }
}