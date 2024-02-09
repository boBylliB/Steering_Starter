using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendedBehavior : SteeringBehavior
{
    public float maxAcceleration = 10f;
    public float maxRotation = 45f;

    // This function should be overridden by all blended behaviors to set the behaviors to utilize
    public virtual List<SteeringBehavior> getBehaviors()
    {
        return new List<SteeringBehavior>();
    }
    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        // Accumulate all behaviors
        foreach (SteeringBehavior behavior in getBehaviors())
        {
            SteeringOutput partialResult = behavior.getSteering();
            result.linear += partialResult.linear * behavior.weight;
            result.angular += partialResult.angular * behavior.weight;
        }

        // Clamp result to bounds
        if (result.linear.magnitude > maxAcceleration)
            result.linear = result.linear.normalized * maxAcceleration;
        if (Mathf.Abs(result.angular) > maxRotation)
            result.angular = Mathf.Sign(result.angular) * maxRotation;

        return result;
    }
}
