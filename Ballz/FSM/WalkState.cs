using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballz
{
    class WalkState : State
    {
        private Ball owner;

        public WalkState(Ball owner)
        {
            this.owner = owner;
        }

        public override void OnEnter()
        {
            // Change Sprite Color
            owner.ChangeSpriteColor(new OpenTK.Vector3(0.0f, 0.0f, 0.0f));
        }

        public override void Update()
        {
            //if(owner.CanSeePlayer())
            //{
            //    fsm.GoTo(StateEnum.FOLLOW);
            //}
            //else
            //{
            //    // Walk
            //    owner.HeadToPoint();
            //}
        }
    }
}
