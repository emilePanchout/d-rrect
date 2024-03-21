//This work is funded by the French program investment of the future under the project JENII - ANR-21-DMES-0006.

using LINEACT.LIT.Coordinates;
using RosMessageTypes.Tf2;
using UnityEngine;

public class TransformReplayer : Replayer
{

    protected override void InitializeReplayer()
    {
        rosConnection.Subscribe<TFMessageMsg>(topic, ReplayData);
    }

    // Replay the transforms from the topic to the scene
    private void ReplayData(TFMessageMsg dataMsg)
    {
        for (int i = 0; i < dataMsg.transforms.Length; i++)
        {
            GameObject objectInScene = GameObject.Find(dataMsg.transforms[i].child_frame_id)?.gameObject;

            if (objectInScene != null)
            {
                Vector3 newPos = new Vector3(
                    (float)dataMsg.transforms[i].transform.translation.x,
                    (float)dataMsg.transforms[i].transform.translation.y,
                    (float)dataMsg.transforms[i].transform.translation.z);

                Quaternion newRot = new Quaternion(
                    (float)dataMsg.transforms[i].transform.rotation.x,
                    (float)dataMsg.transforms[i].transform.rotation.y,
                    (float)dataMsg.transforms[i].transform.rotation.z,
                    (float)dataMsg.transforms[i].transform.rotation.w);

                newPos = newPos.FromROS();
                newRot = newRot.FromROS();

                objectInScene.transform.localPosition = newPos;
                objectInScene.transform.localRotation = newRot;
            }
            else
            {
                Debug.LogWarning("Object not found: " + dataMsg.transforms[i].child_frame_id);
            }

            if (debugMessage)
            {
                Debug.Log("Replaying data from " + topic + " : " + dataMsg.transforms[0].child_frame_id + " : " + dataMsg.transforms[i].header.stamp.sec + ":" + dataMsg.transforms[i].header.stamp.nanosec + " seconds");
            }
        }
    }
}
