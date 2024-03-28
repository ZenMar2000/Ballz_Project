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
            IsActive = false;
            frameW = frameH;

            Layer = DrawLayer.Middleground;

            ResetScale();

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
                ResetScale();
            }
        }

        private void ResetScale()
        {
            scale = (float)parent.TimeBeforeInfection * 0.5f + 1;
            sprite.scale = new Vector2(scale, scale);
        }
    }
}
