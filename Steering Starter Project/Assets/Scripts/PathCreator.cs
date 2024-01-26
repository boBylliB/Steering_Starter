using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public float difference = 0.2f;
    public GameObject pathTarget;
    public Path pathController;

    // Store the previous position
    Vector3 prevPosition;
    
    void Start()
    {
        prevPosition = transform.position;
    }

    void FixedUpdate()
    {
        if ((transform.position - prevPosition).magnitude > difference)
        {
            prevPosition = transform.position;
            pathController.pathTargets.Add(Instantiate(pathTarget, prevPosition, Quaternion.identity));
        }
    }
}
