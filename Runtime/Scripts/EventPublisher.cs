//This work is funded by the French program investment of the future under the project JENII - ANR-21-DMES-0006.

using RosMessageTypes.DRrect;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class EventPublisher : MonoBehaviour
{
    protected ROSConnection rosConnection;
    public string topic;
    public bool debugMessage = true;

    private HeaderMsg header;


    protected virtual void Start()
    {
        rosConnection = ROSConnection.GetOrCreateInstance();
        header = new HeaderMsg();

        InitializePublisher();
        GameObject frequencyController = GameObject.Find("FrequencyController");
        frequencyController.GetComponent<FrequencyController>().SetHeader(header);

    }

    // Initialize the publisher
    protected abstract void InitializePublisher();


    // Create message for non-input events
    protected abstract EventMsg CreateMessage(HeaderMsg header);


    // Create message for input events
    protected abstract EventMsg CreateMessage(HeaderMsg header, Input input);


    // Publish event unrelated to specific inputs
    protected void Publish(InputAction.CallbackContext context)
    {
        EventMsg message = CreateMessage(header);

        rosConnection.Publish(topic, message);

        if (debugMessage)
        {
            Debug.Log("Publishing on " + topic);
        }
    }

    // Publish event related to specific inputs
    protected void Publish(Input input)
    {
        EventMsg message = CreateMessage(header, input);

        rosConnection.Publish(topic, message);

        if (debugMessage)
        {
            Debug.Log("Publishing on " + topic);
        }
    }

}

[System.Serializable]
public class Input
{
    public string name;
    public InputActionReference inputRef;
    public inputType type;
    public string value;
    public float publishDelay;
    public bool isWaiting = false;

    public enum inputType
    {
        Analogue,
        Binary
    }
}