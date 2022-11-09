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
            GeneralPocket[] pockets = board.pocketList;
            Brush p1Brush = new SolidBrush(Color.Black);
            Brush p2Brush = new SolidBrush(Color.Gray);
            Brush r = new SolidBrush(Color.Red);
            Brush w = new SolidBrush(Color.White);
            Font f = DefaultFont;
            int pocketsPP = pockets.Length / 2;
            allPockets = new Rectangle[pockets.Length];
            
            //Draw homePocket p2
            int x = 25;
            int y = 75;
            gr.FillRectangle(p2Brush, makeHomePocket(x,y));
            gr.DrawString("0", f, r, x, y);
            gr.DrawString(board.pocketList[0].AmountOfStones().ToString(), f, w, x+20, y+45);
            allPockets[0] = makeHomePocket(x, y);
            
            //Draw homePocket p1
            x = 75 * (pocketsPP) + 25;
            gr.FillRectangle(p1Brush, makeHomePocket(x, y));
            gr.DrawString((pocketsPP).ToString(), f, r, x, y);
            gr.DrawString(board.pocketList[pocketsPP].AmountOfStones().ToString(), f, w, x + 20, y + 45);
            allPockets[pocketsPP] = makeHomePocket(x, y);



            //P2
            for (int i = 1; i < pocketsPP; i++)
            {
                x = 25 + 75 * i;
                y = 50;
                gr.FillRectangle(p2Brush, makePocket(x,y));
                gr.DrawString(i.ToString(),f, r, x, y);
                string stonesInPocket = board.pocketList[i].AmountOfStones().ToString();
                gr.DrawString(stonesInPocket, f, w, x + 20, y + 20);
                allPockets[i] = makePocket(x, y);
            }

            //P1
            for(int i = pocketsPP + 1; i < pockets.Length; i++)
            {
                string stonesInPocket = board.pocketList[i].AmountOfStones().ToString();
                int drawPos = pockets.Length - i;
                x = 25 + 75 * drawPos;
                y = 150;
                gr.FillRectangle(p1Brush, makePocket(x, y));
                gr.DrawString(i.ToString(), f, r, x, y);
                gr.DrawString(stonesInPocket, f, w, x + 20, y + 20);
                allPockets[i] = makePocket(x, y);
            }

        }

        private Rectangle makePocket(int x, int y)
        {
            Point l = new Point(x,y);
            Size s = new Size(50, 50);
            return new Rectangle(l, s);
        }
        private Rectangle makeHomePocket(int x, int y)
        {
            Point l = new Point(x, y);
            Size s = new Size(50, 100);
            return new Rectangle(l, s);
        }
    }
}
