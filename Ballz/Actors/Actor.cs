using Microsoft.Win32;
using OpenTK;
using System.Collections.Generic;

namespace Ballz
{
    abstract class Actor : GameObject
    {
        public bool IsAlive = true;
        //public virtual int Energy { get => energy; set { energy = MathHelper.Clamp(value, 0, maxEnergy); } }

        public Actor(string texturePath, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, DrawLayer.Playground, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            RigidBody = new RigidBody(this);
        }

        public virtual void OnDie()
        {
            IsActive = false;
        }


        public virtual void Reset()
        {
            IsAlive = true;
        }

        //public virtual void Update()
        //{

        //}
    }
}
