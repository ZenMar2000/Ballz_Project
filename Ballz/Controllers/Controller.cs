using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballz
{
    abstract class Controller
    {
        protected int index;

        public Controller(int ctrlIndex)
        {
            index = ctrlIndex;
        }

        public abstract bool IsFirePressed();
        public abstract bool IsJumpPressed();

        public abstract float GetHorizontal();
        public abstract float GetVertical();
    }
}
