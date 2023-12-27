using System;
using System.Text;

namespace DTT.MinigameMemory
{
    public class MemoryGameResults
    {
        public readonly TimeSpan timeTaken;
        public readonly int amountOfTurns;

        public MemoryGameResults(TimeSpan timeTaken, int amountOfTurns)
        {
            this.timeTaken = timeTaken;
            this.amountOfTurns = amountOfTurns;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Time taken (s): ");
            sb.Append(timeTaken.ToString(@"hh\:mm\:ss"));
            sb.Append('\t');
            sb.Append("Amount of turns taken: ");
            sb.Append(amountOfTurns);

            return sb.ToString();
        }
    }
}