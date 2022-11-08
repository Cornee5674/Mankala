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
            int temp = amountOfStones;
            amountOfStones = 0;
            return temp;
        }
        public void AddStones(int a)
        {
            amountOfStones += a;
        }

        public virtual Player GetOwner()
        {
            return owner;
        }
        public virtual int AmountOfStones()
        {
            return amountOfStones;
        }
        public virtual int GetIndex()
        {
            return index;
        }
        public bool IsEmpty()
        {
            return amountOfStones == 0;
        }


    }
}
