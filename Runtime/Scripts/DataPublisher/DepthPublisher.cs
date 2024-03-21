//This work is funded by the French program investment of the future under the project JENII - ANR-21-DMES-0006.

using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using UnityEngine;

public class DepthPublisher : Publisher<ImageMsg>
{
    public Camera cam;
    public Shader depthShader; // Référence au shader de profondeur
    public int imWidth = 512;
    public int imHeight = 512;

    protected override void InitializePublisher()
    {
        if (cam == null)
        {
            cam = this.gameObject.GetComponent<Camera>();
        }


        rosConnection.RegisterPublisher<ImageMsg>(topic);

        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
        cam.SetReplacementShader(depthShader, "");

        RenderTexture renderTexture = new RenderTexture(1920, 1080, 16);
        cam.targetTexture = renderTexture;

    }

    protected override ImageMsg CreateMessage(HeaderMsg header)
    {
        ImageMsg message = new ImageMsg();
        message.header = header;


        Rect rect = new Rect(0, 0, imWidth, imHeight);
        RenderTexture renderTexture = new RenderTexture(imWidth, imHeight, 24);
        Texture2D screenShot = new Texture2D(imWidth, imHeight, TextureFormat.R16, false);

        cam.targetTexture = renderTexture;
        cam.Render();
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null;

        Destroy(renderTexture);
        renderTexture = null;

        message = screenShot.ToImageMsg(header);

        return message;
    }

    
}
