using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Aiv.Fast2D;
using OpenTK;
using OpenTK.Input;

namespace Ballz
{
    class KeyboardController : Controller
    {
 

        public KeyboardController(int ctrlIndex) : base(ctrlIndex)
        {

        }

        public override float GetHorizontal()
        {
            //float direction = 0.0f;

            //if(Game.Window.GetKey(KeyCode.D))
            //{
            //    direction = 1;
            //}
            //else if(Game.Window.GetKey(KeyCode.A))
            //{
            //    direction = -1;
            //}

            //return direction;
            return 0;
        }

        public override float GetVertical()
        {
            //float direction = 0.0f;

            //if (Game.Window.GetKey(KeyCode.W))
            //{
            //    direction = -1;
            //}
            //else if (Game.Window.GetKey(KeyCode.S))
            //{
            //    direction = 1;
            //}

            //return direction;
            return 0;
        }

        public override bool IsClearWindowKeyPressed()
        {
            return Game.Window.GetKey(KeyCode.Space);
        }

        public override bool IsRestartKeyPressed()
        {
            return Game.Window.GetKey(KeyCode.R);
        }

        public override bool SpawnGreenBallKeyPressed()
        {
            return false;
        }

        public override bool SpawnRedBallKeyPressed()
        {
            return false;
        }

        public bool RedBoost()
        {
            return Game.Window.GetKey(KeyCode.G);
        }

        public bool GreenBoost()
        {
            return Game.Window.GetKey(KeyCode.F);
        }

        public bool RedChaseMode()
        {
            return Game.Window.GetKey(KeyCode.T);
        }

    }
}
