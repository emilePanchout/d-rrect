//This work is funded by the French program investment of the future under the project JENII - ANR-21-DMES-0006.

using TMPro;
using Unity.Robotics.ROSTCPConnector;
using UnityEditor;
using UnityEngine;

public abstract class Replayer : MonoBehaviour
{
    protected ROSConnection rosConnection;
    public string topic;
    public bool debugMessage;

    protected virtual void Start()
    {
        rosConnection = ROSConnection.GetOrCreateInstance();
        InitializeReplayer();

        GameObject replayList = GameObject.Find("ReplayList");
        if (replayList != null)
        {
            GameObject displayPrefab = AssetDatabase.LoadAssetAtPath("Packages/com.lineact.d-rrect/Runtime/Prefabs/UI/TextElement.prefab", typeof(GameObject)) as GameObject;
            Instantiate(displayPrefab, replayList.transform).GetComponent<TMP_Text>().text = name;
        }
        else
        {
            Debug.LogWarning("Can't find RecordList in canvas prefab. Add one or ignore this message.");
        }

    }

    protected abstract void InitializeReplayer();

}

