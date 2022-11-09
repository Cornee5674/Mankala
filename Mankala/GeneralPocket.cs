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
        public bool IsEmpty()
        {
            //Checks if the pocket is empty
            return amountOfStones == 0;
        }

        public virtual Player GetOwner()
        {
            return owner;
        }
        public virtual int GetAmountOfStones()
        {
            return amountOfStones;
        }
        public virtual int GetIndex()
        {
            return index;
        }


    }
}
