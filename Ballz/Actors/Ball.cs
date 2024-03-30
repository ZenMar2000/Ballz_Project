using Aiv.Audio;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Ballz
{
    class Ball : Actor
    {
        private double turnTimer = 0;
        private AudioClip popClip = GfxMngr.GetClip("pop");
        private AudioClip thokClip = GfxMngr.GetClip("thok");
        private bool _beingInfected = false;
        private float turnRatio = 0.0075f;

        protected float infectionTimer = 0;

        public bool BeingInfected
        {
            get
            {
                return _beingInfected;
            }
            private set
            {
                effect.Enable(value);
                _beingInfected = value;
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
            else if (soundTimer <= 0)
            {
                Game.Source.Pitch = RandomGenerator.GetRandomInt(8, 14) * 0.1f;
                Game.Source.Play(thokClip);
                collisionInfo.Collider.soundTimer = 0.75f;
            }
            else
            {
                soundTimer = 0.75f;
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
            BeingInfected = false;
            ChangeSpriteColor(new Vector3(1f, 0f, 0f));
            circle.Enable(true);

            Game.Source.Pitch = RandomGenerator.GetRandomInt(9, 12) * 0.1f;
            Game.Source.Play(popClip);
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

            if (((PlayScene)Game.CurrentScene).RedChase == true && IsInfected)
            {
                ChaseLogic();
            }

            if (soundTimer >= 0)
            {
                soundTimer -= Game.DeltaTime;
            }
        }

        private void CheckWindowBorders()
        {
            bool playSound = false;
            //X Border
            if (Position.X + sprite.Width * 0.5f > Game.Window.OrthoWidth)
            {
                RigidBody.Velocity.X *= -1;
                X = Game.Window.OrthoWidth - sprite.Width * 0.5f;

                playSound = true;
            }
            else if (Position.X - sprite.Width * 0.5f <= 0)
            {
                RigidBody.Velocity.X *= -1;
                X = sprite.Width * 0.5f;
                playSound = true;
            }

            //Y border
            if (Position.Y + sprite.Height * 0.5f > Game.Window.OrthoHeight)
            {
                RigidBody.Velocity.Y *= -1;
                Y = Game.Window.OrthoHeight - sprite.Height * 0.5f;
                playSound = true;
            }
            else if (Position.Y - sprite.Height * 0.5f <= 0)
            {
                RigidBody.Velocity.Y *= -1;
                Y = sprite.Height * 0.5f;
                playSound = true;
            }

            if (playSound)
            {
                Game.Source.Pitch = RandomGenerator.GetRandomInt(4, 7) * 0.1f;
                Game.Source.Play(thokClip);
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

            if (infectionStatus != BeingInfected)
            {
                BeingInfected = infectionStatus;
                if (!BeingInfected)
                {
                    infectionTimer = 0;
                }
            }

            if (infectionTimer >= TimeBeforeInfection)
            {
                IsInfected = true;
            }
        }

        private void ChaseLogic()
        {
            Ball chaseThis = null;
            List<Ball> ballz = ((PlayScene)Game.CurrentScene).mahBalls;
            List<FuzzyLogic> fuzzyLogic = new List<FuzzyLogic>();

            for (int i = 1; i < ballz.Count; i++)
            {
                double distCheck = (ballz[i].Position - Position).LengthSquared;
                if (distCheck <= InfectionRadius && !ballz[i].IsInfected)
                {
                    fuzzyLogic.Add(new FuzzyLogic(this, ballz[i], i));
                }
            }

            if (fuzzyLogic.Count > 0)
            {
                chaseThis = ballz[fuzzyLogic[0].index];
                double maxValue = fuzzyLogic[0].result;
                for (int i = 1; i < fuzzyLogic.Count; i++)
                {
                    if (fuzzyLogic[i].result < maxValue)
                    {
                        maxValue = fuzzyLogic[i].result;
                        chaseThis = ballz[fuzzyLogic[i].index];
                    }
                }
            }
            if (chaseThis != null)
            {
                RotateTowards(chaseThis.Position);
            }
        }

        public void RotateTowards(Vector2 target)
        {
            // Get the angle between the current vector and the target vector
            if (turnTimer <= 0)
            {
                Vector2 newDir;
                Vector2 currentNormalizedDir = RigidBody.Velocity.Normalized();
                Vector2 targetDir = target - Position;
                if (targetDir.X == float.NaN)
                {
                    return;
                }
                targetDir.Normalize();
                targetDir *= maxSpeed;

                newDir = Vector2.Lerp(RigidBody.Velocity, targetDir, turnRatio);

                RigidBody.Velocity = newDir.Normalized() * maxSpeed;
                //double currentAngle = Math.Acos(currentNormalizedDir.X) * 180 / 3.14;
                //newAngle = currentAngle;
                //double targetAngle = Math.Acos(targetDir.X) * 180 / 3.14;

                //if (currentAngle > 180)
                //    currentAngle -= 360;

                //if (targetAngle > 180)
                //    targetAngle -= 360;

                //if (currentAngle < targetAngle - 5)
                //{
                //    newAngle -= turnRatio;
                //    newDir = new Vector2((float)Math.Cos(newAngle), (float)Math.Sin(newAngle));
                //    RigidBody.Velocity = newDir;

                //}
                //else if (currentAngle > targetAngle + 5)
                //{
                //    newAngle += turnRatio;
                //    newDir = new Vector2((float)Math.Cos(newAngle), (float)Math.Sin(newAngle));
                //    RigidBody.Velocity = newDir;

                //}
            }
            else
            {
                turnTimer -= Game.DeltaTime;
            }


            // Magie strane della matematica


        }

        public float GetInfectionTimer()
        {
            return infectionTimer;
        }
    }

    struct FuzzyLogic
    {
        private double distanceFL;
        private double infectionTimerFL;
        private double directionAffinityFL;

        public int index;
        public double result;
        public FuzzyLogic(Ball currentBall, Ball targetBall, int index)
        {
            Vector2 dist = targetBall.Position - currentBall.Position;

            distanceFL = 1-(dist.LengthSquared / currentBall.InfectionRadius);
            infectionTimerFL = targetBall.GetInfectionTimer();
            directionAffinityFL = (Math.Cos(currentBall.RigidBody.Velocity.X) - Math.Cos(targetBall.RigidBody.Velocity.X)) - 1;

            result = distanceFL + infectionTimerFL + directionAffinityFL;
            this.index = index;
        }

        //public FuzzyLogic(double distance, double infectionTimer, double directionAffinity)
        //{
        //    distanceFL = distance;
        //    infectionTimerFL = infectionTimer;
        //    directionAffinityFL = directionAffinity;

        //    result = distanceFL + infectionTimerFL + directionAffinityFL;
        //}
    }
}
