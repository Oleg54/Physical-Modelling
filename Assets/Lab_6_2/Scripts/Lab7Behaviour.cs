using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lab7Behaviour : PhysicBehaviourBase
{
    public readonly MovementBody Body;
    public readonly float Speed;

    public Lab7Behaviour(MovementBody body, float speed, float horizontalAngle)
    {
        Body = body;
        Speed = speed;

        body.Rigidbody.position = Vector3.zero;
        body.Rigidbody.velocity = 
            new Vector2(Mathf.Cos(horizontalAngle * Mathf.Deg2Rad), Mathf.Sin(horizontalAngle * Mathf.Deg2Rad)).normalized * speed;

        body.Rigidbody.angularVelocity = Vector3.zero;
        body.OnCollision += OnCollision;
    }

    private void OnCollision(Collision collision)
    {
        Obstacle obstacle;

        if (!collision.transform.TryGetComponent(out obstacle))
            return;

        float commonImpulse = 
            Body.Rigidbody.mass * Body.LastFrameVelocity.magnitude
            + obstacle.Rigidbody.mass * obstacle.Rigidbody.velocity.magnitude;

        Vector3 direction = Vector3.Reflect(Body.LastFrameVelocity.normalized, collision.contacts.First().normal);
        direction.z = 0;

        if (obstacle.ObstacleType is ObstacleType.Static)
        {
            Body.Rigidbody.velocity = direction * (commonImpulse / Body.Rigidbody.mass);
        }
        else if (obstacle.ObstacleType is ObstacleType.Static_Accelerate)
        {
            Body.Rigidbody.velocity = Vector3.ClampMagnitude(direction * (commonImpulse / Body.Rigidbody.mass) 
                * Random.Range(1.1f, 1.6f), 20);
        }
        else if (obstacle.ObstacleType is ObstacleType.Dynamic)
        {
            Body.Rigidbody.velocity = direction * (commonImpulse / 2 / Body.Rigidbody.mass);
            obstacle.Rigidbody.velocity = -collision.contacts.First().normal * (commonImpulse / 2 / obstacle.Rigidbody.mass);
        }
    }

    public override string GetParameter(OutputParameterType outputParameterType)
    {
        switch (outputParameterType)
        {
            default:
                return string.Empty;
        }
    }
}
