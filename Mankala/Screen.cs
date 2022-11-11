using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mankala
{
    internal partial class Screen : Panel
    {
        public Rectangle[] allPockets;
        public Screen()
        {
            this.ClientSize = new Size(800, 800);
            this.Text = "Games";
            
        }

        public void DrawBoard(Graphics gr, Board board)
        {
            Brush p1Brush = new SolidBrush(Color.Black);
            Brush p2Brush = new SolidBrush(Color.Gray);
            Brush r = new SolidBrush(Color.Red);
            Brush w = new SolidBrush(Color.White);
            Font f = DefaultFont;
            int pocketsPP = board.ListLength / 2;
            allPockets = new Rectangle[board.ListLength];
            
            //Draw homePocket p2
            int x = 25;
            int y = 75;
            gr.FillRectangle(p2Brush, MakeHomePocket(x,y));
            gr.DrawString("0", f, r, x, y);
            gr.DrawString(board.HomepocketP2.AmountofStones.ToString(), f, w, x+20, y+45);
            allPockets[0] = MakeHomePocket(x, y);
            
            //Draw homePocket p1
            x = 75 * (pocketsPP) + 25;
            gr.FillRectangle(p1Brush, MakeHomePocket(x, y));
            gr.DrawString((pocketsPP).ToString(), f, r, x, y);
            gr.DrawString(board.HomepocketP1.AmountofStones.ToString(), f, w, x + 20, y + 45);
            allPockets[pocketsPP] = MakeHomePocket(x, y);
            //normal pockets for p2
            for (int i = 1; i < pocketsPP; i++)
            {
                x = 25 + 75 * i;
                y = 50;
                gr.FillRectangle(p2Brush, MakePocket(x,y));
                gr.DrawString(i.ToString(),f, r, x, y);
                string stonesInPocket = board.GetAtIndex(i).AmountofStones.ToString();
                gr.DrawString(stonesInPocket, f, w, x + 20, y + 20);
                allPockets[i] = MakePocket(x, y);
            }

            //Normal pockets for p1
            for(int i = pocketsPP + 1; i < board.ListLength; i++)
            {
                string stonesInPocket = board.GetAtIndex(i).AmountofStones.ToString();
                int drawPos = board.ListLength - i;
                x = 25 + 75 * drawPos;
                y = 150;
                gr.FillRectangle(p1Brush, MakePocket(x, y));
                gr.DrawString(i.ToString(), f, r, x, y);
                gr.DrawString(stonesInPocket, f, w, x + 20, y + 20);
                allPockets[i] = MakePocket(x, y);
            }

        }

        private Rectangle MakePocket(int x, int y)
        {
            //Creates a normal Pockets which always have the same size
            Point l = new Point(x,y);
            Size s = new Size(50, 50);
            return new Rectangle(l, s);
        }
        private Rectangle MakeHomePocket(int x, int y)
        {
            //Creates a home Pockets which always have the same size
            Point l = new Point(x, y);
            Size s = new Size(50, 100);
            return new Rectangle(l, s);
        }
    }
}
