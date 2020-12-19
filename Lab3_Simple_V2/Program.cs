using System;
using System.Numerics;

namespace Lab3_Simple_V2
{

    enum ChangeInfo { ItemChanged, Add, Remove, Replace};

    delegate void DataChangedEventHandler(object source, DataChangedEventArgs args);

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

    class Program
    {
        private static void DataChangedHandler(object source, DataChangedEventArgs args)
        {
            Console.WriteLine(args.ToString());
        }

        static void Main(string[] args)
        {
            try
            {
                // 1
                V2MainCollection mainCollection = new V2MainCollection();
                mainCollection.DataChanged += DataChangedHandler;

                // add
                Console.WriteLine("====== ADD ======");
                mainCollection.AddDefaults();
                //Console.WriteLine(mainCollection.Count.ToString());

                // replace
                Console.WriteLine();
                Console.WriteLine("====== REPLACE ======");
                V2DataCollection buf = new V2DataCollection("replace", 783.3);
                mainCollection[2] = buf;

                // change
                Console.WriteLine();
                Console.WriteLine("====== CHANGE ======");
                mainCollection[3].Info = "lalala";
                mainCollection[3].Freq = 9999.99;

                // remove
                Console.WriteLine();
                Console.WriteLine("====== REMOVE ======");
                mainCollection.Remove("lalala", 9999.99);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}