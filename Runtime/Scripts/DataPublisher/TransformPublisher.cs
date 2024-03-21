//This work is funded by the French program investment of the future under the project JENII - ANR-21-DMES-0006.

using LINEACT.LIT.Coordinates;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using RosMessageTypes.Tf2;
using UnityEngine;


public class TransformPublisher : Publisher<TFMessageMsg>
{
    public GameObject target;
    private Transform[] objectArray;
    private TransformStampedMsg[] transformArray;

    protected override void InitializePublisher()
    {
        if (target == null)
        {
            target = this.gameObject;
        }

        rosConnection.RegisterPublisher<TFMessageMsg>(topic);
        objectArray = target.GetComponentsInChildren<Transform>(true); // true include inactive children
        transformArray = new TransformStampedMsg[objectArray.Length];

    }

    protected override TFMessageMsg CreateMessage(HeaderMsg header)
    {
        TFMessageMsg message = new TFMessageMsg();
        int i = 0;

        foreach (Transform transform in objectArray)
        {
            TransformStampedMsg transformStamped = new TransformStampedMsg();

            if(transform.parent == null)
            {
                transformStamped.header = header;
            }
            else
            {
                transformStamped.header = new HeaderMsg();
                transformStamped.header.stamp = header.stamp;
                transformStamped.header.frame_id = transform.parent.name;
            }

            transformStamped.child_frame_id = transform.name;


            Vector3 localPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            Vector3 rosPos = localPos.ToROS();

            transformStamped.transform.translation.x = rosPos.x;
            transformStamped.transform.translation.y = rosPos.y;
            transformStamped.transform.translation.z = rosPos.z;
            

            Quaternion localRot = new Quaternion(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
            Quaternion rosRot = localRot.ToROS();

            transformStamped.transform.rotation.w = rosRot.w;
            transformStamped.transform.rotation.x = rosRot.x;
            transformStamped.transform.rotation.y = rosRot.y;
            transformStamped.transform.rotation.z = rosRot.z;

            transformArray[i] = transformStamped;
            i++;

        }

        message.transforms = transformArray;

        return message;

    }

}