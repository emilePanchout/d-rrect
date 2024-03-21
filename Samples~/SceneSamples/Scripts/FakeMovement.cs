using UnityEngine;

public class FakeMovement : MonoBehaviour
{
    public Axis axis;
    public float speed = 1.0f;
    public float distance = 1.0f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    public void Update()
    {
        // Utilisez PingPong pour faire osciller la valeur entre -distance et distance
        float val = Mathf.PingPong(Time.time * speed, distance * 2) - distance;

        switch (axis)
        {
            case Axis.X:
                transform.localPosition = startPosition + new Vector3(val, 0, 0);
                break;
            case Axis.Y:
                transform.localPosition = startPosition + new Vector3(0, val, 0);
                break;
            case Axis.Z:
                transform.localPosition = startPosition + new Vector3(0, 0, val);
                break;
        }
    }
}

public enum Axis
{
    X,
    Y,
    Z
}
