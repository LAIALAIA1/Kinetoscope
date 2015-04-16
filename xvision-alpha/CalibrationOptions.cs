using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    class CalibrationOptions
    {
        public double hScreen { get; set; }
        public double wScreen { get;set;}
        public int screenResolutionX { get; set; }
        public int screenResolutionY { get; set; }
        public Vector3D cameraWorldSpace { get; set; }
        public Vector3D kinectWorldSpace { get; set; }
        public Vector3D kinectCameraSpace { get; set; }
        public Vector3D topLeftScreenWorldSpace { get; set; }
        public Vector3D bottomRightScreenWorldSpace { get; set; }
        public Vector3D topLeftScreenCameraSpace { get; set; }
        public Vector3D bottomRightScreenCameraSpace { get; set; }



        public CalibrationOptions(Vector3D cameraWorldSpace, Vector3D kinectWorldSpace, Vector3D topLeftScreenWorldSpace, double hScreen, double wScreen, int screenResolutionX, int screenResolutionY)
        {
            this.cameraWorldSpace = cameraWorldSpace; //camera position into world space
            this.kinectWorldSpace = kinectWorldSpace; //kinect position into world space
            this.topLeftScreenWorldSpace = topLeftScreenWorldSpace;
            this.hScreen = hScreen;
            this.wScreen = wScreen;
            this.screenResolutionX = screenResolutionX;
            this.screenResolutionY = screenResolutionY;

            init(cameraWorldSpace, kinectWorldSpace, hScreen, wScreen);
        }

        private void init(Vector3D cameraWorldSpace, Vector3D kinectWorldSpace, double hScreen, double wScreen)
        {
            //calculate kinect pos in camera space
            this.kinectCameraSpace = kinectWorldSpace.sub(cameraWorldSpace);

            //calculate bottomright screen pos in world space
            Vector3D translateTopLeftToBottomRight = new Vector3D(wScreen, hScreen, 0);
            this.bottomRightScreenWorldSpace = topLeftScreenWorldSpace.sub(translateTopLeftToBottomRight);

            //calculate topleft screen in camera space
            this.topLeftScreenCameraSpace = topLeftScreenWorldSpace.sub(cameraWorldSpace);

            //calculate bottomright screen in camera space
            this.bottomRightScreenCameraSpace = bottomRightScreenWorldSpace.sub(cameraWorldSpace);
        }

    }
}
