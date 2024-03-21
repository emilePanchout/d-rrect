using System.IO;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;

public class LoadROSConfig : MonoBehaviour
{
    public ROSConnection connection;
    public string configPath = "Packages/com.lineact.d-rrect/Runtime/rosConfigExample.json";
    void Start()
    {
        if(configPath == null)
        {
            configPath = "Packages/com.lineact.d-rrect/Runtime/rosConfigExample.json";
        }
        string json = File.ReadAllText(configPath);
        RosConfig config = JsonUtility.FromJson<RosConfig>(json);

        connection.Connect(config.ipAddress, config.port);
    }

}

[System.Serializable]
public class RosConfig
{
    public string ipAddress;
    public int port;
}