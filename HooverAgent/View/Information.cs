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
            Console.WriteLine("Saved to " + FileName);
            File.WriteAllText(FileName, ToString());
        }

        public static Information Import()
        {
            string rawLines = File.ReadAllText(FileName);
            string[] depthLines = rawLines.Split(DEPTH_SEPARATOR);
            Information information = new Information();
            for (int depthCount = 0; depthCount < depthLines.Length; depthCount++)
            {
                string[] rawLine = depthLines[depthCount].Split(DEPTH_START);
                if (rawLine.Length < 2)
                {
                    continue;                    
                } 
                if (rawLine[0].Split(DEPTH).Length < 2 || rawLine[1].Split(TRY_SEPARATOR).Length < 2)
                {
                    continue;
                }
                int depth = int.Parse(rawLine[0].Split(DEPTH)[1]);
                string[]rawPerfs = rawLine[1].Split(TRY_SEPARATOR);
                for (int perfCount = 0; perfCount < rawPerfs.Length -1; perfCount++) //because the last one is empty
                {
                    Console.WriteLine(rawPerfs[perfCount]);
                    information.AddPerf(depth, float.Parse(rawPerfs[perfCount]));
                }
            }
            Console.WriteLine("Imported from " + FileName);
            return information;
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