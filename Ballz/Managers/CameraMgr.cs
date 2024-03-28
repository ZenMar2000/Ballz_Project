using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballz
{
    struct CameraLimits
    {
        public float MaxX;
        public float MaxY;
        public float MinX;
        public float MinY;

        public CameraLimits(float maxX, float minX, float maxY, float minY)
        {
            MaxX = maxX;
            MinX = minX;
            MaxY = maxY;
            MinY = minY;
        }
    }

    static class CameraMgr
    {
        private static Dictionary<string, Tuple<Camera,float>> cameras;


        public static Camera MainCamera { get; private set; }

        public static GameObject Target;
        public static float CameraSpeed = 5;
        public static Vector2 TargetOffset;
        public static CameraLimits CameraLimits;

        public static void Init(GameObject target, CameraLimits cameraLimits)
        {
            MainCamera = new Camera(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f);
            MainCamera.pivot = new Vector2(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f);
            TargetOffset = new Vector2(0, 0);
            Target = target;
            CameraLimits = cameraLimits;

            cameras = new Dictionary<string, Tuple<Camera, float>>();
        }

        public static void Update()
        {
            if(Target == null)
            {
                return;
            }

            //mainCamera.position = Target.Position; //position locking
            Vector2 oldCameraPos = MainCamera.position;
            //smooth lerp
            MainCamera.position = Vector2.Lerp(MainCamera.position, Target.Position + TargetOffset, Game.Window.DeltaTime * CameraSpeed);
            FixPosition();

            //update cameras
            Vector2 cameraDelta = MainCamera.position - oldCameraPos;

            if(cameraDelta != Vector2.Zero )
            {
                //camera moved
                foreach (var item in cameras)
                {
                    //camera.position += cameraDelta * cameraSpeed
                    item.Value.Item1.position += cameraDelta * item.Value.Item2;
                }
            }
        }

        private static void FixPosition()
        {
            MainCamera.position.X = MathHelper.Clamp(MainCamera.position.X, CameraLimits.MinX, CameraLimits.MaxX);
            MainCamera.position.Y = MathHelper.Clamp(MainCamera.position.Y, CameraLimits.MinY, CameraLimits.MaxY);
        }

        public static void AddCamera(string cameraName, Camera camera=null, float cameraSpeed = 0)
        {
            if(camera==null)
            {
                camera = new Camera(MainCamera.position.X, MainCamera.position.Y);
                camera.pivot = MainCamera.pivot;
            }

            cameras[cameraName] = new Tuple<Camera, float>(camera, cameraSpeed);
        }

        public static Camera GetCamera(string cameraName)
        {
            if (cameras.ContainsKey(cameraName))
            {
                return cameras[cameraName].Item1;
            }
            return null;
        }
    }
}
