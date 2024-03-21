//This work is funded by the French program investment of the future under the project JENII - ANR-21-DMES-0006.

using UnityEngine;
using System.Linq;
using System;


    public class LidarSensor : MonoBehaviour
    {
        #region LIDAR Settings
        [Header("Horizontal Settings")]
        public float MinAngleRaysHorizontal = -30;
        public float MaxAngleRaysHorizontal = 30;
        public float DeltaAngleHorizontal = 0.25f;
        [Header("Vertical Settings")]
        public float MinAngleRaysVertical = -15;
        public float MaxAngleRaysVertical = 15;
        public float DeltaAngleVertical = 2f;
        [Header("Distance Max Settings")]
        public float MaxRange = 30f;
        [Header("Noise Settings")]
        public float Imprecision = 0.03f;
        #endregion

        #region Debug
        [Header("Debug Settings")]
        public bool DrawRay = true; // afficher les Raycasts
        #endregion

        public delegate void OnLaserHit(Vector3 point);

        public OnLaserHit onLaserHit;

        public Vector3[] cloudpointsStatic;

        [Header("Number of ray in real time")]
        [SerializeField] public int nbRaysHorizontal;
        [SerializeField] public int nbRaysVertical;

        private void Start()
        {
            nbRaysHorizontal = Mathf.FloorToInt((MaxAngleRaysHorizontal - MinAngleRaysHorizontal) / DeltaAngleHorizontal) +1;
            nbRaysVertical = Mathf.FloorToInt((MaxAngleRaysVertical - MinAngleRaysVertical) / DeltaAngleVertical) +1;

            cloudpointsStatic = new Vector3[nbRaysHorizontal * nbRaysVertical];
            cloudpointsStatic[0] = new Vector3(0, 0, 76555);
            cloudpointsStatic[1] = new Vector3(234567, 45678, 89);
        }

        private System.Random random = new System.Random(); // Ajoutez ceci en haut de votre classe

        private void Update()
        {
            int indexRay = 0;
            for (float azimuthAngle = MinAngleRaysHorizontal; azimuthAngle <= MaxAngleRaysHorizontal; azimuthAngle += DeltaAngleHorizontal)
            {
                for (float elevationAngle = MinAngleRaysVertical; elevationAngle <= MaxAngleRaysVertical; elevationAngle += DeltaAngleVertical)
                {
                    Quaternion azimuthRotation = Quaternion.Euler(0f, azimuthAngle, 0f);
                    Quaternion elevationRotation = Quaternion.Euler(-elevationAngle, 0f, 0f);
                    Vector3 elevationDirection = elevationRotation * transform.forward;
                    Vector3 direction = azimuthRotation * elevationDirection;

                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, direction, out hit, MaxRange))
                    {
                        if (DrawRay) Debug.DrawRay(transform.position, direction * hit.distance, Color.red);

                        // Ajouter du bruit gaussien
                        Vector3 hitPointWithNoise = hit.point + new Vector3(
                            (float)SampleGaussian(random, 0, Imprecision),
                            (float)SampleGaussian(random, 0, Imprecision),
                            (float)SampleGaussian(random, 0, Imprecision)
                        );

                        cloudpointsStatic[indexRay] = transform.InverseTransformPoint(hitPointWithNoise);
                        if (onLaserHit != null)
                        {
                            onLaserHit(hitPointWithNoise);
                        }
                    }
                    else
                    {
                        cloudpointsStatic[indexRay] = Vector3.zero;
                    }
                    indexRay++;
                }
            }
        }

        public Vector3[] CloudpointsNotNull
        {
            get { return cloudpointsStatic.Where(e => e != Vector3.zero).ToArray(); }
        }

        public static double SampleGaussian(System.Random random, double mean, double stddev)
        {
            // The method requires sampling from a uniform random of (0,1]
            // but Random.NextDouble() returns a sample of [0,1).
            double x1 = 1 - random.NextDouble();
            double x2 = 1 - random.NextDouble();

            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return (y1 * stddev + mean);
        }
    }