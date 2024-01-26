using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : Seek
{
    // The distance along the path to generate a target
    // Theoretically this could be negative,
    // but in my implementation the path offset SHOULD BE POSITIVE
    // This is because, to save on memory, path targets that we pass are removed automatically
    public float pathOffset = 1f;

    // The Path class to use
    public Path path;

    float predictionTime = 0.1f;
    float currentParam = 0f;

    // Overrides Seek's target position with the targeted point on the path
    protected override Vector3 getTargetPosition()
    {
        // Predict the future character position
        Vector3 futurePos = character.transform.position + character.linearVelocity * predictionTime;

        // Find the current position on the path
        currentParam = path.GetParam(futurePos, currentParam);

        // Offset it
        float targetParam = currentParam + pathOffset;

        // Get the target position
        // Default to the predicted future position, to not change anything about our motion if the path doesn't exist
        Vector3 targetPosition = path.GetPosition(targetParam, futurePos);

        return targetPosition;
    }
}
