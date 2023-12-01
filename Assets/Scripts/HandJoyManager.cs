using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class HandJoyManager : MonoBehaviour
{
    [SerializeField] GameObject originObject;
    [SerializeField] GameObject targetObject;

    [SerializeField] float dist_thresh = 1.0f;

    [SerializeField] float deadzone = 0.1f;

    public UnityEvent handJoyFront;
    public UnityEvent handJoyBack;
    public UnityEvent handJoyLeft;
    public UnityEvent handJoyRight;
    public UnityEvent handJoyStop;

    // Start is called before the first frame update
    void Start()
    {
        if (originObject == null)
            originObject = gameObject;
    }

    // Update is called once per frame

    tb4Directions prevDirection = tb4Directions.Stop;
    void Update()
    {
        float distance = Vector3.Distance(originObject.transform.position, targetObject.transform.position);
        if (distance < dist_thresh)
        {
            Vector3 direction = originObject.transform.InverseTransformPoint(targetObject.transform.position);
            direction.Normalize();

            Vector3 dominant_axis = GetDominantAxis(direction);

            if (distance < deadzone)
            {
                if (prevDirection != tb4Directions.Stop)
                    handJoyStop.Invoke();
                Debug.Log("Stop");
                prevDirection = tb4Directions.Stop;
            }
            else if (direction.z > 0 && dominant_axis == Vector3.forward)
            {
                if (prevDirection != tb4Directions.Forward)
                {
                    if (prevDirection != tb4Directions.Stop)
                        handJoyStop.Invoke();
                    handJoyFront.Invoke();
                }
                Debug.Log("Target is in front of the origin");
                prevDirection = tb4Directions.Forward;
            }
            else if (direction.z < 0 && dominant_axis == Vector3.forward)
            {
                if (prevDirection != tb4Directions.Backward)
                {
                    if (prevDirection != tb4Directions.Stop)
                        handJoyStop.Invoke();
                    handJoyBack.Invoke();
                }
                Debug.Log("Target is behind the origin");
                prevDirection = tb4Directions.Backward;
            }
            else if (direction.x > 0 && dominant_axis == Vector3.right)
            {
                if (prevDirection != tb4Directions.Right)
                {
                    if (prevDirection != tb4Directions.Stop)
                        handJoyStop.Invoke();
                    handJoyRight.Invoke();
                }
                Debug.Log("Target is to the right of the origin");
                prevDirection = tb4Directions.Right;
            }
            else if (direction.x < 0 && dominant_axis == Vector3.right)
            {
                if (prevDirection != tb4Directions.Left)
                {
                    if (prevDirection != tb4Directions.Stop)
                        handJoyStop.Invoke();
                    handJoyLeft.Invoke();
                }
                Debug.Log("Target is to the left of the origin");
                prevDirection = tb4Directions.Left;
            }
        }
    }



    public Vector3 GetDominantAxis(Vector3 direction)
    {
        float x = Mathf.Abs(direction.x);
        float y = Mathf.Abs(direction.y);
        float z = Mathf.Abs(direction.z);

        if (x > y && x > z)
        {
            return Vector3.right;
        }
        else if (y > x && y > z)
        {
            return Vector3.up;
        }
        else
        {
            return Vector3.forward;
        }
    }


    // Now 'direction' is a vector that points from originObject to targetObject in the local space of the originObject

}


public enum tb4Directions
{
    Forward,
    Backward,
    Left,
    Right,
    Stop
}