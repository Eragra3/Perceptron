using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perceptron
{
    public static class CsvPrinter
    {
        public const char SEPARATOR = '|';

        public static void DumpLine(params object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                var v = values[i];
                Console.Write(v);
                if (i + 1 < values.Length) Console.Write(SEPARATOR);
            }
            Console.WriteLine();
        }

        public static void DumpHeaderLine(params string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                var v = values[i];
                Console.Write(v);
                if (i + 1 < values.Length) Console.Write(SEPARATOR);
            }
            Console.WriteLine();
        }

        public static void DumpParams(params KeyValuePair<string, object>[] values)
        {
            Console.WriteLine($"sep={SEPARATOR}");
            for (int i = 0; i < values.Length; i++)
            {
                var v = values[i];
                Console.Write($"{v.Key}:{v.Value}");
                if (i + 1 < values.Length) Console.Write(SEPARATOR);
            }
            Console.WriteLine();
        }
    }
}
