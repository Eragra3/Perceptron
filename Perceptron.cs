using System;
using System.Linq;
using System.Text;

public class Perceptron
{
    private double[] _weights;
    private double _learningRate;
    private double _bias;
    private StepFunctionEnum _stepFunction;

    private bool _isBipolar => _stepFunction == StepFunctionEnum.Bipolar;

    public Perceptron(
        double[] weights,
        double learningRate,
        double bias,
        StepFunctionEnum stepFunction
        ) : this(weights, learningRate, bias)
    {
        _stepFunction = stepFunction;
    }
    public Perceptron(double[] weights, double learningRate, double bias)
    {
        _weights = weights;
        _learningRate = learningRate;
        _bias = bias;
        _stepFunction = StepFunctionEnum.Unipolar;
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
        return sum >= 0 ? 1 : 0;
    }

    public double Train(TrainObject to)
    {
        return Train(to.Input, to.Solution);
    }
    public double Train(double[] input, double solution)
    {
        if (_isBipolar)
        {
            if (solution == 0) solution = -1;
        }

        var decision = Feedforward(input);

        var error = solution - decision;

        for (int i = 0; i < _weights.Length; i++)
        {
            _weights[i] += _learningRate * input[i] * error;
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
        sb.AppendLine($"{"bias".PadRight(15)} : {_bias}");
        var weights = _weights.Aggregate("", (acc, w) => acc += Math.Round(w, 4).ToString() + ", ");
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
            sb.Append(weight.ToString().Replace(',', '.'));
            sb.Append("*x" + i);
            if (i + i < _weights.Length) sb.Append("+");
        }
        sb.Append("+");
        sb.Append(_bias.ToString().Replace(',', '.'));
        sb.Append(";ezplot(f,[0,1]);");
        sb.Append("grid on;");
        return sb.ToString();
    }
}

public enum StepFunctionEnum
{
    Unipolar,
    Bipolar
}