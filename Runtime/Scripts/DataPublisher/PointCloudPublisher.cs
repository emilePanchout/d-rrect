using LINEACT.LIT.Coordinates;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using System;
using UnityEngine;

public class PointCloudPublisher : Publisher<PointCloud2Msg>
{
    public LidarSensor lidarSensor;

    protected override void InitializePublisher()
    {
        rosConnection.RegisterPublisher<PointCloud2Msg>(topic);
    }

    protected override PointCloud2Msg CreateMessage(HeaderMsg header)
    {
        // Créer un nouveau message PointCloud
        PointCloud2Msg message = new PointCloud2Msg();
        message.header = header;

        // Définir les champs de données du nuage de points
        PointFieldMsg[] fields = new PointFieldMsg[3];
        fields[0] = new PointFieldMsg();
        fields[0].name = "x";
        fields[0].offset = 0;
        fields[0].datatype = PointFieldMsg.FLOAT32;
        fields[0].count = 1;

        fields[1] = new PointFieldMsg();
        fields[1].name = "y";
        fields[1].offset = 4;
        fields[1].datatype = PointFieldMsg.FLOAT32;
        fields[1].count = 1;

        fields[2] = new PointFieldMsg();
        fields[2].name = "z";
        fields[2].offset = 8;
        fields[2].datatype = PointFieldMsg.FLOAT32;
        fields[2].count = 1;

        message.fields = fields;

        // Définir la hauteur et la largeur du nuage de points
        message.height = (uint)lidarSensor.nbRaysVertical;
        message.width = (uint)lidarSensor.nbRaysHorizontal;

        // Définir si le nuage de points est dense ou non
        message.is_dense = true; // Si tous les points sont valides
        message.point_step = 12;

        // Remplissage des données des points du nuage de points
        int numberOfPoints = lidarSensor.nbRaysHorizontal * lidarSensor.nbRaysVertical;
        message.data = new byte[numberOfPoints * 12]; // Chaque point a 3 coordonnées float32 (12 octets par point)

        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector3 point = lidarSensor.cloudpointsStatic[i].ToROS();
            byte[] pointBytes = BitConverter.GetBytes(point.x);
            Buffer.BlockCopy(pointBytes, 0, message.data, i * 12, 4); // Copiez les 4 octets de la coordonnée x
            pointBytes = BitConverter.GetBytes(point.y);
            Buffer.BlockCopy(pointBytes, 0, message.data, i * 12 + 4, 4); // Copiez les 4 octets de la coordonnée y
            pointBytes = BitConverter.GetBytes(point.z);
            Buffer.BlockCopy(pointBytes, 0, message.data, i * 12 + 8, 4); // Copiez les 4 octets de la coordonnée z
        }

        return message;
    }
}
