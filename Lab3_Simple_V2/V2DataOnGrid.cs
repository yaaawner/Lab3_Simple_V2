using System;
using System.Collections.Generic;
using System.Numerics;
using System.IO;
using System.Collections;
using System.Globalization;

namespace Lab3_Simple_V2
{
    class V2DataOnGrid : V2Data, IEnumerable<DataItem>
    {
        public Grid1D[] Grids { get; set; }
        public Complex[,] Node { get; set; }

        public V2DataOnGrid(string info, double freq, Grid1D ox, Grid1D oy) : base(info, freq)
        {
            Info = info;
            Freq = freq;
            Grids = new Grid1D[2] { ox, oy };
        }

        public V2DataOnGrid(string filename) : base()
        {
            /* ФОРМАТ ВХОДНОГО ФАЙЛА
             * 1 строка - Info      - информация о таблице
             * 2 строка - Freq      - значение частоты
             * 3 строка - Grid1D Ox - значения Num и Step через пробел
             * 4 строка - Grid1D Oy - значения Num и Step через пробел
             * 
             * 5 строка и дальше - таблица
             * Узел таблицы разделяется пробелом
             * Поля комлексного числа (действительная и мнимая части) нижним подчеркиванием
             * 
             */

            /********* Пример входного файла *********/
            /*
             * Information
             * 100
             * 3 10
             * 5 10
             * 12,0_5,23 11,67_7,78 7,16_4,00 8,34_-7,55 1,09_1,09
             * 2,44_-5,43 16,11_7,90 -2,33_4,55 8,76_-7,10 1,01_-1,40
             * 1,67_5,29 1,10_7,36 7,18_-44,44 4,89_-1,10 1,09_1,34
             * 
             */
            CultureInfo CIru = new CultureInfo("RU-ru");        // исправлена локализация

            FileStream fs = null;
            try 
            {
                //Directory.SetCurrentDirectory()
                Directory.SetCurrentDirectory("..\\..\\..\\");  // путь к исходным файлам проекта
                fs = new FileStream(filename, FileMode.Open);
                StreamReader reader = new StreamReader(fs);

                Info = reader.ReadLine();
                Freq = double.Parse(reader.ReadLine(), CIru);
                string[] str;
                Grids = new Grid1D[2];

                for (int i = 0; i < 2; i++)
                {
                    str = reader.ReadLine().Split(' ');
                    Grids[i].Num = int.Parse(str[0], CIru);
                    Grids[i].Step = float.Parse(str[1], CIru);
                }

                Node = new Complex[Grids[0].Num, Grids[1].Num];
                string[] strnode;
                string[] strcompl;

                for (int i = 0; i < Grids[0].Num; i++)
                {
                    strnode = reader.ReadLine().Split(' ');
                    for (int j = 0; j < Grids[1].Num; j++)
                    {
                        strcompl = strnode[j].Split('_');               // исправлен разделитель
                        Node[i, j] = new Complex(double.Parse(strcompl[0], CIru), double.Parse(strcompl[1], CIru));
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        

        public void initRandom(double minValue, double maxValue)
        {
            Node = new Complex[Grids[0].Num, Grids[1].Num];
            Random rnd = new Random();

            for (int i = 0; i < Grids[0].Num; i++)
            {
                for (int j = 0; j < Grids[1].Num; j++)
                {
                    Node[i, j] = new Complex(rnd.NextDouble() * (maxValue - minValue), rnd.NextDouble() * (maxValue - minValue));
                }
            }
        }

        public static explicit operator V2DataCollection(V2DataOnGrid val)
        {
            V2DataCollection ret = new V2DataCollection(val.Info, val.Freq);

            for (int i = 0; i < val.Grids[0].Num; i++)
            {
                for (int j = 0; j < val.Grids[1].Num; j++)
                {
                    ret.dataItems.Add(new DataItem(new Vector2((i + 1) * val.Grids[0].Step, 
                        (j + 1) * val.Grids[1].Step), val.Node[i, j]));
                }
            }

            return ret;
        }

        public override Complex[] NearAverage(float eps)
        {
            int N = Grids[0].Num * Grids[1].Num;
            double sum = 0;

            for (int i = 0; i < Grids[0].Num; i++)
            {
                for (int j = 0; j < Grids[1].Num; j++)
                {
                    sum += Node[i, j].Real;
                }
            }

            double average = sum / N;
            int count = 0;
            for (int i = 0; i < Grids[0].Num; i++)
            {
                for (int j = 0; j < Grids[1].Num; j++)
                {
                    if (Math.Abs(Node[i, j].Real - average) < eps)
                    {
                        count++;
                    }
                }
            }

            Complex[] ret = new Complex[count];
            count = 0;
            for (int i = 0; i < Grids[0].Num; i++)
            {
                for (int j = 0; j < Grids[1].Num; j++)
                {
                    if (Math.Abs(Node[i, j].Real - average) < eps)
                    {
                        ret[count++] = Node[i, j];
                    }
                }
            }

            return ret;
        }

        public override string ToString()
        {
            return "Type: 2DataOnGrid Base: Info: " + Info.ToString() + " Freq: " + Freq.ToString()
                 + " Ox: " + Grids[0].ToString() + " Oy: " + Grids[1].ToString();
        }

        public override string ToLongString()
        {
            string ret = "";

            for (int i = 0; i < Grids[0].Num; i++)
            {
                for (int j = 0; j < Grids[1].Num; j++)
                {
                    ret = ret + (" (" + (Grids[0].Step * (i + 1)).ToString() + ", " + (Grids[1].Step * (j + 1)).ToString()
                              + ") Value: " + Node[i, j].ToString()) + "\n";
                }
                ret = ret + "\n";
            }

            return "Type: 2DataOnGrid Base: Info: " + Info + " Freq: " + Freq.ToString()
                 + " Ox: " + Grids[0].ToString() + " Oy: " + Grids[1].ToString() + "\n" + ret;
        }

        public override string ToLongString(string format)
        {
            string ret = "";

            for (int i = 0; i < Grids[0].Num; i++)
            {
                for (int j = 0; j < Grids[1].Num; j++)
                {
                    ret = ret + (" (" + (Grids[0].Step * (i + 1)).ToString(format) + ", "
                              + (Grids[1].Step * (j + 1)).ToString(format)
                              + ") Value: " + Node[i, j].ToString(format)) + "\n";
                }
                ret = ret + "\n";
            } 
            
            return "Type: 2DataOnGrid Base: Info: " + Info + " Freq: " + Freq.ToString(format)
                     + " Ox: " + Grids[0].ToString(format) + " Oy: " + Grids[1].ToString(format) + "\n" + ret;
        }


        public IEnumerator<DataItem> GetEnumerator()
        {
            V2DataCollection ret = new V2DataCollection(Info, Freq);

            for (int i = 0; i < Grids[0].Num; i++)
            {
                for (int j = 0; j < Grids[1].Num; j++)
                {
                    ret.dataItems.Add(new DataItem(new Vector2((i + 1) * Grids[0].Step,
                        (j + 1) * Grids[1].Step), Node[i, j]));
                }
            }
            return ((IEnumerable<DataItem>)ret.dataItems).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            V2DataCollection ret = new V2DataCollection(Info, Freq);

            for (int i = 0; i < Grids[0].Num; i++)
            {
                for (int j = 0; j < Grids[1].Num; j++)
                {
                    ret.dataItems.Add(new DataItem(new Vector2((i + 1) * Grids[0].Step,
                        (j + 1) * Grids[1].Step), Node[i, j]));
                }
            }
            return ((IEnumerable)ret.dataItems).GetEnumerator();
        }
    }
}