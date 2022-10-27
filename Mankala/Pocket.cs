using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal class Pocket : GeneralPocket
    {
        public override bool UpdatePocket(Player player)
        {
            throw new NotImplementedException();
        }

        public override int EmptyPocket()
        {
            return base.EmptyPocket();
        }
    }
}
