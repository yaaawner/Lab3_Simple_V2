using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using System.Numerics;
using System.ComponentModel;

namespace Lab3_Simple_V2
{
    class V2MainCollection : IEnumerable<V2Data>
    {
        private List<V2Data> v2Datas;

        public event DataChangedEventHandler DataChanged;

        protected void PropertyHandler(object source, PropertyChangedEventArgs args)
        {
            if (DataChanged != null)
            {
                DataChanged(this, new DataChangedEventArgs(ChangeInfo.ItemChanged, this.Count));
            }
        }

        public V2Data this[int index]
        {
            get
            {
                return v2Datas[index];
            }
            set
            {
                v2Datas[index] = value;
                
                if (DataChanged != null)
                {
                    DataChanged(this, new DataChangedEventArgs(ChangeInfo.Replace, v2Datas.Count));
                }
                
            }
        }

        public int Count
        {
            get { return v2Datas.Count; }
        }

        public void Add(V2Data item)
        {
            v2Datas.Add(item);
            if (DataChanged != null)
            {
                DataChanged(this, new DataChangedEventArgs(ChangeInfo.Add, item.Freq));
            }
            item.PropertyChanged += PropertyHandler;
        }

        public bool Remove(string id, double w)
        {
            bool flag = false;

            for (int i = 0; i < v2Datas.Count;)
            {
                if (v2Datas[i].Freq == w && v2Datas[i].Info == id)
                {
                    v2Datas[i].PropertyChanged -= PropertyHandler;
                    v2Datas.Remove(v2Datas[i]);
                    flag = true;
                }
                else
                {
                    i++;
                }
            }

            if (DataChanged != null)
            {
                DataChanged(this, new DataChangedEventArgs(ChangeInfo.Remove, w));
            }

            return flag;
        }

        public void AddDefaults()
        {
            Grid1D Ox = new Grid1D(10, 3);
            Grid1D Oy = new Grid1D(10, 3);
            v2Datas = new List<V2Data>();
            V2DataOnGrid[] grid = new V2DataOnGrid[4];
            V2DataCollection[] collections = new V2DataCollection[4];

            for (int i = 0; i < 3; i++)
            {
                grid[i] = new V2DataOnGrid("data info2"/*+ i.ToString()*/, 2, Ox, Oy);     // test i = 2
                collections[i] = new V2DataCollection("collection info" + i.ToString(), i);
            }

            for (int i = 0; i < 3; i++)
            {
                grid[i].initRandom(0, 100);
                collections[i].initRandom(4, 100, 100, 0, 100);
                v2Datas.Add(grid[i]);
                v2Datas.Add(collections[i]);
            }

            Grid1D nullOx = new Grid1D(0, 0);
            Grid1D nullOy = new Grid1D(0, 0);
            grid[3] = new V2DataOnGrid("null", 100, nullOx, nullOy);
            collections[3] = new V2DataCollection("null", 100);

            grid[3].initRandom(0, 100);
            collections[3].initRandom(0, 100, 100, 0, 100);
            v2Datas.Add(grid[3]);
            v2Datas.Add(collections[3]);
        }

        public override string ToString()
        {
            string ret = "";
            foreach (V2Data data in v2Datas)
            {
                ret += (data.ToString() + '\n');
            }
            return ret;
        }

        public IEnumerator<V2Data> GetEnumerator()
        {
            return ((IEnumerable<V2Data>)v2Datas).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)v2Datas).GetEnumerator();
        }

        public string ToLongString(string format)
        {
            string ret = "";
            foreach (V2Data data in v2Datas)
            {
                ret += (data.ToLongString(format) + '\n');
            }
            return ret;
        }

        public double Average
        {
            get {
                IEnumerable<DataItem> collection = from elem in (from data in v2Datas
                                                                 where data is V2DataCollection
                                                                 select (V2DataCollection)data)
                                                   from item in elem
                                                   select item;

                IEnumerable<DataItem> grid = from elem in (from data in v2Datas
                                                                 where data is V2DataOnGrid
                                                                 select (V2DataOnGrid)data)
                                                   from item in elem
                                                   select item;

                IEnumerable<DataItem> items = collection.Union(grid);

                return items.Average(n => n.Complex.Magnitude);
            }
        }

        public DataItem NearAverage
        {
            get
            {
                double a = this.Average;

                IEnumerable<DataItem> collection = from elem in (from data in v2Datas
                                                                 where data is V2DataCollection
                                                                 select (V2DataCollection)data)
                                                   from item in elem
                                                   select item;

                IEnumerable<DataItem> grid = from elem in (from data in v2Datas
                                                           where data is V2DataOnGrid
                                                           select (V2DataOnGrid)data)
                                             from item in elem
                                             select item;

                IEnumerable<DataItem> items = collection.Union(grid);

                var dif = from item in items
                          select Math.Abs(item.Complex.Magnitude - a);

                double min = dif.Min();

                var ret = from item in items
                          where Math.Abs(item.Complex.Magnitude - a) <= min
                          select item;

                //Console.WriteLine(ret.First().Complex.Magnitude);
                return ret.First();
            }
        }

        public IEnumerable<Vector2> Vectors
        {
            get
            { 

                return from elem in (from data in v2Datas
                                     where data is V2DataCollection
                                     select (V2DataCollection)data)
                       from item in elem
                       select item.Vector;
            }
        }
    }
}
