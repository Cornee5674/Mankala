using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal abstract class GeneralPocket
    {
        int amountOfStones;
        Player player;

        public abstract bool UpdatePocket(Player player);

        public virtual int EmptyPocket()
        {
            return 0;
        }
    }
}
