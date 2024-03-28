using OpenTK;

namespace Ballz
{
    abstract class Actor : GameObject
    {

        //protected int energy;
        //protected int maxEnergy;

        public bool IsAlive = true;
        //public virtual int Energy { get => energy; set { energy = MathHelper.Clamp(value, 0, maxEnergy); } }

        public Actor(string texturePath, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, DrawLayer.Playground, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            RigidBody = new RigidBody(this);

            //maxEnergy = 100;

        }

        public virtual void OnDie()
        {
            IsActive = false;
        }


        public virtual void Reset()
        {
            IsAlive = true;
        }

        public override void Update()
        {
            if (IsActive && RigidBody.Velocity != Vector2.Zero)
            {
                Forward = RigidBody.Velocity;
            }

            //X Border
            if (Position.X + sprite.Width * 0.5f > Game.Window.OrthoWidth)
            {
                RigidBody.Velocity.X *= -1;
                X = Game.Window.OrthoWidth - sprite.Width * 0.5f;
            }
            else if (Position.X - sprite.Width * 0.5f <= 0)
            {
                RigidBody.Velocity.X *= -1;
                X = sprite.Width * 0.5f;
            }

            //Y border
            if (Position.Y + sprite.Height * 0.5f > Game.Window.OrthoHeight)
            {
                RigidBody.Velocity.Y *= -1;
                Y = Game.Window.OrthoHeight - sprite.Height * 0.5f;
            }
            else if (Position.Y - sprite.Height * 0.5f <= 0)
            {
                RigidBody.Velocity.Y *= -1;
                Y = sprite.Height * 0.5f;
            }
        }
    }
}
