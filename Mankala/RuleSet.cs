using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal abstract class RuleSet
    {
        int amountOfPockets;
        int stonesPerPocket;
        bool hasHomePockets;

        public RuleSet()
        {

        }

        public abstract Player GetNextPlayer(Move move);
        public abstract bool GameIsFinished(Board board);

        public Board MakeBoard()
        {
            return null;
        }

    }
}
