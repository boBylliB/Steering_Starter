using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler
{
    public bool collided;
    public bool isPlayerA;
    public string tagA;
    public string tagB;

    public Kinematic character;

    public void checkCollision(Collision collision)
    {
        if ((isPlayerA && collision.gameObject.CompareTag(tagB)) || (!isPlayerA && collision.gameObject.CompareTag(tagA)))
        {
            Debug.Log("Collided as " + (isPlayerA ? "Team A" : "Team B") + " with " + collision.gameObject.tag);
            // If we enter this condition, we've collided with an enemy
            collided = true;
            Kinematic enemy = collision.gameObject.GetComponent<Kinematic>();

            // Calculate axis of collision
            Vector3 collisionAxis = enemy.transform.position - character.transform.position;
            collisionAxis.y = 0;
            collisionAxis.Normalize();

            // Calculate relative velocity
            Vector3 relVel = enemy.linearVelocity - character.linearVelocity;
            relVel.y = 0;
            // Project that velocity onto the collision axis
            Vector3 projVel = collisionAxis * Vector3.Dot(relVel, collisionAxis);

            // Assuming elastic collision and both objects being of equal mass (and a boatload of other assumptions to be fair)
            // Our new velocity after collision is then just our previous velocity plus the relative velocity
            // This means we can simply add the projected relative velocity to our velocity and call it a day
            character.linearVelocity += projVel;
        }
    }
}
