using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Ballz
{
    enum RigidBodyType { Player = 1, PlayerBullet = 2, Enemy = 4, EnemyBullet = 8, PowerUp = 16, Tile = 32 }

    class RigidBody
    {
        protected uint collisionMask;

        public GameObject GameObject;   // Owner
        public Collider Collider;

        public bool IsGravityAffected;
        public bool IsCollisionAffected = true;
        public float Friction;
        public bool IsActive { get { return GameObject.IsActive; } }

        public Vector2 Velocity;

        public Vector2 Position { get { return GameObject.Position; } }

        public RigidBodyType Type;

        public RigidBody(GameObject owner)
        {
            GameObject = owner;
            PhysicsMngr.AddItem(this);
        }

        public void Update()
        {
            if (IsGravityAffected)
            {
                Velocity.Y += PhysicsMngr.G * Game.DeltaTime;
            }

            ApplyFriction();

            GameObject.Position += Velocity * Game.DeltaTime;
        }

        protected void ApplyFriction()
        {
            if(Friction > 0 && Velocity != Vector2.Zero)
            {
                float fAmount = Friction * Game.DeltaTime;//velocity to subtract
                float newVelocityLength = Velocity.Length - fAmount;

                if(newVelocityLength < 0)
                {//stop
                    Velocity = Vector2.Zero;
                    return;
                }

                Velocity = Velocity.Normalized() * newVelocityLength;
            }
        }

        public bool Collides(RigidBody other, ref Collision collisionInfo)
        {
            return Collider.Collides(other.Collider, ref collisionInfo);
        }

        public void AddCollisionType(RigidBodyType type)
        {
            collisionMask |= (uint)type;//collisionMask = collisionMask | (uint)type
        }

        public void AddCollisionType(uint type)
        {
            collisionMask |= type;//collisionMask = collisionMask | type
        }

        public bool CollisionTypeMatches(RigidBodyType type)
        {
            return ((uint)type & collisionMask) != 0;
        }

        public void Destroy()
        {
            GameObject = null;
            if (Collider != null)
            {
                Collider.RigidBody = null;
                Collider = null;
            }

            PhysicsMngr.RemoveItem(this);
        }
    }
}
