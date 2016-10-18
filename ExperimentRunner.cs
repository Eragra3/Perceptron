using System;

namespace Perceptron
{
    public static class ExperimentRunner
    {
        public static bool Silent = false;
        ///<summary>
        ///Both start and end are inclusive
        ///</summary>
        public static void LearningRate(
            double start,
            double end,
            double step,
            int repetitions,
            double initialWeightsLimit,
            StepFunctionEnum stepFunction,
            bool useAdaline = false,
            double adalineTreshold = 1,
            int inputsCount = 2)
        {
            if (start > end || step > Math.Abs(end - start) || step == 0) throw new ArgumentException();

            int experimentIndex = 0;
            int run = 0;
            int epochsSum = 0;
            for (double learningRate = start; learningRate <= end; learningRate += step)
            {
                experimentIndex++;
                epochsSum = 0;
                run = 0;
                Perceptron p = null;
                ConsoleHelper.WriteResponseLine($"Experiment - {experimentIndex}");
                try
                {
                    for (int j = 0; j < repetitions; j++)
                    {
                        run++;
                        p = PerceptronTrainer.CreatePerceptron(learningRate, initialWeightsLimit, inputsCount, stepFunction, useAdaline);
                        var epochs = PerceptronTrainer.TrainPerceptron_And(p, adalineTreshold, true);

                        if (!useAdaline)
                        {
                            if (!PerceptronTrainer.Test_And(p, true)) throw new PerceptronLearnException();
                        }

                        epochsSum += epochs;
                    }
                }
                catch (PerceptronLearnException)
                {
                    ConsoleHelper.WriteErrorLine("Perceptron can't Test_And params:");
                    p?.Dump();
                }
                var avarageEpochs = epochsSum / run;
                ConsoleHelper.WriteExperimentLine($"\tavarage epochs - {avarageEpochs}");
                ConsoleHelper.WriteLine();
            }
        }

        public static void WeightsRange(
            double start,
            double end,
            double step,
            int repetitions,
            double learningRate,
            StepFunctionEnum stepFunction,
            bool useAdaline = false,
            double adalineTreshold = 1,
            int inputsCount = 2)
        {
            if (start > end || step > Math.Abs(end - start) || step == 0) throw new ArgumentException();

            int experimentIndex = 0;
            int run = 0;
            int epochsSum = 0;
            for (double weightsLimit = start; weightsLimit <= end; weightsLimit += step)
            {
                experimentIndex++;
                epochsSum = 0;
                run = 0;
                Perceptron p = null;
                ConsoleHelper.WriteResponseLine($"Experiment - {experimentIndex}");
                try
                {
                    for (int j = 0; j < repetitions; j++)
                    {
                        run++;
                        p = PerceptronTrainer.CreatePerceptron(learningRate, weightsLimit, inputsCount, stepFunction, useAdaline);
                        var epochs = PerceptronTrainer.TrainPerceptron_And(p, adalineTreshold, true);

                        if (!useAdaline)
                        {
                            if (!PerceptronTrainer.Test_And(p, true)) throw new PerceptronLearnException();
                        }

                        epochsSum += epochs;
                    }
                }
                catch (PerceptronLearnException)
                {
                    ConsoleHelper.WriteErrorLine("Perceptron can't learn with params:");
                    p?.Dump();
                }
                var avarageEpochs = epochsSum / run;
                ConsoleHelper.WriteExperimentLine($"\tavarage epochs - {avarageEpochs}");
                ConsoleHelper.WriteLine();
            }

        }

        public static void AdalineTreshold(
            double start,
            double end,
            double step,
            int repetitions,
            double learningRate,
            double weightsLimit,
            StepFunctionEnum stepFunction,
            int inputsCount = 2)
        {
            if (start > end || step > Math.Abs(end - start) || step == 0) throw new ArgumentException();

            int experimentIndex = 0;
            int run = 0;
            int epochsSum = 0;
            for (double adalineTreshold = start; adalineTreshold <= end; adalineTreshold += step)
            {
                experimentIndex++;
                epochsSum = 0;
                run = 0;
                Perceptron p = null;
                ConsoleHelper.WriteResponseLine($"Experiment - {experimentIndex}");
                try
                {
                    for (int j = 0; j < repetitions; j++)
                    {
                        run++;
                        p = PerceptronTrainer.CreatePerceptron(learningRate, weightsLimit, inputsCount, stepFunction, true);
                        var epochs = PerceptronTrainer.TrainPerceptron_And(p, adalineTreshold, true);

                        epochsSum += epochs;
                    }
                }
                catch (PerceptronLearnException)
                {
                    ConsoleHelper.WriteErrorLine("Perceptron can't learn with params:");
                    p?.Dump();
                }
                var avarageEpochs = epochsSum / run;
                ConsoleHelper.WriteExperimentLine($"\tavarage epochs - {avarageEpochs}");
                ConsoleHelper.WriteLine();
            }

        }

        public class PerceptronLearnException : Exception
        {

        }
    }
}