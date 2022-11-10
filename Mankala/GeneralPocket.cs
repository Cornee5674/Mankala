using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal abstract class GeneralPocket
    {
        protected int amountOfStones;
        protected int index;
        protected Player owner;
        public abstract bool UpdatePocket(Player player);
        public virtual int EmptyPocket()
        {
            //Empties the pocket and returns the amount of stones that were in there
            int stonesInThisPocket = amountOfStones;
            amountOfStones = 0;
            return stonesInThisPocket;
        }
        public void AddStones(int a)
        {
            //Adds Stones to this pocket
            amountOfStones += a;
        }
        public int Index
        {
            get { return index; }
        }
        public int AmountofStones{
            get { return amountOfStones; }
        }
        public bool IsOwner(Player p)
        {
            return p == owner;
        }


    }
}
