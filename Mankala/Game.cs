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
        Player p1;
        Player p2;
        Player currentPlayer;
        Board board;
        RuleSet ruleset;
        Screen screen;
        Label playerAtTurnLabel;
        Button nextForcedTurnButton;
        int nextForcedMove = -1;

        public Game()
        {
            this.DoubleBuffered = true;
            DrawStartScreen();
        }
        private void InitValuesConfirm(object sender, EventArgs e)
        {
            //Event handler from the confirm button when selecting your rules
            //Gets all values from the fields and stores them
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

            if (mankalaButton.Checked)
                ruleset = new MankalaRuleset(pocketsPP, stonesPP);
            else if (wakiButton.Checked)
                ruleset = new WakiRuleset(pocketsPP, stonesPP);
            else if (amondiButton.Checked)
                ruleset = new AmondiRuleSet(pocketsPP,stonesPP);
            else
            {
                MessageBox.Show("Please make a complete choice first");
                return;
            }
            p1 = new Player(player1Name.Text);
            p2 = new Player(player2Name.Text);

            //Clears the current fields
            this.Controls.Clear();
            //Draws the playing board
            DrawBoard();


        }
        private void DrawBoard()
        {
            //Create board
            board = ruleset.MakeBoard(p1, p2);
            currentPlayer = p1;
            Size s = CalcScreenSize();
            this.Size = s;

            CreateScreen(s);

        }
        private void CreateScreen(Size s)
        {
            //Initiation of screen
            screen = new Screen();
            screen.Paint += ReDraw;
            screen.MouseClick += MouseIsClicked;

            playerAtTurnLabel = new Label();
            playerAtTurnLabel.Location = new Point(s.Width / 2, 10);
            playerAtTurnLabel.Text = this.p1.Name + " is at Turn";
            screen.Controls.Add(playerAtTurnLabel);

            nextForcedTurnButton = new Button();
            nextForcedTurnButton.Location = new Point(s.Width / 2 - 100, 10);
            nextForcedTurnButton.Text = "Next Forced";
            nextForcedTurnButton.Visible = false;
            nextForcedTurnButton.Click += DoForcedTurn;
            screen.Controls.Add(nextForcedTurnButton);

            Label p1 = new Label();
            p1.AutoSize = true;
            p1.Location = new Point(25, 190);
            p1.Text = this.p1.Name;

            Label p2 = new Label();
            p2.AutoSize = true;
            p2.Location = new Point(25, 30);
            p2.Text = this.p2.Name;

            screen.Controls.Add(p1);
            screen.Controls.Add(p2);
            this.Controls.Add(screen);
        }
        private Size CalcScreenSize()
        {
            //Method that calculates the size of the screen based on the size of the board
            return new Size(75 * (board.ListLength/2) + 125, 250);
        }
        private void ReDraw(object obj, PaintEventArgs pea)
        {
            //Draw the board
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
            //List of the hitboxes of all the pockets
            Rectangle[] rects = screen.allPockets;
            Point p = new Point(x, y);

            for (int i = 0; i < rects.Length; i++)
            {
                if (rects[i].Contains(p))
                    return i;
            }
            return -1; //return -1 if no square was clicked
        }
        public void WantToDoTurn(int index, bool isFirst)
        {
            

            if (ruleset.GameIsFinished(board, currentPlayer))
            {
                int stonesp2 = board.HomepocketP2.AmountofStones;
                int stonesp1 = board.HomepocketP1.AmountofStones;
                if (stonesp1 > stonesp2)
                    MessageBox.Show("The game is finished. " + p1.Name + " wins!");
                else if (stonesp1 < stonesp2)
                    MessageBox.Show("The game is finished. " + p2.Name + " wins!");
                else
                    MessageBox.Show("The game is finished. It is a draw");
                return;
            }
                
            Move move;
            int chosenPocket = index;
            int halfOfPockets = board.ListLength / 2;

            //If there is a forced move you have to play that and can't play anything else
            if (nextForcedMove >= 0 && isFirst)
            {
                MessageBox.Show("You have a forced move");
                return;
            }
            if (index == -1)
                throw new Exception("Something happened");

            //Checks if this is the first move and if we are not in some sort of forced move loop
            if (isFirst)
            {
                //Checks if the move is illegal, and returns if this is the case
                if (board.GetAtIndex(chosenPocket).AmountofStones == 0)
                {
                    MessageBox.Show("Illegal move: " + chosenPocket + " is empty");
                    return;
                }
                if (index<halfOfPockets && currentPlayer == p1)
                {
                    MessageBox.Show("Illegal move: " + chosenPocket + " is not your pocket");
                    return;
                }
                if(index >= halfOfPockets && currentPlayer == p2)
                {
                    MessageBox.Show("Illegal move: " + chosenPocket + " is not your pocket");
                    return;
                }
                if (board.GetAtIndex(chosenPocket) is HomePocket)
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
                nextForcedMove = move.endingPocket.Index;
                nextForcedTurnButton.Visible = true;
            }
            //Checks if the other player has his turn now
            else if (ruleset.NeedToSwitchPlayer(move, board))
            {
                currentPlayer = PlayerChange(currentPlayer);
                nextForcedMove = -1; // -1 means there is no forced move active right now
            }
            else
                nextForcedMove = -1;

            screen.Invalidate(); // Redraw the board
        }
        private void DoForcedTurn(object obj, EventArgs ea)
        {
            //Event handler for doForcedTurnButton
            nextForcedTurnButton.Visible = false;
            WantToDoTurn(nextForcedMove, false);
        }
        private Player PlayerChange(Player current)
        {
            //Method that changes the current player and updates the label at the top
            if (current == p1)
            {
                playerAtTurnLabel.Text = p2.Name + " is at Turn";
                return p2;
            }
            playerAtTurnLabel.Text = p1.Name + " is at Turn";
            return p1;
        }
        private void DrawStartScreen()
        {
            //Designer generated code
            this.label1 = new System.Windows.Forms.Label();
            this.amountOfPocketsField = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.amountOfStonesField = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.player1Name = new System.Windows.Forms.TextBox();
            this.player2Name = new System.Windows.Forms.TextBox();
            this.mankalaButton = new System.Windows.Forms.RadioButton();
            this.wakiButton = new System.Windows.Forms.RadioButton();
            this.amondiButton = new System.Windows.Forms.RadioButton();
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
            this.amountOfPocketsField.Value = 6;
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
            this.amountOfStonesField.Value = 4;
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
            this.mankalaButton.AutoSize = true;
            this.mankalaButton.Location = new System.Drawing.Point(10, 236);
            this.mankalaButton.Name = "MankalaButton";
            this.mankalaButton.Size = new System.Drawing.Size(82, 21);
            this.mankalaButton.TabIndex = 8;
            this.mankalaButton.TabStop = true;
            this.mankalaButton.Text = "Mankala";
            this.mankalaButton.UseVisualStyleBackColor = true;
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
            this.amondiButton.AutoSize = true;
            this.amondiButton.Location = new System.Drawing.Point(242, 236);
            this.amondiButton.Name = "amondiButton";
            this.amondiButton.Size = new System.Drawing.Size(76, 21);
            this.amondiButton.TabIndex = 10;
            this.amondiButton.TabStop = true;
            this.amondiButton.Text = "Amondi";
            this.amondiButton.UseVisualStyleBackColor = true;
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
            this.Controls.Add(this.amondiButton);
            this.Controls.Add(this.wakiButton);
            this.Controls.Add(this.mankalaButton);
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
        //All GUI components
        private NumericUpDown amountOfPocketsField;
        private Label label2;
        private Label label3;
        private NumericUpDown amountOfStonesField;
        private Label label4;
        private Label label5;
        private TextBox player1Name;
        private TextBox player2Name;
        private RadioButton mankalaButton;
        private RadioButton wakiButton;
        private RadioButton amondiButton;
        private Button initConfirmButton;
        private Label label1;
    }
}
