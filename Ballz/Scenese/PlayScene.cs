using Aiv.Audio;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ballz
{
    class PlayScene : Scene
    {
        protected int maxBalls = 25;//25

        private AudioClip clokClip;
        public bool RedChase = false;

        protected bool clickedL = false;
        protected bool clickedR = false;
        protected bool pressedG = false;
        protected bool pressedT = false;
        protected bool pressedF = false;
        protected bool pressedSpace = false;

        protected KeyboardController keyboardCtrl;

        public List<Ball> mahBalls { get; protected set; }

        public float GroundY { get; protected set; }

        public PlayScene(KeyboardController ctrl) : base()
        {
            keyboardCtrl = ctrl;

        }

        public override void Start()
        {

            LoadAssets();
            mahBalls = new List<Ball>();
            List<Vector2> spawnPos = new List<Vector2>();


            CameraLimits cameraLimits = new CameraLimits(Game.Window.OrthoWidth * 0.8f, Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f, 0);
            CameraMgr.Init(null, cameraLimits);


            for (int i = 0; i < maxBalls; i++)
            {

                int rep = 0;
                Vector2 spawn;
                bool validPosition = true;

                do
                {
                    int randX = RandomGenerator.GetRandomInt(0, (int)Game.Window.OrthoWidth);
                    int randY = RandomGenerator.GetRandomInt(0, (int)Game.Window.OrthoHeight);

                    //int randX = i + 1;
                    //int randY = 2;
                    spawn = new Vector2(randX, randY);
                    for (int x = 0; x < spawnPos.Count(); x++)
                    {
                        if (spawnPos[x].X == spawn.X && spawnPos[x].Y == spawn.Y)
                        {
                            validPosition = false;
                        }
                        else
                        {
                            validPosition = true;
                            break;
                        }
                    }

                    rep++;
                }
                while (rep < 20 && validPosition == false);

                if (validPosition)
                {
                    //Console.WriteLine(spawn);
                    mahBalls.Add(new Ball(i == maxBalls - 1));
                    mahBalls.Last().Position = spawn;
                }
                else
                {
                    Console.WriteLine("Could find a valid spawn position. Exiting spawn loop");
                    break;
                }
            }

            base.Start();
        }

        protected override void LoadAssets()
        {
            //images
            GfxMngr.AddTexture("ball", "Assets/grey_ball.png");
            GfxMngr.AddTexture("backgroundBall", "Assets/background_ball.png");
            GfxMngr.AddTexture("circle", "Assets/circle.png");

            //audio 
            GfxMngr.AddClip("clok", "Assets/Audio/clok_edited.wav");
            GfxMngr.AddClip("thok", "Assets/Audio/thok_edited.wav");
            GfxMngr.AddClip("pop", "Assets/Audio/pop_edited.wav");

            clokClip = GfxMngr.GetClip("clok");

            //fonts
            //FontMngr.AddFont("stdFont", "Assets/textSheet.png", 15, 32, 20, 20);
        }

        public override void Input()
        {
            HandleInput();
        }

        public override void Update()
        {
            PhysicsMngr.Update();

            UpdateMngr.Update();

            PhysicsMngr.CheckCollisions();
        }

        public override Scene OnExit()
        {
            mahBalls = null;
            //Bg = null;

            //BulletMngr.ClearAll();
            //SpawnMngr.ClearAll();
            UpdateMngr.ClearAll();
            PhysicsMngr.ClearAll();
            DrawMngr.ClearAll();
            GfxMngr.ClearAll();
            FontMngr.ClearAll();
            //PowerUpsMngr.ClearAll();

            //DebugMngr.ClearAll();

            return base.OnExit();
        }

        public override void Draw()
        {
            DrawMngr.Draw();

            //DebugMngr.Draw();
        }

        private void HandleInput()
        {
            if (keyboardCtrl.IsRestartKeyPressed())
            {
                Game.CurrentScene.OnExit();
                Game.CurrentScene.Start();
                Console.WriteLine("Restarted");
                Console.Clear();

            }

            if (keyboardCtrl.IsClearWindowKeyPressed())
            {
                if (!pressedSpace)
                {
                    pressedSpace = true;
                    UpdateMngr.ClearAll();
                    PhysicsMngr.ClearAll();
                    DrawMngr.ClearAll();

                    mahBalls = new List<Ball>();
                    Console.WriteLine("Window cleared");
                    Console.Clear();
                }
            }
            else if (pressedSpace)
            {
                pressedSpace = false;
            }

            if (keyboardCtrl.RedBoost())
            {
                if (!pressedG)
                {
                    pressedG = true;
                    foreach (Ball ball in mahBalls)
                    {
                        if (ball.IsInfected)
                        {
                            ball.RigidBody.Velocity *= 5;
                        }
                    }
                    Console.WriteLine("Red boost");
                }
            }
            else if (pressedG)
            {
                pressedG = false;
            }

            if (keyboardCtrl.GreenBoost())
            {
                if (!pressedF)
                {
                    pressedF = true;
                    foreach (Ball ball in mahBalls)
                    {
                        if (!ball.IsInfected)
                        {
                            ball.RigidBody.Velocity *= 5;
                        }
                    }
                    Console.WriteLine("Green boost");
                }
            }
            else if (pressedF)
            {
                pressedF = false;
            }

            if (keyboardCtrl.RedChaseMode())
            {
                if (!pressedT)
                {
                    pressedT = true;
                    RedChase = !RedChase;
                    Console.WriteLine("Red Chase mode: " + RedChase);
                }
            }
            else if (pressedT)
            {
                pressedT = false;
            }
            if (Game.Window.MouseLeft)
            {
                if (!clickedL)
                {
                    clickedL = true;
                    float x = Game.Window.MouseX;
                    float y = Game.Window.MouseY;
                    if (x >= 0 && y >= 0 && x < Game.Window.OrthoWidth && y < Game.Window.OrthoHeight)
                    {
                        mahBalls.Add(new Ball(false));
                        mahBalls.Last().Position = new Vector2(x, y);

                        Game.Source.Pitch = RandomGenerator.GetRandomInt(10, 14) * 0.1f;
                        Game.Source.Play(clokClip);
                        Console.WriteLine("Spawned green ball");
                    }
                }
            }
            else if (clickedL)
            {
                clickedL = false;
            }


            if (Game.Window.MouseRight)
            {
                if (!clickedR)
                {
                    clickedR = true;
                    float x = Game.Window.MouseX;
                    float y = Game.Window.MouseY;
                    if (x >= 0 && y >= 0 && x < Game.Window.OrthoWidth && y < Game.Window.OrthoHeight)
                    {
                        mahBalls.Add(new Ball(true));
                        mahBalls.Last().Position = new Vector2(x, y);
                        Game.Source.Pitch = RandomGenerator.GetRandomInt(10, 14) * 0.1f;
                        Game.Source.Play(clokClip);
                        Console.WriteLine("Spawned red ball");
                    }
                }
            }
            else if (clickedR)
            {
                clickedR = false;
            }
        }
    }
}
