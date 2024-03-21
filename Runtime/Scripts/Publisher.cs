//This work is funded by the French program investment of the future under the project JENII - ANR-21-DMES-0006.

using RosMessageTypes.Std;
using TMPro;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using UnityEditor;
using UnityEngine;

public abstract class Publisher<T> : MonoBehaviour where T : Message
{
    protected ROSConnection rosConnection;
    private GameObject frequencyController;
    public string topic;
    public bool isPublishing = true;
    public bool debugMessage;


    protected virtual void Start()
    {
        rosConnection = ROSConnection.GetOrCreateInstance();
        InitializePublisher();

        frequencyController = GameObject.Find("FrequencyController");

        if (frequencyController != null)
        {
            frequencyController.GetComponent<FrequencyController>().onFrequency.AddListener(Publish);
        }
        else
        {
            Debug.LogWarning("FrequencyController not found. Unable to subscribe for frequency updates. Add the prefab in your scene to be able to publish.");
        }

        GameObject recordList = GameObject.Find("RecordList");
        if (recordList != null)
        {
            GameObject displayPrefab = AssetDatabase.LoadAssetAtPath("Packages/com.lineact.d-rrect/Runtime/Prefabs/UI/TextElement.prefab", typeof(GameObject)) as GameObject;
            Instantiate(displayPrefab, recordList.transform).GetComponent<TMP_Text>().text = name;
        }
        else
        {
            Debug.LogWarning("Can't find RecordList in canvas prefab. Add one or ignore this message.");
        }

    }

    protected abstract void InitializePublisher();
    protected abstract T CreateMessage(HeaderMsg header);

    protected void Publish(HeaderMsg header)
    {
        if(isPublishing)
        {
            T message = CreateMessage(header);

            rosConnection.Publish(topic, message);

            if (debugMessage)
            {
                Debug.Log("Publishing on " + topic);
            }
        }
        
    }
}
