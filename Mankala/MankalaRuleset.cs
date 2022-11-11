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
            //Default Mankala rules
            amountOfPockets = 6;
            hasHomePockets = true;
            stonesPerPocket = 4;
            hasForcedMoves = true;
        }
        public MankalaRuleset(int amountOfPockets, int stonesPerPocket)
        {
            //Mankala rules with own variation
            this.amountOfPockets = amountOfPockets;
            this.stonesPerPocket = stonesPerPocket;
            hasHomePockets = true;
            hasForcedMoves = true;
        }
        public override bool NeedToSwitchPlayer(Move move, Board board)
        {
            //Method returns true if the other player has the turn after a move
            Player playerAtTurn = move.currentPlayer;
            GeneralPocket endingPocket = move.endingPocket;

            bool endPocketIsOwn = endingPocket.IsOwner(playerAtTurn);
            
            //An empty pocket has 1 stone after the move is done
            bool endPocketIsEmpty = endingPocket.AmountofStones == 1;
            GeneralPocket opposing = GetOpposing(endingPocket, board);
            bool opposingIsEmpty = opposing.AmountofStones <= 1;

            //Last stone lands in homePocket of player
            if (endingPocket is HomePocket)
                return false;

            //Last stone ends in non empty pocket
            if (endingPocket.AmountofStones > 1)
                throw new Exception("This point shouldn't be reached"); //Shouldn't be reached cuz forced move

            //Last stone ends in empty pocket of opponent
            if (endPocketIsEmpty && !endPocketIsOwn)
                return true;

            //Last stone ends in empty pocket of player and opposing is empty
            if (endPocketIsEmpty && endPocketIsOwn && opposingIsEmpty)
                return true;

            //Last stone ends in empty pocket of player and opposing is not empty 
            if(endPocketIsEmpty && endPocketIsOwn && !opposingIsEmpty)
            {
                //Stones of ending pocket and opposing pocket will be added to homepocket
                int a = endingPocket.EmptyPocket();
                int b = opposing.EmptyPocket();

                //Add stones to homepocket of the player
                int homePocketIndex = board.GetHomePocket(playerAtTurn).Index;
                board.GetAtIndex(homePocketIndex).AddStones(a + b);
                return true;
            }
            throw new Exception("This should not be reached");
        }

        public override Board MakeBoard(Board b, Player p1, Player p2)
        {
            //Creates a factory and asks the factory to create a board
            BoardFactory f = new BoardFactory();
            return f.CreateBoard(amountOfPockets,hasHomePockets,stonesPerPocket,p1,p2);
        }
        public override bool IsForcedTurn(Move move, Board board)
        {
            //If the last stone lands in another pocket than the home pocket and it isn't empty its a forced turn
            if (move.endingPocket is Pocket && move.endingPocket.AmountofStones > 1)
                return true;
            return false;

        }
    }
}
