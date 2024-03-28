using OpenTK;
using System;
using System.Linq.Expressions;

namespace Ballz
{
    class Ball : Actor
    {

        public Ball() : base("ball")
        {
            IsActive = true;
            IsInfected = false;
            maxSpeed = 1f;

            frameW = frameH;

            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType(RigidBodyType.Player);
            RigidBody.Friction = 0f;

            ChangeSpriteColor(new Vector3(0f, 1f, 0f));
            SetRandomDirection();

            Reset();

            UpdateMngr.AddItem(this);
            DrawMngr.AddItem(this);
        }

        public override void OnCollide(Collision collisionInfo)
        {
            Vector2 bounce = RigidBody.Position - collisionInfo.Collider.Position;
            bounce.Normalize();
            bounce *= 0.15f;

            Vector2 otherBounce = bounce * -1;
            otherBounce.Normalize();
            otherBounce *= 0.15f;

            RigidBody.Velocity += bounce;
            RigidBody.Velocity.Normalize();
            RigidBody.Velocity *= maxSpeed;

            collisionInfo.Collider.RigidBody.Velocity += otherBounce;
            collisionInfo.Collider.RigidBody.Velocity.Normalize();
            collisionInfo.Collider.RigidBody.Velocity *= maxSpeed;
        }

        public override void OnDie()
        {
            Console.WriteLine("Ball is dead");
        }

        public void ChangeSpriteColor(Vector3 newCol)
        {
            sprite.SetAdditiveTint(new Vector4(newCol, 0.0f));
        }

        private void SetRandomDirection()
        {
            RigidBody.Velocity = new Vector2(RandomGenerator.GetRandomFloat(-1, 1), RandomGenerator.GetRandomFloat(-1, 1));

            RigidBody.Velocity.Normalize();
            RigidBody.Velocity *= maxSpeed;
        }

        public void InfectTheBall()
        {
            IsInfected = true;
            ChangeSpriteColor(new Vector3(1f,0f,0f));
        }
    }
}
