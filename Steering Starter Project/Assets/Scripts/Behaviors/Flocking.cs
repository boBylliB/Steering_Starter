using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : BlendedBehavior
{
    public Kinematic character;
    public List<Kinematic> flock;
    public FlockCOM centerOfMass;
    public float targetSpeed;

    // The collision radius of a character
    public float radius = 1f;
    public float sepThreshold = 10f;

    // Minimum distance to a wall
    public float avoidDist = 30f;
    // The distance to look for collisions
    public float lookAhead = 10f;
    // Tags to ignore
    public List<string> ignoredTags;
    // The line renderer to use for debugging raycasts
    public bool debug = false;
    public LineRenderer lr;
    public Material hitMat;
    public Material missMat;

    // The number of rays to cast
    public int numRays = 1;
    // The ray spread angle
    public float rayAngle = 10f;

    public float sepWeight = 1f;
    public float alignWeight = 1f;
    public float matchWeight = 1f;
    public float cohesionWeight = 1f;
    public float avoidWeight = 1f;
    public float dodgeWeight = 1f;

    public override List<SteeringBehavior> getBehaviors()
    {
        List<SteeringBehavior> behaviors = new List<SteeringBehavior>();

        // Separation
        Separation sep = new Separation();
        sep.character = character;
        sep.targets = flock;
        sep.weight = sepWeight;
        sep.threshold = sepThreshold;
        sep.maxAcceleration = 3f;
        behaviors.Add(sep);

        // Alignment
        VelocityAlign align = new VelocityAlign();
        align.character = character;
        align.target = centerOfMass;
        align.weight = alignWeight;
        behaviors.Add(align);

        // Speed Matching
        SpeedMatch match = new SpeedMatch();
        match.character = character;
        match.targetSpeed = targetSpeed;
        match.weight = matchWeight;
        behaviors.Add(match);

        // Cohesion
        Pursue cohesion = new Pursue();
        cohesion.character = character;
        cohesion.target = centerOfMass.gameObject;
        cohesion.weight = cohesionWeight;
        behaviors.Add(cohesion);

        // Wall avoidance
        ObstacleAvoidance avoid = new ObstacleAvoidance();
        avoid.character = character;
        avoid.avoidDist = avoidDist;
        avoid.lookAhead = lookAhead;
        avoid.lr = lr;
        avoid.hitMat = hitMat;
        avoid.missMat = missMat;
        avoid.debug = debug;
        avoid.ignoredTags = ignoredTags;
        avoid.numRays = numRays;
        avoid.rayAngle = rayAngle;
        behaviors.Add(avoid);

        // Collision avoidance
        AggressiveCollisionAvoidance dodge = new AggressiveCollisionAvoidance();
        dodge.character = character;
        dodge.targets = flock;
        dodge.radius = radius;
        behaviors.Add(dodge);

        return behaviors;
    }
}
