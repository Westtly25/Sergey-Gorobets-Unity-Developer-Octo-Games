using System.Collections.Generic;
using System.Linq;

namespace DTT.MinigameMemory
{
    public class AssignedValueShuffleAlgorithm : IShuffleAlgorithm
    {
        private System.Random random = new System.Random();

        private List<double> randoms = new List<double>();

        /// <summary>
        /// Shuffles the list to get a random ordered list.
        /// </summary>
        /// <typeparam name="T">Objects in the list.</typeparam>
        /// <param name="list">The list that needs to be shuffled.</param>
        /// <returns>The shuffled list.</returns>
        public List<T> Shuffle<T>(List<T> list)
        {
            randoms.Clear();

            while (randoms.Count < list.Count)
            {
                bool newUniqueValue = true;

                double randomValue = random.NextDouble();
                foreach (double value in randoms)
                {
                    if (value == randomValue)
                    {
                        newUniqueValue = false;
                        break;
                    }
                }

                if (!newUniqueValue)
                    continue;

                randoms.Add(randomValue);
            }

            Dictionary<double, T> keyValuePairs = new Dictionary<double, T>();
            for (int i = 0; i < list.Count; i++)
                keyValuePairs.Add(randoms[i], list[i]);

            var sortedDict = (from entry in keyValuePairs orderby entry.Key ascending select entry);

            for (int i = 0; i < list.Count; i++)
                list[i] = sortedDict.ElementAt(i).Value;

            return list;
        }
    }
}