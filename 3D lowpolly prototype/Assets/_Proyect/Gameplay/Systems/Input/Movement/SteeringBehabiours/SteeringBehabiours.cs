using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SteeringBehabiours 
{
    public static Vector2 Seek(Rigidbody rigidbody, Vector2 target, float maxSpeed, float maxForce)
    {
        Vector2 position = new Vector2(rigidbody.position.x, rigidbody.position.z);
        Vector2 velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.z);

        Vector2 desiredVelocity = target - position;
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        Vector2 steering = desiredVelocity - velocity;
        float sterMagnitude = steering.magnitude;
        if ( sterMagnitude > maxForce)
        {
            steering = (steering / sterMagnitude) * maxForce;
        }

        return steering;
    }

    public static Vector2 Seek(Rigidbody rigidbody, Vector2 desiredVelocity, float maxForce)
    {
        Vector2 velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.z);

        Vector2 steering = desiredVelocity - velocity;
        float sterMagnitude = steering.magnitude;
        if (sterMagnitude > maxForce)
        {
            steering = (steering / sterMagnitude) * maxForce;
        }

        return steering;
    }

    public static Vector2 ArribeXZ(Rigidbody rigidbody, Vector2 target, float maxSpeed, float maxForce, float decreaseVelRadious, float stopRadious)
    {
        Vector2 position = new Vector2(rigidbody.position.x, rigidbody.position.z);
        Vector2 velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.z);

        Vector2 desiredVelocity;
        
        float distance = Vector2.Distance(position, target);
        if (distance <= stopRadious)
        {
            desiredVelocity = Vector2.zero;
        }
        else if (distance < decreaseVelRadious)
        {
            desiredVelocity = target - position;
            desiredVelocity = desiredVelocity.normalized * (distance / decreaseVelRadious * maxSpeed);
        }
        else
        {
            desiredVelocity = target - position;
            desiredVelocity = desiredVelocity.normalized * maxSpeed;
        }

        return Seek(rigidbody, desiredVelocity, maxForce);
    }
}
