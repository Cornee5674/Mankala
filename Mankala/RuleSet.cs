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
        public bool hasForcedMoves;
        public RuleSet()
        {
        }
        public abstract bool NeedToSwitchPlayer(Move move, Board board);
        public virtual bool GameIsFinished(Board board, Player playerAtTurn)
        {
            //In most versions the game ends when a player has no more pockets with stones in them
            for (int i = 0; i < board.ListLength; i++)
            {
                if (!(board.GetAtIndex(i) is Pocket)) //Ignore homePockets
                    continue;
                if (!board.GetAtIndex(i).IsOwner(playerAtTurn)) //Ignore pockets of the opposing player
                    continue;
                if (board.GetAtIndex(i).AmountofStones != 0) // If any pocket is not empty the game isnt finished yet
                    return false;
            }
            return true;
        }
        public abstract bool IsForcedTurn(Move move, Board board);
        public Board MakeBoard(Player p1, Player p2)
        {
            BoardFactory f = new BoardFactory();
            return f.CreateBoard(amountOfPockets, hasHomePockets, stonesPerPocket, p1, p2);
        }
        
    }

    public class ImpossibleStateException : Exception
    {
        public ImpossibleStateException(string message) : base(message) { }
    }
}
