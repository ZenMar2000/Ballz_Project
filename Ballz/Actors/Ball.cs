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
                effect.Enable(value);
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
        public float TimeBeforeInfection { get; private set; }
        public float InfectionRadius { get; private set; }

        CircleEffect effect;
        InfectionCircle circle;
        public Ball(bool isInfected = false) : base("ball")
        {
            TimeBeforeInfection = 4;
            InfectionRadius = 2;
            effect = new CircleEffect(this);
            circle = new InfectionCircle(this);
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

            if (bounce == Vector2.Zero)
            {
                do
                {
                    bounce.X = RandomGenerator.GetRandomFloat(-1, 1);
                    bounce.Y = RandomGenerator.GetRandomFloat(-1, 1);
                } while (bounce == Vector2.Zero);
            }

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
            float x;
            float y;
            do
            {
                x = RandomGenerator.GetRandomFloat(-1, 1);
                y = RandomGenerator.GetRandomFloat(-1, 1);
            } while (x == y && x == 0);

            RigidBody.Velocity = new Vector2(x, y);

            RigidBody.Velocity.Normalize();
            RigidBody.Velocity *= maxSpeed;
        }

        public void InfectTheBall()
        {
            infectionTimer = 0;
            BeginInfected = false;
            ChangeSpriteColor(new Vector3(1f, 0f, 0f));
            circle.Enable(true);
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
            List<Ball> deezNuts = ((PlayScene)Game.CurrentScene).mahBalls;
            bool infectionStatus = false;

            foreach (Ball bollock in deezNuts)
            {
                Vector2 dist = bollock.Position - Position;
                if (bollock != this && dist.LengthSquared < InfectionRadius)
                {
                    if (bollock.IsInfected)
                    {   
                        //Yo that look serious. Maybe ask a doctor?
                        infectionTimer += Game.DeltaTime;
                        infectionStatus = true;
                        break;
                    }
                }
            }

            if (infectionStatus != BeginInfected)
            {
                BeginInfected = infectionStatus;
                if (!BeginInfected)
                {
                    infectionTimer = 0;
                }
            }

            if (infectionTimer >= TimeBeforeInfection)
            {
                IsInfected = true;
            }
        }
    }
}
