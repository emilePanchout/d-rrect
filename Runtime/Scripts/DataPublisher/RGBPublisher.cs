//This work is funded by the French program investment of the future under the project JENII - ANR-21-DMES-0006.

using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using UnityEngine;

public class RGBPublisher : Publisher<ImageMsg>
{
    public Camera cam;
    public int imWidth = 512;
    public int imHeight = 512;

    protected override void InitializePublisher()
    {
        if (cam == null)
        {
            cam = this.gameObject.GetComponent<Camera>();
        }

        rosConnection.RegisterPublisher<ImageMsg>(topic);
    }

    protected override ImageMsg CreateMessage(HeaderMsg header)
    {
        ImageMsg message = new ImageMsg();
        message.header = header;


        // Create a texture and read the camera image into it
        Rect rect = new Rect(0, 0, imWidth, imHeight);
        RenderTexture renderTexture = new RenderTexture(imWidth, imHeight, 24);
        Texture2D image = new Texture2D(imWidth, imHeight, TextureFormat.RGBA32, false);

        cam.targetTexture = renderTexture;
        cam.Render();
        RenderTexture.active = renderTexture;
        image.ReadPixels(rect, 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null;

        Destroy(renderTexture);
        renderTexture = null;

        // Encode texture into ros message
        message = image.ToImageMsg(header);

        return message;
    }

}