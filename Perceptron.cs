using System;
using System.Linq;
using System.Text;

public class Perceptron
{
    private double[] _weights;
    private double _learningRate;
    private double _bias;

    public Perceptron(double[] weights, double learningRate, double bias)
    {
        _weights = weights;
        _learningRate = learningRate;
        _bias = bias;
    }

    public double Net(double[] xs)
    {
        double sum = 0;
        for (int i = 0; i < xs.Length; i++)
        {
            sum += xs[i] * _weights[i];
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
    public double Train(double[] xs, double solution)
    {
        var decision = Feedforward(xs);

        var error = solution - decision;

        for (int i = 0; i < _weights.Length; i++)
        {
            _weights[i] += _learningRate * xs[i] * error;
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
        sb.AppendLine($"{"bias".PadRight(15)} : {_bias}");
        var weights = _weights.Aggregate("", (acc, w) => acc += Math.Round(w, 4).ToString() + ", ");
        sb.AppendLine($"{"weights".PadRight(15)} : {weights}");

        return sb.ToString();
    }
}