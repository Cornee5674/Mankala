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
        private NumericUpDown amountOfPocketsField;
        private Label label2;
        private Label label3;
        private NumericUpDown amountOfStonesField;
        private Label label4;
        private Label label5;
        private TextBox player1Name;
        private TextBox player2Name;
        private RadioButton MankalaButton;
        private RadioButton wakiButton;
        private RadioButton customButton;
        private Button initConfirmButton;
        private Label label1;
        int nextForcedMove = -1;

        public Game()
        {
            DrawStartScreen();
        }
        
        private void InitValuesConfirm(object sender, EventArgs e)
        {
            int stonesPP = Decimal.ToInt32(amountOfStonesField.Value);
            int pocketsPP = Decimal.ToInt32(amountOfPocketsField.Value);
            if (stonesPP == 0 || pocketsPP == 0)
            {
                MessageBox.Show("Please make a complete choice first");
                return;
            }
            if (player1Name.Text == "" || player2Name.Text == "")
            {
                MessageBox.Show("Please make a complete choice first");
                return;
            }

            if (MankalaButton.Checked)
                ruleset = new MankalaRuleset(pocketsPP, stonesPP);
            else if (wakiButton.Checked)
                ruleset = new WakiRuleset(pocketsPP, stonesPP);
            else if (customButton.Checked)
                throw new NotImplementedException();
            else
            {
                MessageBox.Show("Please make a complete choice first");
                return;
            }
            player1 = new Player(player1Name.Text);
            player2 = new Player(player2Name.Text);
            this.Controls.Clear();
            DrawBoard();


        }
        private void DrawBoard()
        {
            currentPlayer = player1;
            board = ruleset.MakeBoard(board, player1, player2);
            Size s = CalcScreenSize();
            this.Size = s;
            screen = new Screen();
            screen.Paint += ReDraw;
            screen.MouseClick += MouseIsClicked;

            playerAtTurnLabel = new Label();
            playerAtTurnLabel.Location = new Point(s.Width / 2, 10);
            playerAtTurnLabel.Text = player1.playerName + " is at Turn";
            screen.Controls.Add(playerAtTurnLabel);

            nextForcedTurn = new Button();
            nextForcedTurn.Location = new Point(s.Width / 2 - 100, 10);
            nextForcedTurn.Text = "Next Forced";
            nextForcedTurn.Visible = false;
            nextForcedTurn.Click += DoForcedTurn;
            screen.Controls.Add(nextForcedTurn);

            Label p1 = new Label();
            p1.AutoSize = true;
            p1.Location = new Point(25, 190);
            p1.Text = player1.playerName;

            Label p2 = new Label();
            p2.AutoSize = true;
            p2.Location = new Point(25, 30);
            p2.Text = player2.playerName;

            screen.Controls.Add(p1);
            screen.Controls.Add(p2);

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
        private void MouseIsClicked(object obj, MouseEventArgs mea)
        {
            int mX = mea.X;
            int mY = mea.Y;
            int index = IndexFromClick(mX, mY);
            if (index < 0) //A negative numbers means no square was clicked
                return;
            WantToDoTurn(index,true);
        }

        private int IndexFromClick(int x, int y)
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
            if (ruleset.GameIsFinished(board, currentPlayer))
            {
                MessageBox.Show("The game is finished");
                return;
            }
                
            Move move;
            int chosenPocket = index;
            int halfOfPockets = board.pocketList.Length / 2;

            if (nextForcedMove >= 0 && nextForcedMove != index)
            {
                MessageBox.Show("You have a forced move");
                return;
            }
            if (index == -1)
                throw new Exception("Something happened");

            if (isFirst)
            {
                //Checks if the move is illegal, returns if its the case
                if (board.pocketList[chosenPocket].IsEmpty())
                {
                    MessageBox.Show("Illegal move: " + chosenPocket + " is empty");
                    return;
                }
                if (index<halfOfPockets && currentPlayer == player1)
                {
                    MessageBox.Show("Illegal move: " + chosenPocket + " is not your pocket");
                    return;
                }
                if(index >= halfOfPockets && currentPlayer == player2)
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
            else if (ruleset.SamePlayerAtTurn(move, board))
            {
                currentPlayer = PlayerChange(currentPlayer);
                nextForcedMove = -1;
            }
            else
                nextForcedMove = -1;
            screen.Invalidate();
        }
        private void DoForcedTurn(object obj, EventArgs ea)
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

        private void DrawStartScreen()
        {
            this.label1 = new Label();
            this.amountOfPocketsField = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.amountOfStonesField = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.player1Name = new System.Windows.Forms.TextBox();
            this.player2Name = new System.Windows.Forms.TextBox();
            this.MankalaButton = new System.Windows.Forms.RadioButton();
            this.wakiButton = new System.Windows.Forms.RadioButton();
            this.customButton = new System.Windows.Forms.RadioButton();
            this.initConfirmButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.amountOfPocketsField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.amountOfStonesField)).BeginInit();
            this.SuspendLayout();
            // 
            // amountOfPocketsField
            // 
            this.amountOfPocketsField.Location = new System.Drawing.Point(224, 67);
            this.amountOfPocketsField.Name = "amountOfPocketsField";
            this.amountOfPocketsField.Size = new System.Drawing.Size(120, 22);
            this.amountOfPocketsField.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(187, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Amout of Pockets per Player";
            //
            //Label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new Point(100, 30);
            this.label1.Name = "label1";
            this.label1.Text = "How do you want to play?";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Amount of Stones per Pocket";
            // 
            // amountOfStonesField
            // 
            this.amountOfStonesField.Location = new System.Drawing.Point(224, 92);
            this.amountOfStonesField.Name = "amountOfStonesField";
            this.amountOfStonesField.Size = new System.Drawing.Size(120, 22);
            this.amountOfStonesField.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Name player 1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "Name player 2";
            // 
            // textBox1
            // 
            this.player1Name.Location = new System.Drawing.Point(224, 136);
            this.player1Name.Name = "player1Name";
            this.player1Name.Size = new System.Drawing.Size(100, 22);
            this.player1Name.TabIndex = 6;
            // 
            // textBox2
            // 
            this.player2Name.Location = new System.Drawing.Point(224, 164);
            this.player2Name.Name = "player2Name";
            this.player2Name.Size = new System.Drawing.Size(100, 22);
            this.player2Name.TabIndex = 7;
            // 
            // radioButton1
            // 
            this.MankalaButton.AutoSize = true;
            this.MankalaButton.Location = new System.Drawing.Point(10, 236);
            this.MankalaButton.Name = "MankalaButton";
            this.MankalaButton.Size = new System.Drawing.Size(82, 21);
            this.MankalaButton.TabIndex = 8;
            this.MankalaButton.TabStop = true;
            this.MankalaButton.Text = "Mankala";
            this.MankalaButton.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.wakiButton.AutoSize = true;
            this.wakiButton.Location = new System.Drawing.Point(126, 236);
            this.wakiButton.Name = "wakiButton";
            this.wakiButton.Size = new System.Drawing.Size(60, 21);
            this.wakiButton.TabIndex = 9;
            this.wakiButton.TabStop = true;
            this.wakiButton.Text = "Waki";
            this.wakiButton.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.customButton.AutoSize = true;
            this.customButton.Location = new System.Drawing.Point(242, 236);
            this.customButton.Name = "customButton";
            this.customButton.Size = new System.Drawing.Size(76, 21);
            this.customButton.TabIndex = 10;
            this.customButton.TabStop = true;
            this.customButton.Text = "Custom";
            this.customButton.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.initConfirmButton.Location = new System.Drawing.Point(126, 281);
            this.initConfirmButton.Name = "button1";
            this.initConfirmButton.Size = new System.Drawing.Size(75, 23);
            this.initConfirmButton.TabIndex = 11;
            this.initConfirmButton.Text = "Confirm";
            this.initConfirmButton.UseVisualStyleBackColor = true;
            this.initConfirmButton.Click += InitValuesConfirm;
            // 
            // Game
            // 
            this.ClientSize = new System.Drawing.Size(365, 316);
            this.Controls.Add(this.initConfirmButton);
            this.Controls.Add(this.customButton);
            this.Controls.Add(this.wakiButton);
            this.Controls.Add(this.MankalaButton);
            this.Controls.Add(this.player2Name);
            this.Controls.Add(this.player1Name);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.amountOfStonesField);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.amountOfPocketsField);
            this.Name = "Game";
            ((System.ComponentModel.ISupportInitialize)(this.amountOfPocketsField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.amountOfStonesField)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        
    }
}
