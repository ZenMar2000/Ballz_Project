using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Ballz
{
    static class Game
    {
        // Variables
        public static Window Window;
        //public static Background Bg;
        //public static Ball Ball;

        //private static KeyboardController keyboardCtrl;
        //private static List<Controller> controllers;

        // Properties
        public static Scene CurrentScene { get; private set; }
        public static float DeltaTime { get { return Window.DeltaTime; } }
        public static Vector2 ScreenCenter { get; private set; }

        public static float OptimalScreenHeight { get; private set; }
        public static float UnitSize { get; private set; }
        public static float OptimalUnitSize { get; private set; }

        public static float HalfDiagonalSquared { get { return ScreenCenter.LengthSquared; } }


        public static void Init()
        {
            Window = new Window(1280, 720, "Heads");
            Window.SetDefaultViewportOrthographicSize(10);

            OptimalScreenHeight = 1080;//best resolution
            UnitSize = Window.Height / Window.OrthoHeight;//72 (1280x720 HD)
            OptimalUnitSize = OptimalScreenHeight / Window.OrthoHeight;//108  (1080/10)

            ScreenCenter = new Vector2(Window.OrthoWidth * 0.5f, Window.OrthoHeight * 0.5f);

            // SCENES
            //TitleScene titleScene = new TitleScene("titleScreen");
            PlayScene playScene = new PlayScene();
            //GameOverScene gameOverScene = new GameOverScene();

            //titleScene.NextScene = playScene;
            playScene.NextScene = null;
            //gameOverScene.NextScene = titleScene;

            CurrentScene = playScene;

            // CONTROLLERS
            // Always create a keyboard controller (init at 0 cause we only have 1 keyboard)
            //keyboardCtrl = new KeyboardController(0);
        }

        //public static Controller GetController(int index)
        //{
        //    Controller ctrl = keyboardCtrl;

        //    if (index < controllers.Count)
        //    {
        //        ctrl = controllers[index];
        //    }

        //    return ctrl;
        //}

        public static float PixelsToUnits(float pixelsSize)
        {
            return pixelsSize / OptimalUnitSize;
        }

        public static void Play()
        {
            CurrentScene.Start();

            while (Window.IsOpened)
            {
                //if (!Ball.IsAlive)
                //    return;

                //// INPUT
                //if (Window.GetKey(KeyCode.Esc))
                //{
                //    return;
                //}

                //Ball.Input();

                //// UPDATE
                //PhysicsMngr.Update();
                //UpdateMngr.Update();
                //SpawnMngr.Update();
                //PowerUpsMngr.Update();

                //// COLLISIONS
                //PhysicsMngr.CheckCollisions();

                //// DRAW
                //DrawMngr.Draw();
                //DebugMngr.Draw();

                // Show FPS on Window Title Bar
                Window.SetTitle($"FPS: {1f / Window.DeltaTime}");

                // Exit when ESC is pressed
                if (Window.GetKey(KeyCode.Esc))
                {
                    break;
                }

                if (!CurrentScene.IsPlaying)
                {
                    Scene nextScene = CurrentScene.OnExit();

                    if (nextScene != null)
                    {
                        CurrentScene = nextScene;
                        CurrentScene.Start();
                    }
                    else
                    {
                        return;
                    }
                }

                // INPUT
                CurrentScene.Input();

                // UPDATE
                CurrentScene.Update();

                // DRAW
                CurrentScene.Draw();

                Window.Update();
            }
        }
    }
}
