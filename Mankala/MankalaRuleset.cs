using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal class MankalaRuleset : RuleSet
    {
        public MankalaRuleset()
        {
            amountOfPockets = 6;
            hasHomePockets = true;
            stonesPerPocket = 4;
            hasForcedMoves = true;
        }
        public MankalaRuleset(int amountOfPockets, int stonesPerPocket)
        {
            this.amountOfPockets = amountOfPockets;
            this.stonesPerPocket = stonesPerPocket;
            hasHomePockets = true;
            hasForcedMoves = true;
        }
        public override bool GameIsFinished(Board board, Player playerAtTurn)
        {
            return base.GameIsFinished(board, playerAtTurn);
        }

        public override bool SamePlayerAtTurn(Move move, Board board)
        {
            //Tells whether or not the same player has the turn after a move
            Player playerAtTurn = move.currentPlayer;
            GeneralPocket endingPocket = move.endingPocket;

            bool endPocketIsOwn = endingPocket.GetOwner() == playerAtTurn;
            
            //An empty pocket has 1 stone after the move is done
            bool endPocketIsEmpty = endingPocket.AmountOfStones() == 1;
            GeneralPocket opposing = GetOpposing(endingPocket, board);
            bool opposingIsEmpty = opposing.AmountOfStones() <= 1;

            //Last stone lands in homePocket of player
            if (endingPocket is HomePocket)
                return false;

            //Last stone ends in non empty pocket
            if (endingPocket.AmountOfStones() > 1)
                return false; //Shouldn't be reached cuz forced move

            //Last stone ends in empty pocket of opponent
            if (endPocketIsEmpty && !endPocketIsOwn)
                return true;

            //Last stone ends in empty pocket of player and opposing is empty
            if (endPocketIsEmpty && endPocketIsOwn && opposingIsEmpty)
                return true;

            //Last stone ends in empty pocket of player and opposing is not empty 
            if(endPocketIsEmpty && endPocketIsOwn && !opposingIsEmpty)
            {
                int a = endingPocket.EmptyPocket();
                int b = opposing.EmptyPocket();
                GeneralPocket[] pockets = board.pocketList;
                if (pockets[0].GetOwner() == playerAtTurn)
                    board.pocketList[0].AddStones(a + b);
                else
                    board.pocketList[pockets.Length / 2].AddStones(a + b);
                return true;
            }
            throw new Exception("This should not be reached");
        }

        public override Board MakeBoard(Board b, Player p1, Player p2)
        {
            if(b is null)
            {
                MankalaFactory f = new MankalaFactory();
                return f.createBoard(amountOfPockets,hasHomePockets,stonesPerPocket,p1,p2);
            }
            return b;
            
        }


        public override bool IsForcedTurn(Move move, Board board)
        {
            //If the last stone lands in another pocket than the home pocket and it isn't empty its a forced turn
            if (move.endingPocket is Pocket && move.endingPocket.AmountOfStones() > 1)
                return true;
            return false;

        }
    }
}
