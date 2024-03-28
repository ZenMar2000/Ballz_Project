using OpenTK;
using System;
using System.Collections.Generic;

namespace Ballz
{
    class Ball : Actor
    {
        private bool _beginInfection = false;
        public bool BeginInfected
        {
            get
            {
                return _beginInfection;
            }
            private set
            {
                _beginInfection = value;
                if (value == true)
                {
                    ChangeSpriteColor(new Vector3(1, 1, 0));
                }
                else
                {
                    ChangeSpriteColor(new Vector3(0, 1, 0));

                }
            }
        }
        private bool _isInfected;
        public bool IsInfected
        {
            get
            {
                return _isInfected;
            }
            private set
            {
                _isInfected = value;
                if (value)
                {
                    InfectTheBall();
                }
            }
        }

        protected float infectionTimer = 0;
        protected float timeBeforeInfection = 4;
        protected float infectionRadius = 2;

        public Ball(bool isInfected = false) : base("ball")
        {
            IsActive = true;
            if (isInfected)
            {
                IsInfected = isInfected;
            }
            else
            {
                ChangeSpriteColor(new Vector3(0f, 1f, 0f));
            }
            maxSpeed = 1f;

            frameW = frameH;

            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType(RigidBodyType.Player);
            RigidBody.Friction = 0f;

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
            infectionTimer = 0;
            BeginInfected = false;
            ChangeSpriteColor(new Vector3(1f, 0f, 0f));
        }

        public override void Update()
        {
            if (IsActive && RigidBody.Velocity != Vector2.Zero)
            {
                Forward = RigidBody.Velocity;
            }
            CheckWindowBorders();

            if (!IsInfected)
            {
                InfectFromOthers();
            }

            base.Update();
        }

        private void CheckWindowBorders()
        {
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

        private void InfectFromOthers()
        {
            List<Ball> balls = ((PlayScene)Game.CurrentScene).myBalls;
            bool infectionStatus = false;

            foreach (Ball ball in balls)
            {
                Vector2 dist = ball.Position - Position;
                if (ball != this && dist.LengthSquared < infectionRadius)
                {
                    if (ball.IsInfected)
                    {

                        infectionTimer += Game.DeltaTime;
                        infectionStatus = true;
                        break;
                    }
                }
            }

            if (infectionStatus != BeginInfected)
            {
                BeginInfected = infectionStatus;
            }

            if (infectionTimer >= timeBeforeInfection)
            {
                IsInfected = true;
            }
        }
    }
}
