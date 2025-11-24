using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public GameObject followObject;
    float smoothing = 1f;
    Vector3 zPositionVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        zPositionVector = new Vector3(0,0,transform.position.z);
        SetPosition(followObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 goal = followObject.transform.position;
        SetPosition(transform.position * (1-smoothing) + followObject.transform.position * smoothing);
    }

    void SetPosition(Vector2 position)
    {
        transform.position = (Vector3)position + zPositionVector;
    }
}
