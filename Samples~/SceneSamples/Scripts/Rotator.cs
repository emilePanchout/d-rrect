using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rotator : MonoBehaviour
{
    public float angle;
    public Axis axis;

    void Update()
    {
        switch (axis)
        {
            case Axis.X:
                transform.Rotate(angle, 0, 0);
                break;
            case Axis.Y:
                transform.Rotate(0, angle, 0);
                break;
            case Axis.Z:
                transform.Rotate(0, 0, angle);
                break;
        }
    }
}

