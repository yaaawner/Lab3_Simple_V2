using System;
using System.Collections.Generic;
using System.Text;

namespace Lab3_Simple_V2
{
    class DataChangedEventArgs
    {
        public ChangeInfo Change { get; set; }

        public double D { get; set; }

        public DataChangedEventArgs(ChangeInfo change, double d)
        {
            Change = change;
            D = d;
        }

        public override string ToString()
        {
            string ch = "default";
            switch (Change)
            {
                case ChangeInfo.ItemChanged:
                    ch = "ItemChanged";
                    break;
                case ChangeInfo.Add:
                    ch = "Add";
                    break;
                case ChangeInfo.Remove:
                    ch = "Remove";
                    break;
                case ChangeInfo.Replace:
                    ch = "Replace";
                    break;
                default:
                    break;
            }

            return "Change: " + ch + "; Value: " + D.ToString();
        }

    }
}
