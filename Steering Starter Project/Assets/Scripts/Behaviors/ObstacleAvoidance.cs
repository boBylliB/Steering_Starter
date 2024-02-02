using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : Seek
{
    // Minimum distance to a wall
    public float avoidDist = 30f;
    // The distance to look for collisions
    public float lookAhead = 10f;
    // For debug purposes, I'm also rendering a line
    public LineRenderer lr;
    public Material hitMat;
    public Material missMat;

    protected override Vector3 getTargetPosition(out bool valid)
    {
        // We want to store raycast data to get the point of intersection
        RaycastHit hitInfo;
        // Set debug line renderer stuff
        lr.SetPosition(0, character.transform.position);
        lr.SetPosition(1, character.linearVelocity.normalized * lookAhead + character.transform.position);
        // Raycast ahead to check for a collision
        // For some unknown reason, the raycast seems to detect collisions much further out than intended
        // I've divided the actual lookahead distance by 10 here, as that matches up pretty well
        if (Physics.Raycast(character.transform.position, character.linearVelocity, out hitInfo, lookAhead))
        {
            lr.material = hitMat;
            valid = true;
            Vector3 targetPoint = hitInfo.point + hitInfo.normal * avoidDist;
            // Deliberately remove any y values
            targetPoint.y = 0;
            return targetPoint;
        }
        else
        {
            lr.material = missMat;
            // If there's no collision, set the seek position to be invalid
            //valid = false;
            //return Vector3.positiveInfinity;
            // For demonstration, instead of doing nothing we accelerate at full speed forwards
            valid = true;
            return character.transform.position + character.linearVelocity.normalized;
        }
    }
}
