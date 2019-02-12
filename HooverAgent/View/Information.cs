using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HooverAgent.View
{
    [Serializable]
    public class Information
    {
        private Dictionary<int, List<float>> Performances { get; set; }
        private static string FileName = "performances";
        static string DEPTH = "Depth ";
        static string TRY_SEPARATOR = "\t";
        static string DEPTH_START = " :\n";
        static string DEPTH_SEPARATOR = "\n\n";

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


            foreach (KeyValuePair<int, List<float>> entry in Performances)
            {
                stringBuilder.Append(DEPTH)
                    .Append(entry.Key)
                    .Append(DEPTH_START);
                for (int i = 0; i < entry.Value.Count; i++)
                {
                    stringBuilder.Append(entry.Value[i])
                        .Append(TRY_SEPARATOR);
                }

                stringBuilder.Append(DEPTH_SEPARATOR);
            }

            return stringBuilder.ToString();
        }

        public void Save()
        {
            File.WriteAllText(FileName, ToString());
        }
        public int GetDepthWithBestMean()
        {
            var maxMean = float.MinValue;
            var bestN = -1;
            foreach (var (depth, _) in Performances)
            {
                var mean = ProcessMeanForDepth(depth);
                if (mean > maxMean)
                {
                    maxMean = mean;
                    bestN = depth;
                }
            }

            return bestN;
        }

        private float ProcessMeanForDepth(int n)
        {
            float mean = 0;
            var performances = Performances[n];
            foreach (var perf in performances)
            {
                mean += perf;
            }

            mean /= performances.Count;
            return mean;
        }
    }
}