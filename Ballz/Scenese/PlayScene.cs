﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Ballz
{
    class PlayScene : Scene
    {
        protected int maxBalls =100;
        public List<Ball> myBalls { get; protected set; }

        public float GroundY { get; protected set; }

        public PlayScene() : base()
        {

        }

        public override void Start()
        {
            LoadAssets();
            myBalls = new List<Ball>();
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
                    myBalls.Add(new Ball(i == maxBalls-1));
                    myBalls.Last().Position = spawn;
                }
                else
                {
                    Console.WriteLine("Could find a valid spawn position. Exiting spawn loop");
                    break;
                }
            }

            Thread.Sleep(1000);
            base.Start();
        }

        protected override void LoadAssets()
        {
            //images
            GfxMngr.AddTexture("ball", "Assets/grey_ball.png");

            //fonts
            //FontMngr.AddFont("stdFont", "Assets/textSheet.png", 15, 32, 20, 20);
        }

        public override void Input()
        {
            //Optional input
        }

        public override void Update()
        {
            PhysicsMngr.Update();

            UpdateMngr.Update();

            PhysicsMngr.CheckCollisions();
        }

        public override Scene OnExit()
        {
            myBalls = null;
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
