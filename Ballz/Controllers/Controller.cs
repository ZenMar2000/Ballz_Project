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

        public abstract bool IsClearWindowKeyPressed();
        public abstract bool IsRestartKeyPressed();
        public abstract bool SpawnGreenBallKeyPressed();
        public abstract bool SpawnRedBallKeyPressed();


        public abstract float GetHorizontal();
        public abstract float GetVertical();

    }
}
