using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ballz
{
    class PlayScene : Scene
    {
        protected int maxBalls =100
            ;
        //protected GameObject Bg;
        public List<Ball> balls { get; protected set; }

        public float GroundY { get; protected set; }

        public PlayScene() : base()
        {

        }

        public override void Start()
        {
            LoadAssets();
            balls = new List<Ball>();
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

                    spawn = new Vector2(randX, randY);
                    for (int x = 0; x < spawnPos.Count(); x++)
                    {
                        if (spawnPos[x] == spawn)
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
                    balls.Add(new Ball());
                    balls.Last().Position = spawn;
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
            //GfxMngr.AddTexture("BG", "Assets/hex_grid_green.png");

            GfxMngr.AddTexture("ball", "Assets/grey_ball.png");
            //GfxMngr.AddTexture("player_2", "Assets/player_2.png");
            //GfxMngr.AddTexture("enemy_0", "Assets/enemy_0.png");
            //GfxMngr.AddTexture("enemy_1", "Assets/enemy_1.png");

            //GfxMngr.AddTexture("barFrame", "Assets/loadingBar_frame.png");
            //GfxMngr.AddTexture("blueBar", "Assets/loadingBar_bar.png");

            //GfxMngr.AddTexture("bullet", "Assets/fireball.png");
            //GfxMngr.AddTexture("heart", "Assets/heart.png");

            //GfxMngr.AddTexture("tile_crate", "Assets/Levels/crate.png");
            //GfxMngr.AddTexture("tile_earth", "Assets/Levels/earth.png");
            //GfxMngr.AddTexture("tile_earthGrass", "Assets/Levels/earthGrass.png");
            //GfxMngr.AddTexture("tile_stone", "Assets/Levels/stone.png");

            //fonts
            //FontMngr.AddFont("stdFont", "Assets/textSheet.png", 15, 32, 20, 20);
            //FontMngr.AddFont("comics", "Assets/comics.png", 10, 32, 61, 65);
        }

        public override void Input()
        {
            //Optional input
        }

        public override void Update()
        {
            //if (!myBall.IsAlive)
            //    IsPlaying = false;

            PhysicsMngr.Update();
            UpdateMngr.Update();

            PhysicsMngr.CheckCollisions();

            //CameraMgr.Update();
        }

        public override Scene OnExit()
        {
            balls = null;
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
    }
}
