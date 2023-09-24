using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    public Vector2 minPos;
    public Vector2 maxPos;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPosition.x = Mathf.Clamp(target.position.x, minPos.x, maxPos.x);
            targetPosition.y = Mathf.Clamp(target.position.y, minPos.y, maxPos.y);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }
}
