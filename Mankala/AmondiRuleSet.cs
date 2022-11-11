using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    class AmondiRuleSet : RuleSet
    {
        public AmondiRuleSet()
        {
            amountOfPockets = 6;
            hasHomePockets = false;
            stonesPerPocket = 4;
            hasForcedMoves = false;
        }
        public AmondiRuleSet(int amountOfPockets, int stonesPerPocket)
        {
            this.amountOfPockets = amountOfPockets;
            hasHomePockets = true;
            this.stonesPerPocket = stonesPerPocket;
            hasForcedMoves = false;
        }
        public override bool IsForcedTurn(Move move, Board board)
        {
            return false;
        }

        public override bool NeedToSwitchPlayer(Move move, Board board)
        {
            //Method returns true if the other player has the turn after a move
            Player playerAtTurn = move.currentPlayer;
            GeneralPocket endingPocket = move.endingPocket;
            bool endPocketIsEmpty = endingPocket.AmountofStones == 1;
            bool endPocketIsOwn = endingPocket.IsOwner(playerAtTurn);

            if (endPocketIsOwn && endingPocket is HomePocket)
                return false;
            if(endPocketIsEmpty && !endPocketIsOwn)
            {
                //Left pocket
                

                int takeAmountRight = 0;
                GeneralPocket right;
                
                
                right = board.GetAtIndex(endingPocket.Index - 1);
                if(right is Pocket)
                {
                    //Get half of the stones from the right pocket and take those from the pocket
                    takeAmountRight = GetHalf(right);
                    right.AddStones(takeAmountRight * -1);
                }
                //Right pocket
                int takeAmountLeft = 0;
                GeneralPocket left;
                if (endingPocket.Index == board.ListLength - 1) //Avoid going outside the boundaries
                    left = board.GetAtIndex(0);
                else
                    left = board.GetAtIndex(endingPocket.Index + 1);
                if (left is Pocket)
                {
                    //Get half of the stones from the left pocket and take those from the pocket
                    takeAmountLeft = GetHalf(left);
                    left.AddStones(takeAmountLeft * -1);
                }
                //Add all these stones to the homepocket
                board.AddToHomePocket(playerAtTurn,takeAmountLeft + takeAmountRight);
            }
            return true;

        }
        public override bool GameIsFinished(Board board, Player playerAtTurn)
        {
            bool oneHas20 = board.HomepocketP1.AmountofStones >= 20 || board.HomepocketP2.AmountofStones >= 20;
            return base.GameIsFinished(board, playerAtTurn) || oneHas20;
        }

        private int GetHalf(GeneralPocket p)
        {
            decimal half = p.AmountofStones / 2;
            return (int)Math.Floor(half);
        }
    }
}
