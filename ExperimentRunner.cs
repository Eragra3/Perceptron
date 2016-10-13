using System;
using static ConsoleHelper;

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
         int inputsCount = 2)
    {
        if (start > end || step > Math.Abs(end - start)) throw new ArgumentException();

        int experimentIndex = 0;
        int run = 0;
        int epochsSum = 0;
        for (double learningRate = start; learningRate <= end; learningRate += step)
        {
            experimentIndex++;
            epochsSum = 0;
            run = 0;
            Perceptron p = null;
            WriteResponseLine($"Experiment - {experimentIndex}");
            try
            {
                for (int j = 0; j < repetitions; j++)
                {
                    run++;
                    p = PerceptronTrainer.CreatePerceptron(learningRate, initialWeightsLimit, inputsCount);
                    var epochs = PerceptronTrainer.TrainPerceptron_And(p, silent: true);

                    if (!PerceptronTrainer.Test_And(p, silent: true)) throw new PerceptronLearnException();
                    p?.Dump();

                    epochsSum += epochs;
                    if (!Silent) WriteExperimentLine($"epochs - {epochs.ToString().PadRight(2)}");
                }
            }
            catch (PerceptronLearnException)
            {
                WriteErrorLine("Perceptron can't learn with params:");
                p?.Dump();
            }
            var avarageEpochs = epochsSum / run;
            WriteExperimentLine($"\tavarage epochs - {avarageEpochs}");
            WriteLine();
        }

    }

    public class PerceptronLearnException : Exception
    {

    }
}