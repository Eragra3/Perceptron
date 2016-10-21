
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace Perceptron
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var options = new Options();
            var parser = Parser.Default;

            string invokedCommand = "";
            object invokedSubOptions = null;

            if (!parser.ParseArgumentsStrict(args, options, (command, subOptions) =>
            {
                invokedCommand = command;
                invokedSubOptions = subOptions;
            }))
            {
                Environment.Exit(Parser.DefaultExitCodeFail);
            }

            var verbose = ((CommonSubOptions)invokedSubOptions).Verbose;
            if (verbose)
            {
                Console.WriteLine(Options.GetHeading().ToString());
                Console.WriteLine();
            }

            switch (invokedCommand)
            {
                case "train":
                    var trainSubOptions = (TrainSubOptions)invokedSubOptions;

                    if (!trainSubOptions.Validate()) Environment.Exit(Parser.DefaultExitCodeFail);

                    var learningRate = trainSubOptions.LearningRate;
                    var stepFunction = trainSubOptions.StepFunction;
                    var weightsLimit = trainSubOptions.InitialWeights;
                    var useAdaline = trainSubOptions.UseAdaline;

                    var perceptron = PerceptronTrainer.CreatePerceptron(
                        learningRate,
                        weightsLimit,
                        2,
                        stepFunction,
                        useAdaline
                        );

                    if (useAdaline)
                    {
                        var adalineThreshold = trainSubOptions.AdalineThreshold;
                        PerceptronTrainer.TrainPerceptron_And(perceptron, adalineThreshold, verbose);
                    }
                    else
                    {
                        PerceptronTrainer.TrainPerceptron_And(perceptron);
                    }

                    if (!PerceptronTrainer.Test_And(perceptron, verbose))
                    {
                        Console.Error.WriteLine("Test data failed!");
                    }

                    if (trainSubOptions.Octave)
                    {
                        ConsoleHelper.WriteYellowLine("Octave plot code:");
                        Console.WriteLine(perceptron.GenerateOctaveCode());
                        Console.WriteLine();
                    }

                    Console.Write(perceptron.ToJson());

                    break;
                case "experiment":
                    var experimentSubOptions = (ExperimentSubOptions)invokedSubOptions;

                    if (!experimentSubOptions.Validate()) Environment.Exit(Parser.DefaultExitCodeFail);

                    switch (experimentSubOptions.Type)
                    {
                        case ExperimentRunner.ExperimentType.LearningRate:
                            ExperimentRunner.LearningRate(
                                experimentSubOptions.From,
                                experimentSubOptions.To,
                                experimentSubOptions.Step,
                                experimentSubOptions.Repetitions,
                                experimentSubOptions.InitialWeights,
                                experimentSubOptions.StepFunction,
                                2,
                                experimentSubOptions.UseAdaline,
                                experimentSubOptions.AdalineThreshold,
                                experimentSubOptions.Verbose
                                );
                            break;
                        case ExperimentRunner.ExperimentType.InitialWeights:
                            ExperimentRunner.WeightsRange(
                                experimentSubOptions.From,
                                experimentSubOptions.To,
                                experimentSubOptions.Step,
                                experimentSubOptions.Repetitions,
                                experimentSubOptions.LearningRate,
                                experimentSubOptions.StepFunction,
                                2,
                                experimentSubOptions.UseAdaline,
                                experimentSubOptions.AdalineThreshold,
                                experimentSubOptions.Verbose
                                );
                            break;
                        case ExperimentRunner.ExperimentType.AdalineThreshold:
                            ExperimentRunner.AdalineTreshold(
                                experimentSubOptions.From,
                                experimentSubOptions.To,
                                experimentSubOptions.Step,
                                experimentSubOptions.Repetitions,
                                experimentSubOptions.LearningRate,
                                experimentSubOptions.InitialWeights,
                                2,
                                experimentSubOptions.Verbose
                                );
                            break;
                        case ExperimentRunner.ExperimentType.AdalineError:
                            ExperimentRunner.AdalineError(
                                experimentSubOptions.LearningRate,
                                experimentSubOptions.InitialWeights,
                                experimentSubOptions.AdalineThreshold,
                                2
                                );
                            break;
                        case ExperimentRunner.ExperimentType.Error:
                            ExperimentRunner.Error(
                                experimentSubOptions.LearningRate,
                                experimentSubOptions.InitialWeights,
                                experimentSubOptions.StepFunction,
                                2
                                );
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                case "test":
                    var testSubOptions = (TestSubOptions)invokedSubOptions;

                    if (!testSubOptions.Validate()) Environment.Exit(Parser.DefaultExitCodeFail);

                    var json = File.ReadAllText(testSubOptions.PerceptronJsonPath);

                    var p = PerceptronTrainer.CreatePerceptron(json);

                    if (p == null)
                    {
                        ConsoleHelper.WriteErrorLine("Perceptron json is invalid");
                        Environment.Exit(Parser.DefaultExitCodeFail);
                    }

                    if (testSubOptions.TestAnd) PerceptronTrainer.Test_And(p, true);

                    if (!double.IsNaN(testSubOptions.X1) && !double.IsNaN(testSubOptions.X2))
                    {
                        var input = new[] { testSubOptions.X1, testSubOptions.X2 };
                        Console.WriteLine(p.Feedforward(input));
                    }

                    if (testSubOptions.Octave) Console.WriteLine(p.GenerateOctaveCode());

                    break;
                default:
                    Environment.Exit(Parser.DefaultExitCodeFail);
                    break;
            }

            Environment.Exit(0);
        }
    }
}