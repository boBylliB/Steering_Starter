using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<GameObject> pathTargets;

    // This is needed to keep the same path parameter while removing GameObjects from the list
    int idxOffset = 0;

    private float ClosestParamAlongLine(Vector3 point, Vector3 start, Vector3 end)
    {
        return -Vector3.Dot(end - start, start - point) / Vector3.Dot(end - start, end - start);
    }
    private float DistanceToTarget(int idx, Vector3 position)
    {
        if (idx - idxOffset < pathTargets.Count && idx - idxOffset >= 0)
        {
            return (pathTargets[idx - idxOffset].transform.position - position).magnitude;
        }
        else
        {
            // We return the max value if the point doesn't exist because all the logic later will select smaller distances
            // This way, a separate error code or special output isn't needed
            return float.MaxValue;
        }
    }
    // The path parameter in this implementation is defined such that it linearly increases along the path by 1 per target
    // For example, between target 0 and 1, the parameter linearly increases from 0 to 1,
    // and between target 1 and 2, the parameter linearly increases from 1 to 2
    public float GetParam(Vector3 position, float lastParam)
    {
        // If there are no path targets, return 0
        if (pathTargets.Count == 0) return 0;

        // We can use lastParam to determine if we've passed any new pathTargets, and remove them if so
        while (Mathf.FloorToInt(lastParam) > idxOffset)
        {
            ++idxOffset;
            pathTargets.RemoveAt(0);
        }

        // Use the lastParam to find the starting search index
        int searchIdx = Mathf.FloorToInt(lastParam);

        // Search forward from that index until a local minimum distance is found
        float prevDist = float.MaxValue;
        float nextDist = DistanceToTarget(searchIdx, position);
        do
        {
            ++searchIdx;
            prevDist = nextDist;
            nextDist = DistanceToTarget(searchIdx, position);
        } while (nextDist < prevDist);
        Debug.Log("prevDist = " + prevDist + " nextDist = " + nextDist);
        // Since it ends with the previous distance being a local minimum, we need to decrement the index by 1
        --searchIdx;
        // We actually need to find the closest line segment, not just point, so we then select the minimum of the two options on either side of the closest target
        int endIdx = DistanceToTarget(searchIdx - 1, position) < DistanceToTarget(searchIdx + 1, position) ? searchIdx - 1 : searchIdx + 1;
        Debug.Log(" searchIdx = " + searchIdx + " endIdx = " + endIdx)

        // Now, we just need to find the closest point along that line segment, and add it to the found index
        // In this case, since we're using it as a path parameter, we can actually directly use a parameter along the line,
        // negated to be 0 to 1 instead of 0 to -1
        // The closest point on the line SHOULD be between the two points, since we're using the two closest points
        return searchIdx + ClosestParamAlongLine(position, pathTargets[searchIdx - idxOffset].transform.position, pathTargets[endIdx - idxOffset].transform.position);
    }
    // This function converts a path parameter to an actual point along the path
    public Vector3 GetPosition(float param, Vector3 defaultPosition)
    {
        // If there are no path targets, return the given default position
        if (pathTargets.Count == 0) return defaultPosition;
        // If there is only 1 path target, return that position
        if (pathTargets.Count == 1) return pathTargets[0].transform.position;

        // Use the parameter given to find the path targets to use
        int startIdx = Mathf.FloorToInt(param);

        // Use the path targets to create a line segment, for which the parameter can directly be used to find a point along that line segment
        Vector3 start = pathTargets[startIdx - idxOffset].transform.position;
        Vector3 end = pathTargets[startIdx - idxOffset - 1].transform.position;

        // Return the point along that line corresponding to the given parameter
        return start + (param - startIdx) * (end - start);
    }
}