using System;
using System.Numerics;
using System.Collections.Generic;
using System.Collections;

namespace Lab2_V2_1
{
    class V2DataCollection : V2Data, IEnumerable<DataItem>
    {
        public List<DataItem> dataItems { get; set; }

        public V2DataCollection(string info, double freq) : base(info, freq)
        {
            dataItems = new List<DataItem>();
            Info = info;
            Freq = freq;
        }

        public void initRandom(int nItems, float xmax, float ymax, double minValue, double maxValue)
        {
            dataItems = new List<DataItem>();
            Random rnd = new Random();
            for (int i = 0; i < nItems; i++)
            {
                dataItems.Add(new DataItem()
                {
                    Vector = new Vector2((float)rnd.NextDouble() * xmax, (float)rnd.NextDouble() * ymax),
                    Complex = new Complex(rnd.NextDouble() * (maxValue - minValue), rnd.NextDouble() * (maxValue - minValue))
                });
            }
        }

        public override Complex[] NearAverage(float eps)
        {
            int count = 0;
            double sum = 0;
            foreach (DataItem item in dataItems)
            {
                sum += item.Complex.Real;
            }

            double average = sum / dataItems.Count;

            foreach (DataItem item in dataItems)
            {
                if (Math.Abs(item.Complex.Real - average) < eps)
                {
                    count++;
                }
            }

            Complex[] ret = new Complex[count];
            count = 0;
            foreach (DataItem item in dataItems)
            {
                if (Math.Abs(item.Complex.Real - average) < eps)
                {
                    ret[count++] = item.Complex;
                }
            }

            return ret;
        }

        public override string ToString()
        {
            return "Type: V2DataCollection Base: info " + Info + " freq " + Freq.ToString()
                    + " Count: " + dataItems.Count.ToString();
        }

        public override string ToLongString()
        {
            string ret = "";

            foreach (DataItem item in dataItems)
            {
                ret += (item.ToString() + "\n");
            }

            return "Type: V2DataCollection Base: info " + Info + " freq " + Freq.ToString()
                    + " Count: " + dataItems.Count.ToString() + "\n" + ret;
        }

        public override string ToLongString(string format)
        {
            string ret = "";

            foreach (DataItem item in dataItems)
            {
                ret += (item.ToString(format) + "\n");
            }

            return "Type: V2DataCollection Base: info " + Info + " freq " + Freq.ToString(format)
                    + " Count: " + dataItems.Count.ToString(format) + "\n" + ret;
        }

        public IEnumerator<DataItem> GetEnumerator()
        {
            return ((IEnumerable<DataItem>)dataItems).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dataItems).GetEnumerator();
        }
    }
}