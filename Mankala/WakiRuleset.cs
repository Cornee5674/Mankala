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

        public override bool IsForcedTurn(Move move, Board board)
        {
            //Waki does not have forced moves
            return false;
        }


        public override bool NeedToSwitchPlayer(Move move, Board board)
        {
            //Returns true if the other player has the turn after this move
            Player playerAtTurn = move.currentPlayer;
            GeneralPocket endingPocket = move.endingPocket;
            //If the ending pocket is of the player at turn, the opposing player gets to play
            if (endingPocket.IsOwner(playerAtTurn))
                return true;
            //If there is 2 or 3 stones in the ending pocket after a move, they are all thrown into the homepocket
            if(endingPocket.AmountofStones == 2 || endingPocket.AmountofStones == 3)
            {
                int stones = endingPocket.EmptyPocket();
                board.AddToHomePocket(playerAtTurn, stones);
            }
            return true;

        }
    }
}
