using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal class WakiRuleset : RuleSet
    {
        public WakiRuleset()
        {
            amountOfPockets = 6;
            hasHomePockets = false;
            stonesPerPocket = 4;
            hasForcedMoves = false;
        }
        public WakiRuleset(int amountOfPockets, int stonesPerPocket)
        {
            this.amountOfPockets = amountOfPockets;
            hasHomePockets = false;
            this.stonesPerPocket = stonesPerPocket;
            hasForcedMoves = false;
        }
        public override bool GameIsFinished(Board board, Player p)
        {
            return base.GameIsFinished(board, p);
        }

        public override bool IsForcedTurn(Move move, Board board)
        {
            //Waki does no have forced moves
            return false;
        }

        public override Board MakeBoard(Board b, Player p1, Player p2)
        {
            if(b is null)
            {
                WakiFactory f = new WakiFactory();
                Board res = f.createBoard(amountOfPockets, hasHomePockets, stonesPerPocket, p1, p2);
                return res;
            }
            return b;
        }

        public override bool SamePlayerAtTurn(Move move, Board board)
        {
            Player playerAtTurn = move.currentPlayer;
            GeneralPocket endingPocket = move.endingPocket;
            //If the ending pocket is of the player at turn, the opposing player gets to play
            if (endingPocket.GetOwner() == playerAtTurn)
                return true;
            if(endingPocket.AmountOfStones() == 2 || endingPocket.AmountOfStones() == 3)
            {
                int stones = endingPocket.EmptyPocket();
                AddToHomePocket(playerAtTurn, stones, board);
            }
            return true;

        }
    }
}
