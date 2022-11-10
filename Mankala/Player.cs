using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal class Player
    {
        private readonly string playerName;
        public Player(string name)
        {
            playerName = name;
        }
        public string Name
        {
            get { return playerName; }
        }
    }
}
