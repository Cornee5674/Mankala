using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal abstract class RuleSet
    {
        protected int amountOfPockets;
        protected int stonesPerPocket;
        protected bool hasHomePockets;

        public RuleSet()
        {
        }

        public abstract bool SamePlayerAtTurn(Move move, Board board);
        public abstract bool GameIsFinished(Board board, Player p);
        public abstract Board MakeBoard(Board b,Player p1, Player p2);
        protected GeneralPocket GetOpposing(GeneralPocket originalPocket, Board board)
        {
            //Returns the pocket which is on the other side of the board
            int index = originalPocket.GetIndex();
            int x = amountOfPockets - index;
            //Overflow error
            int pocketIndexOpposing = amountOfPockets + 2 + x;
            if (pocketIndexOpposing >= board.pocketList.Length)
                pocketIndexOpposing -= board.pocketList.Length;
            return board.pocketList[pocketIndexOpposing];
        }
        protected void AddToHomePocket(Player p, int stones, Board board)
        {
            if (!hasHomePockets)
                throw new ImpossibleStateException("Can't add to Home pockets if there are none");
            if (board.pocketList[0].GetOwner() == p)
            {
                board.pocketList[0].AddStones(stones);
                return;
            }
            board.pocketList[amountOfPockets + 1].AddStones(stones);

        }
        public abstract bool IsForcedTurn(Move move, Board board);
    }

    public class ImpossibleStateException : Exception
    {
        public ImpossibleStateException(string message) : base(message) { }
    }
}
