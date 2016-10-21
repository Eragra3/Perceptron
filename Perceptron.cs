using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Perceptron
{
    public class Perceptron
    {
        [JsonProperty]
        private double[] _weights;
        [JsonProperty]
        private double _learningRate;
        [JsonProperty]
        private double _bias;
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        private StepFunction _stepFunction;
        [JsonProperty]
        private readonly bool _isAdaline;

        [JsonIgnore]
        public bool IsAdaline => _isAdaline;
        [JsonIgnore]
        public bool IsBipolar => _stepFunction == StepFunction.Bipolar;

        public Perceptron(
            double[] weights,
            double learningRate,
            double bias,
            StepFunction stepFunction,
            bool isAdaline
            )
        {
            _weights = weights;
            _learningRate = learningRate;
            _bias = bias;
            _stepFunction = stepFunction;
            _isAdaline = isAdaline;
        }

        public double Net(double[] input)
        {
            double sum = 0;
            for (int i = 0; i < input.Length; i++)
            {
                sum += input[i] * _weights[i];
            }
            sum += _bias;
            return sum;
        }

        public int Step(double sum)
        {
            if (_stepFunction == StepFunction.Bipolar) return sum >= 0 ? 1 : -1;
            return sum >= 0 ? 1 : 0;
        }

        public double Train(TrainObject to)
        {
            return Train(to.Input, to.Solution);
        }
        public double Train(double[] input, int solution)
        {
            if (IsBipolar)
            {
                if (solution == 0) solution = -1;
            }


            double net = Net(input);
            var decision = Step(net);

            double error;
            if (IsAdaline)
            {
                error = solution - net;
            }
            else
            {
                error = solution - decision;
            }

            for (int i = 0; i < _weights.Length; i++)
            {
                if (IsAdaline)
                {
                    _weights[i] += _learningRate * input[i] * error;
                }
                else
                {
                    _weights[i] += _learningRate * input[i] * error;
                }
            }
            _bias += _learningRate * error;

            return error;
        }

        public int Feedforward(params double[] xs)
        {
            var net = Net(xs);
            return Step(net);
        }

        public string Dump()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Perceptron data:");
            sb.AppendLine($"{"learning rate".PadRight(15)} : {_learningRate}");
            sb.AppendLine($"{"step function".PadRight(15)} : {_stepFunction}");
            sb.AppendLine($"{"adaline".PadRight(15)} : {IsAdaline}");
            sb.AppendLine($"{"bias".PadRight(15)} : {_bias}");
            var weights = _weights.Aggregate("", (acc, w) => acc + (Math.Round(w, 4).ToString(CultureInfo.InvariantCulture) + ", "));
            sb.AppendLine($"{"weights".PadRight(15)} : {weights}");

            return sb.ToString();
        }

        public string GenerateOctaveCode()
        {
            StringBuilder sb = new StringBuilder("f = @(");
            for (var i = 0; i < _weights.Length; i++)
            {
                sb.Append("x" + i);
                if (i + i < _weights.Length) sb.Append(",");
            }
            sb.Append(") ");
            for (var i = 0; i < _weights.Length; i++)
            {
                var weight = _weights[i];
                sb.Append(Math.Round(weight, 4).ToString(CultureInfo.InvariantCulture).Replace(',', '.'));
                sb.Append("*x" + i);
                if (i + i < _weights.Length) sb.Append("+");
            }
            sb.Append("+");
            sb.Append(Math.Round(_bias, 4).ToString(CultureInfo.InvariantCulture).Replace(',', '.'));
            sb.Append(";ezplot(f,[0,1]);");
            sb.Append("grid on;");
            return sb.ToString();
        }

        public string ToJson()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            return json;
        }

        public static Perceptron FromJson(string json)
        {
            Perceptron perceptron;
            try
            {
                perceptron = JsonConvert.DeserializeObject<Perceptron>(json);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
            return perceptron;
        }
    }

    public enum StepFunction
    {
        Unipolar,
        Bipolar
    }
}