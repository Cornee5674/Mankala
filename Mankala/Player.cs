using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal class Player
    {
        public string playerName;
        private int playerIndex;

        public Player(string name)
        {
            playerName = name;
        }

        public static List<HomePocket> getAllHomepockets(Board b)
        {
            GeneralPocket[] pockets = b.pocketList;
            List<HomePocket> res = new List<HomePocket>();
            res.Add((HomePocket)pockets[0]);
            int i = pockets.Length / 2;
            res.Add((HomePocket)pockets[i]);
            return res;
        }
    }
}
