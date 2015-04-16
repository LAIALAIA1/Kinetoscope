using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Diagnostics;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    class ScreenProjector
    {

        public CalibrationOptions calibrationOptions { get; set; }

        public ScreenProjector(CalibrationOptions options)
        {
            this.calibrationOptions = options;
        }

        private Vector3D mapKinectSpacePointToCameraSpace(Vector3D point)
        {
            return point.add(calibrationOptions.kinectCameraSpace);
        }

        private Vector3D mapCameraSpacePointToScreenPoint(Vector3D point)
        {
            double zScreen = calibrationOptions.topLeftScreenCameraSpace.z;
            double xScreen = (point.x / point.z) * zScreen;
            double yScreen = (point.y / point.z) * zScreen;
            return new Vector3D(xScreen, yScreen, zScreen);;
        }

        private Point mapCameraScreenPointToPixelCoord(Vector3D point)
        {
            double iM = calibrationOptions.topLeftScreenWorldSpace.x - (point.x + calibrationOptions.cameraWorldSpace.x);
            double jM = calibrationOptions.topLeftScreenWorldSpace.y - (point.y + calibrationOptions.cameraWorldSpace.y);


            double iPx = (iM * ((double)calibrationOptions.screenResolutionX / calibrationOptions.wScreen));
            double jPx = (jM * ((double)calibrationOptions.screenResolutionY / calibrationOptions.hScreen));

            //Debug.Print(iPx + " "+jPx);
            return new Point(iPx, jPx);
        }

        public Point projectToScreen(CameraSpacePoint point)
        {
            Vector3D convertedPoint = new Vector3D(point.X, point.Y, point.Z);
            return mapCameraScreenPointToPixelCoord(mapCameraSpacePointToScreenPoint(mapKinectSpacePointToCameraSpace(convertedPoint)));
        }
    }
}
