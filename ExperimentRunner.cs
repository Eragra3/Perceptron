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
            CsvPrinter.DumpHeaderLine("n", "learning rate", "min epochs", "max epochs", "avg epochs");

            int experimentIndex = 0;
            for (double learningRate = start; learningRate <= end; learningRate += step)
            {
                experimentIndex++;

                var minEpochs = int.MaxValue;
                var epochsSum = 0;
                var maxEpochs = 0;

                var run = 0;
                Perceptron p = null;
                if (verbose) ConsoleHelper.WriteYellowLine($"Experiment - {experimentIndex}");

                for (int j = 0; j < repetitions; j++)
                {
                    run++;
                    p = PerceptronTrainer.CreatePerceptron(learningRate, initialWeightsLimit, inputsCount, stepFunction, useAdaline);
                    var epochs = PerceptronTrainer.TrainPerceptron_And(p, adalineThreshold, verbose);

                    if (epochs < minEpochs) minEpochs = epochs;
                    if (epochs > maxEpochs) maxEpochs = epochs;

                    epochsSum += epochs;
                }

                var avarageEpochs = epochsSum / run;
                CsvPrinter.DumpLine(experimentIndex, learningRate, minEpochs, maxEpochs, avarageEpochs);
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
            CsvPrinter.DumpHeaderLine("n", "initial weights limit", "min epochs", "max epochs", "avg epochs");

            int experimentIndex = 0;
            for (double weightsLimit = start; weightsLimit <= end; weightsLimit += step)
            {
                experimentIndex++;

                var minEpochs = int.MaxValue;
                var epochsSum = 0;
                var maxEpochs = 0;

                var run = 0;
                Perceptron p = null;
                if (verbose) ConsoleHelper.WriteYellowLine($"Experiment - {experimentIndex}");

                for (int j = 0; j < repetitions; j++)
                {
                    run++;
                    p = PerceptronTrainer.CreatePerceptron(learningRate, weightsLimit, inputsCount, stepFunction, useAdaline);
                    var epochs = PerceptronTrainer.TrainPerceptron_And(p, adalineThreshold, verbose);

                    if (epochs < minEpochs) minEpochs = epochs;
                    if (epochs > maxEpochs) maxEpochs = epochs;

                    epochsSum += epochs;
                }

                var avarageEpochs = epochsSum / run;
                CsvPrinter.DumpLine(experimentIndex, weightsLimit, minEpochs, maxEpochs, avarageEpochs);
            }

        }

        public static void AdalineTreshold(
            double start,
            double end,
            double step,
            int repetitions,
            double learningRate,
            double weightsLimit,
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
                new KeyValuePair<string, object>("inputsCount", inputsCount),
                new KeyValuePair<string, object>("verbose", verbose)
                );
            CsvPrinter.DumpHeaderLine("n", "adaline threshold", "min epochs", "max epochs", "avg epochs", "final error");

            int experimentIndex = 0;
            for (double adalineThreshold = start; adalineThreshold <= end; adalineThreshold += step)
            {
                experimentIndex++;

                var minEpochs = int.MaxValue;
                var epochsSum = 0;
                var maxEpochs = 0;

                var run = 0;
                Perceptron p = null;
                if (verbose) ConsoleHelper.WriteYellowLine($"Experiment - {experimentIndex}");
                try
                {
                    for (int j = 0; j < repetitions; j++)
                    {
                        run++;
                        p = PerceptronTrainer.CreatePerceptron(learningRate, weightsLimit, inputsCount, StepFunction.Bipolar, true);
                        var epochs = PerceptronTrainer.TrainPerceptron_And(p, adalineThreshold, verbose);

                        if (epochs < minEpochs) minEpochs = epochs;
                        if (epochs > maxEpochs) maxEpochs = epochs;

                        epochsSum += epochs;
                    }
                }
                catch (PerceptronLearnException)
                {
                    if (verbose) ConsoleHelper.WriteErrorLine("Perceptron can't learn with params:");
                    p?.Dump();
                }
                var avarageEpochs = epochsSum / run;
                var currentError = PerceptronTrainer.GetAdalineError(p);
                CsvPrinter.DumpLine(experimentIndex, adalineThreshold, minEpochs, maxEpochs, avarageEpochs, currentError);
            }

        }

        public static void AdalineError(
            double learningRate,
            double weightsLimit,
            double adalineThreshold,
            int inputsCount)
        {

            CsvPrinter.DumpParams(
                new KeyValuePair<string, object>("weightsLimit", weightsLimit),
                new KeyValuePair<string, object>("learningRate", learningRate),
                new KeyValuePair<string, object>("inputsCount", inputsCount)
                );
            CsvPrinter.DumpHeaderLine("n", "error");

            try
            {
                var p = PerceptronTrainer.CreatePerceptron(learningRate, weightsLimit, inputsCount, StepFunction.Bipolar, true);
                var epochs = PerceptronTrainer.TrainPerceptron_And(p, adalineThreshold, dumpData: true);
            }
            catch (PerceptronLearnException)
            {
            }
        }

        public static void Error(
            double learningRate,
            double weightsLimit,
            StepFunction stepFunction,
            int inputsCount)
        {

            CsvPrinter.DumpParams(
                new KeyValuePair<string, object>("weightsLimit", weightsLimit),
                new KeyValuePair<string, object>("learningRate", learningRate),
                new KeyValuePair<string, object>("stepFunction", stepFunction),
                new KeyValuePair<string, object>("inputsCount", inputsCount)
                );
            CsvPrinter.DumpHeaderLine("n", "error");

            try
            {
                var p = PerceptronTrainer.CreatePerceptron(learningRate, weightsLimit, inputsCount, stepFunction, false);
                var epochs = PerceptronTrainer.TrainPerceptron_And(p, dumpData: true);
            }
            catch (PerceptronLearnException)
            {
            }
        }

        public class PerceptronLearnException : Exception
        {

        }

        public enum ExperimentType
        {
            LearningRate,
            InitialWeights,
            AdalineThreshold,
            AdalineError,
            Error
        }
    }
}