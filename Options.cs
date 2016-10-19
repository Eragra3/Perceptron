﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using static Perceptron.StepFunction;

namespace Perceptron
{
    public class CommonSubOptions
    {
        [ParserState]
        public IParserState LastParserState { get; set; }

        [Option('v', "verbose", HelpText = "Explain what is happening")]
        public bool Verbose { get; set; } = false;
    }

    public class TrainSubOptions : CommonSubOptions
    {
        [Option('l', "learning-rate", HelpText = "Perceptron learning rate")]
        public double LearningRate { get; set; } = 0.2;

        [Option('w', "weights", HelpText = "Initial weights range = (-number;number) (exclusive)")]
        public double InitialWeights { get; set; } = 1;

        [Option('s', "step-function", HelpText = "Step function")]
        public StepFunction StepFunction { get; set; } = Unipolar;

        [Option('a', "adaline", HelpText = "Use adaline")]
        public bool UseAdaline { get; set; } = false;

        [Option('t', "adaline-threshold", HelpText = "Adaline error treshold")]
        public double AdalineThreshold { get; set; } = 1.0;

        [Option('o', "octave", HelpText = "Generate octave code to plot approximated function")]
        public bool Octave { get; set; } = false;

        public bool Validate()
        {
            if (LearningRate <= 0)
            {
                Console.Error.WriteLine("Learning rate cannot be lower than 0");
                return false;
            }

            return true;
        }
    }

    public class ExperimentSubOptions : CommonSubOptions
    {
        //[Option("from", HelpText = "Initial value", Required = true)]
        //public double From { get; set; }

        //[Option("to", HelpText = "Final value", Required = true)]
        //public double To { get; set; }

        //[Option("step", HelpText = "Step", Required = true)]
        //public double Step { get; set; }

        [ValueOption(0)]
        public double From { get; set; }

        [ValueOption(1)]
        public double Step { get; set; }

        [ValueOption(2)]
        public double To { get; set; }

        [ValueOption(3)]
        public int Repetitions { get; set; } = 1000;

        [Option('e', "type", HelpText = "Experiment type (LearningRate, InitialWeights, AdalineThreshold)", Required = true)]
        public ExperimentRunner.ExperimentType Type { get; set; }

        [Option('l', "learning-rate", HelpText = "Perceptron learning rate (must be greater than 0)")]
        public double LearningRate { get; set; } = 0.2;

        [Option('w', "weights", HelpText = "Initial weights range = (-number;number) (exclusive)")]
        public double InitialWeights { get; set; } = 1;

        [Option('s', "step-function", HelpText = "Step function")]
        public StepFunction StepFunction { get; set; } = Unipolar;

        [Option('a', "adaline", HelpText = "Use adaline")]
        public bool UseAdaline { get; set; } = false;

        [Option('t', "adaline-threshold", HelpText = "Adaline error treshold")]
        public double AdalineThreshold { get; set; } = 1.0;
    }

    public class Options
    {
        [ParserState]
        public IParserState LastParserState { get; set; }

        [VerbOption("train", HelpText = "Create and train perceptron")]
        public TrainSubOptions TrainVerb { get; set; }

        [VerbOption("experiment", HelpText = "Run experiment")]
        public ExperimentSubOptions Command { get; set; }

        [HelpVerbOption]
        public string GetUsage(string verb)
        {
            var help = HelpText.AutoBuild(this, verb);

            if (this.LastParserState?.Errors.Any() == true)
            {
                var errors = help.RenderParsingErrorsText(this, 2); // indent with two spaces

                if (!string.IsNullOrEmpty(errors))
                {
                    help.AddPreOptionsLine(string.Concat(Environment.NewLine, "ERROR(S):"));
                    help.AddPreOptionsLine(errors);
                }
            }

            return help;
        }

        public static HeadingInfo GetHeading()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            return new HeadingInfo(assembly.FullName, $"{version.Major}.{version.Minor}");
        }
    }
}
