//This work is funded by the French program investment of the future under the project JENII - ANR-21-DMES-0006.

using RosMessageTypes.Std;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FrequencyController : MonoBehaviour
{
    public float delay = 3.0f;
    public FrequencyEvent onFrequency;
    
    private void Start()
    {
        StartCoroutine(PublishRequest(delay));
        Debug.Log("Publishing initialized to " + delay + " seconds");
    }

    // Trigger a unity event every x seconds. Publisher will be able to subscribe to this event to publish on the frequency
    IEnumerator PublishRequest(float seconds)
    {
        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer >= seconds)
            {
                HeaderMsg headerMsg = new HeaderMsg();
                SetHeader(headerMsg);

                onFrequency.Invoke(headerMsg);

                timer = 0f;
            }

            yield return null;
        }
    }

    // Set the header of the ros message sent when subscribing to the frequency event
    public HeaderMsg SetHeader(HeaderMsg header)
    {
#if !ROS2
        header.stamp.sec = (uint)Time.timeAsDouble;
#else
        header.stamp.sec = (int)Time.timeAsDouble;
#endif
        header.stamp.nanosec = ((uint)((Time.timeAsDouble - Time.timeAsDouble) * 1000000000));
        header.frame_id = "unity";

        return header;
    }
}


[System.Serializable]
public class FrequencyEvent : UnityEvent<HeaderMsg>
{
}