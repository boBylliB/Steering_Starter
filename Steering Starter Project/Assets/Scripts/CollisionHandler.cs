using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public bool collided;
    public bool isPlayerA;
    public string tagA;
    public string tagB;

    public Kinematic character;

    public void Start()
    {
        collided = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (isPlayerA && collision.gameObject.CompareTag(tagB) || !isPlayerA && collision.gameObject.CompareTag(tagA))
        {
            // If we enter this condition, we've collided with an enemy
            collided = true;

            // Calculate momentum transfer
            
        }
    }
}
