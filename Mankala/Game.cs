using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mankala
{
    internal class Game : Form
    {
        Player player1;
        Player player2;
        Player currentPlayer;
        Board board;
        RuleSet ruleset;
        Screen screen;
        Label playerAtTurnLabel;
        Button nextForcedTurn;
        int nextForcedMove = -1;

        public Game()
        {
            player1 = new Player("Player 1");
            player2 = new Player("Player 2");
            currentPlayer = player1;
            ruleset = new MankalaRuleset();
            board = ruleset.MakeBoard(board, player1,player2);
            Size s = CalcScreenSize();
            this.Size = s;
            screen = new Screen();
            screen.Paint += ReDraw;
            screen.MouseClick += mouseClick;

            playerAtTurnLabel = new Label();
            playerAtTurnLabel.Location = new Point(s.Width / 2, 10);
            playerAtTurnLabel.Text = player1.playerName + " is at Turn";
            screen.Controls.Add(playerAtTurnLabel);

            nextForcedTurn = new Button();
            nextForcedTurn.Location = new Point(s.Width / 2 - 100, 10);
            nextForcedTurn.Text = "Next Forced";
            nextForcedTurn.Visible = false;
            nextForcedTurn.Click += forcedTurn;
            screen.Controls.Add(nextForcedTurn);

            this.DoubleBuffered = true;
            Controls.Add(screen);
        }
        private Size CalcScreenSize()
        {
            return new Size(75 * (board.pocketList.Length/2) + 125, 250);
        }
        private void ReDraw(object obj, PaintEventArgs pea)
        {
            Graphics gr = pea.Graphics;
            screen.DrawBoard(gr, board);
        }
        private void mouseClick(object obj, MouseEventArgs mea)
        {
            int mX = mea.X;
            int mY = mea.Y;
            int index = indexFromClick(mX, mY);
            if (index < 0) //A negative numbers means no square was clicked
                return;
            WantToDoTurn(index,true);
        }

        private int indexFromClick(int x, int y)
        {
            Rectangle[] rects = screen.allPockets;
            Point p = new Point(x, y);
            for (int i = 0; i < rects.Length; i++)
            {
                if (rects[i].Contains(p))
                    return i;
            }
            return -1;
        }

        public void WantToDoTurn(int index, bool isFirst)
        {
            Move move;
            int chosenPocket = index;
            int halfOfPockets = board.pocketList.Length / 2;

            if (nextForcedMove >= 0 && nextForcedMove != index)
            {
                MessageBox.Show("You have a forced move");
                return;
            }

            if (isFirst)
            {
                //Checks if the move is illegal, returns if its the case
                if (board.pocketList[chosenPocket].IsEmpty())
                {
                    MessageBox.Show("Illegal move: " + chosenPocket + " is empty");
                    return;
                }
                if (index<halfOfPockets && currentPlayer == player2)
                {
                    MessageBox.Show("Illegal move: " + chosenPocket + " is not your pocket");
                    return;
                }
                if(index >= halfOfPockets && currentPlayer == player1)
                {
                    MessageBox.Show("Illegal move: " + chosenPocket + " is not your pocket");
                    return;
                }
                if (board.pocketList[chosenPocket] is HomePocket)
                {
                    MessageBox.Show("Illegal move: " + chosenPocket + " is a Homepocket");
                    return;
                }
            }
            
            MessageBox.Show("You clicked: " + chosenPocket);
            
            move = board.DoTurn(currentPlayer, chosenPocket);

            //Checks if the next move will be a forced one
            if (ruleset.IsForcedTurn(move, board))
            {
                nextForcedMove = move.endingPocket.GetIndex();
                nextForcedTurn.Visible = true;
            }
            //Checks if the other player has his turn now
            else if (!ruleset.SamePlayerAtTurn(move, board))
            {
                currentPlayer = PlayerChange(currentPlayer);
                nextForcedMove = -1;
            }
            else
                nextForcedMove = -1;
            screen.Invalidate();
        }
        private void forcedTurn(object obj, EventArgs ea)
        {
            nextForcedTurn.Visible = false;
            WantToDoTurn(nextForcedMove, false);
        }
        private Player PlayerChange(Player current)
        {
            if (current == player1)
            {
                playerAtTurnLabel.Text = player2.playerName + " is at Turn";
                return player2;

            }
            playerAtTurnLabel.Text = player1.playerName + " is at Turn";
            return player1;
        }
    }
}
