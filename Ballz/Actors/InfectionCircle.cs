using OpenTK;
using System;

namespace Ballz
{
    class InfectionCircle : Actor
    {
        Ball parent;
        float scale;
        public InfectionCircle(Ball parent) : base("circle")
        {
            this.parent = parent;
            IsActive = false;
            frameW = frameH;

            scale = 0.33f + (0.10f * (parent.InfectionRadius - 2f));
            sprite.scale = new Vector2(scale, scale);

            Layer = DrawLayer.Background;
            UpdateMngr.AddItem(this);
            DrawMngr.AddItem(this);
        }

        public override void Update()
        {
            Position = parent.Position;
        }

        public void Enable(bool enabled)
        {
            IsActive = enabled;
        }


    }
}
