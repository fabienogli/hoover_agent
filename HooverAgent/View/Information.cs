using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace HooverAgent.View
{
    [Serializable]
    public class Information
    {
        private Dictionary<int, List<float>> Performances { get; set; }

        public Information()
        {
            Performances = new Dictionary<int, List<float>>();
        }

        public void addPerf(int n, float perf)
        {
            if (!Performances.ContainsKey(n))
            {
                Performances.Add(n, new List<float>());
            }
            Performances[n].Add(perf);
        }
    }
}