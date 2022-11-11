using System;
using Xunit;
using Mankala;

namespace MankalaUnitTests
{
    /* Classes to test:
    - RuleSet: GameIsFinished(), GetOpposing(), AddToHomePocket()
    - WakiliRuleset : NeedToSwitchPlayer()
    - MankalaRuleset: NeedToSwitchPlayer(), IsForcedTurn()
    - Pocket: UpdatePocket()
    - HomePocket: UpdatePocket(), EmptyPocket()
    - GeneralPocket: EmptyPocket(), AddStones(), IsOwner()
    - Factory: CreateBoard()
    - Board: DoTurn(), GetAtIndex(), GetHomePocket()

    */
    public class RuleSetTests
    {
        BoardFactory boardFactory = new BoardFactory();
 
        [Fact]
        void TestGameIsFinished()
        {
            Player player1 = new Player("f");
            Player player2 = new Player("d");
            Board board1 = boardFactory.CreateBoard(3, true, 6, player1, player2);
            Board board2 = boardFactory.CreateBoard(1, false, 9, player1, player2);
            Board board3 = boardFactory.CreateBoard(7, true, 2, player1, player2);


        }

        [Fact]
        void TestGetOpposing()
        {

        }

        [Fact]
        void TestAddToHomePocket()
        {

        }
    }
}
