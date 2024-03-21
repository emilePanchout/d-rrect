//This work is funded by the French program investment of the future under the project JENII - ANR-21-DMES-0006.

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace com.lineact.ar
{
    public class CameraIntrinsicSetup : MonoBehaviour
    {

        // Vincent WC calibration
        // Focal Length [ 817.454  824.140 ]
        // [ 817.454  0  696.851;  0  824.140  356.517;  0  0  1 ]

        // https://stackoverflow.com/questions/39992968/how-to-calculate-field-of-view-of-the-camera-from-camera-intrinsic-matrix/41137160#41137160
        // for FOV with fx & fy

        Camera mainCamera;

        public string Name; // name of the camera just info
        public float FocalLength = 35.0f; // f can be arbitrary, as long as sensor_size is resized to make ax,ay consistient
        public float Fov_vertical = 0; // à mettre à jour avec les params intrinsèque de la caméra
        public float size_mm_vertical = 16; // f can be arbitrary, as long as sensor_size is resized to make ax,ay consistient

        public float fx, fy, cx, cy;
        public float width, height;
        private void OnEnable()
        {
            Screen.SetResolution((int)width, (int)height, false);
        }
        // Use this for initialization
        void Start()
        {
            mainCamera = gameObject.GetComponent<Camera>();
            ChangeCameraParam();
        }

        public void ChangeCameraParam()
        {
            if(Fov_vertical > 0)
            {
                FocalLength = Camera.FieldOfViewToFocalLength(Fov_vertical, size_mm_vertical);//38 mm est arbitraire
            }
            //string path = "Assets/Resources/Intrinsic.txt";
            //float ax, ay, sizeX, sizeY;
            //float x0, y0, shiftX, shiftY;
            //int width, height;
            float sizeX, sizeY;
            float shiftX, shiftY;

            //string[] lines = File.ReadAllLines(path);
            //string[] parameters = lines[1].Split(' ');
            //string[] resolution = lines[3].Split(' ');

            //ax = float.Parse(parameters[0]);
            //ay = float.Parse(parameters[1]);
            //x0 = float.Parse(parameters[2]);
            //y0 = float.Parse(parameters[3]);

            //width = int.Parse(resolution[0]);
            //height = int.Parse(resolution[1]);

            sizeX = FocalLength * width / fx;
            sizeY = FocalLength * height / fy;

            //PlayerSettings.defaultScreenWidth = width;
            //PlayerSettings.defaultScreenHeight = height;

            shiftX = -(cx - width / 2.0f) / width;
            shiftY = (cy - height / 2.0f) / height;

            mainCamera.sensorSize = new Vector2(sizeX, sizeY);     // in mm, mx = 1000/x, my = 1000/y
            mainCamera.focalLength = FocalLength;                  // in mm, ax = f * mx, ay = f * my 
            // focal lens and field of view: field of view = 2 * arctan(sensor size / (focal length * 2))
            mainCamera.lensShift = new Vector2(shiftX, shiftY);    // W/2,H/w for (0,0), 1.0 shift in full W/H in image plane

        }
    }
}
