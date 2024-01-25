using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : Kinematic
{
    Wander mySteeringType;

    // Start is called before the first frame update
    void Start()
    {
        mySteeringType = new Wander();
        mySteeringType.character = this;
    }

    // Update is called once per frame
    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate.linear = mySteeringType.getSteering().linear;
        steeringUpdate.angular = mySteeringType.getSteering().angular;
        base.Update();
    }
}