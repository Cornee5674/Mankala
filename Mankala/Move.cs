using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal class Move
    {
        public Player currentPlayer;
        public GeneralPocket endingPocket;
        public Move(Player currentPlayer, GeneralPocket endingPocket)
        {
            this.currentPlayer = currentPlayer;
            this.endingPocket = endingPocket;
        }
    }
}
