using System;
using System.Collections.Generic;
using System.Text;

namespace HooverAgent.View
{
    [Serializable]
    public class Information
    {
        private Dictionary<int, List<float>> Performances { get; }

        public Information()
        {
            Performances = new Dictionary<int, List<float>>();
        }

        public void AddPerf(int n, float perf)
        {
            if (!Performances.ContainsKey(n))
            {
                Performances.Add(n, new List<float>());
            }
            Performances[n].Add(perf);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string DEPTH = "Depth ";
            string DEPTH_START = " :\n";
            string TRY_SEPARATOR = "\t";
            string DEPTH_SEPARATOR = "\n\n";
            
            foreach (KeyValuePair<int, List<float>> entry in Performances)
            {
                stringBuilder.Append(DEPTH)
                    .Append(entry.Key)
                    .Append(DEPTH_START);
                for (int i = 0; i < entry.Value.Count; i++)
                {
                    stringBuilder.Append(i)
                        .Append(TRY_SEPARATOR);
                }
                stringBuilder.Append(DEPTH_SEPARATOR);
            }
            return stringBuilder.ToString();
        }

       
    }
}