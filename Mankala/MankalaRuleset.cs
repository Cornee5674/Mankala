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
        }
        public override bool GameIsFinished(Board board, Player playerAtTurn)
        {
            //In Mankala the game ends when a player has no more pockets with stones in them
            GeneralPocket[] temp = board.pocketList;
            for (int i = 0; i < temp.Length; i++)
            {
                if (!(temp[i] is Pocket)) //Ignore homePockets
                    continue;
                if (temp[i].GetOwner() != playerAtTurn) //Ignore pockets of the opposing player
                    continue;
                if (temp[i].AmountOfStones() != 0) // If any pocket is not empty the game isnt finished yet
                    return false;
            }
            return true;
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
            bool opposingIsEmpty = opposing.AmountOfStones() == 1;

            //These Cases dont entirely work right now :(
            if (endPocketIsOwn && endingPocket is HomePocket)
                return true;
            if (!endPocketIsOwn && endPocketIsEmpty)
                return false;
            if (!endPocketIsOwn && opposingIsEmpty)
                return true;
            if (endPocketIsOwn && opposing.IsEmpty())
                return false;
            if (endPocketIsOwn && endPocketIsEmpty && !opposingIsEmpty)
            {
                int stonesForHome = 1;
                endingPocket.AddStones(-1);
                stonesForHome += opposing.EmptyPocket();
                AddToHomePocket(playerAtTurn, stonesForHome,board);
                return false;
            }
            //Default case, shouldn't be reached either way
            return false;

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
            if (move.endingPocket is Pocket && !move.endingPocket.IsEmpty())
                return true;
            return false;

        }
    }
}
