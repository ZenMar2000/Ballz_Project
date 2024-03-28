using OpenTK;

namespace Ballz
{
    class CircleEffect : Actor
    {
        private float scale;
        Ball parent;
        public CircleEffect(Ball parent) : base("backgroundBall")
        {
            this.parent = parent;
            scale = (float)parent.TimeBeforeInfection * 0.5f + 1;
            IsActive = false;
            sprite.scale = new Vector2(scale, scale);
            frameW = frameH;
            Layer = DrawLayer.Middleground;
            UpdateMngr.AddItem(this);
            DrawMngr.AddItem(this);
        }

        public override void Update()
        {
            if (IsActive)
            {
                scale -= Game.DeltaTime * 0.5f;
                sprite.scale = new Vector2(scale, scale);
            }
            Position = parent.Position;
        }

        public void Enable(bool enabled)
        {
            IsActive = enabled;
            if (enabled == false)
            {
                scale = (float)parent.TimeBeforeInfection * 0.5f + 1;
                sprite.scale = new Vector2(scale, scale);
            }
        }
    }
}
