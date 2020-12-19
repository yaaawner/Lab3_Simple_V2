using System;
using System.Numerics;
using System.IO;

namespace Lab2_V2_1
{
    struct DataItem
    {
        public Vector2 Vector { get; set; }
        public Complex Complex { get; set; }

        public DataItem(Vector2 vector, Complex complex)
        {
            Vector = vector;
            Complex = complex;
        }

        public override string ToString()
        {
            return "Vector: " + Vector.X.ToString() + " " + Vector.Y.ToString() + " " +
                   "Complex: " + Complex.ToString();
        }

        public string ToString(string format)
        {
            return "Vector: " + Vector.X.ToString(format) + " " + Vector.Y.ToString(format) + " " +
                   "Complex: " + Complex.ToString(format);
        }

    }

    struct Grid1D
    {
        public float Step { get; set; }
        public int Num { get; set; }

        public Grid1D(float step, int num)
        {
            Step = step;
            Num = num;
        }

        public override string ToString()
        {
            return "Step: " + Step.ToString() + "; Num: " + Num.ToString();
        }

        public string ToString(string format)
        {
            return "Step: " + Step.ToString(format) + "; Num: " + Num.ToString(format);
        }
    }

    abstract class V2Data
    {
        public string Info { get; set; }
        public double Freq { get; set; }

        public V2Data(string info, double freq)
        {
            Info = info;
            Freq = freq;
        }

        public V2Data()
        {
            Info = "info";
            Freq = 100;
        }

        public abstract Complex[] NearAverage(float eps);
        public abstract string ToLongString();
        public abstract string ToLongString(string format);

        public override string ToString()
        {
            return "Info: " + Info + " Frequency: " + Freq.ToString();
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            /* 1 */
            try
            {
                V2DataOnGrid grid = new V2DataOnGrid("inputfile.txt");
                Console.WriteLine(grid.ToLongString("n"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //Console.WriteLine("========= DIRECTORY =========");
            //Directory.SetCurrentDirectory("..\\..\\..\\");
            //Console.WriteLine(Directory.GetCurrentDirectory());

            /* 2 */
            try
            {
                V2MainCollection mainCollection = new V2MainCollection();
                mainCollection.AddDefaults();
                Console.WriteLine(mainCollection.ToLongString("n"));

                Console.WriteLine();
                Console.WriteLine("Среднее значение модуля поля для всех результатов измерений в коллекции V2MainCollection:");
                Console.WriteLine(mainCollection.Average.ToString());

                Console.WriteLine();
                Console.WriteLine("Значение модуля поля ближе всего к среднему значению модуля поля среди всех результатов измерений:");
                Console.WriteLine(mainCollection.NearAverage.ToString());

                Console.WriteLine();
                Console.WriteLine("Экземпляры Vector2 точки измерения поля, которые встречаются в каждом элементе типа V2DataCollection в коллекции V2MainCollection:");
                foreach (Vector2 v in mainCollection.Vectors)
                {
                    Console.WriteLine(v.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}