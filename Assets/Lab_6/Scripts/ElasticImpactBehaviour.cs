using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElasticImpactBehaviour : PhysicBehaviourBase
{
    public readonly MovementBody Body1;
    public readonly MovementBody Body2;
    public readonly float M1;
    public readonly float M2;
    public readonly float Speed1;
    public readonly float Speed2;
    public readonly bool ImpactIsForward;

    private bool _wasCollision;

    public ElasticImpactBehaviour(MovementBody body1, MovementBody body2, float m1, float m2, 
        float speed1, float speed2, bool impactIsForward)
    {
        Body1 = body1;
        Body2 = body2;
        M1 = m1;
        M2 = m2;
        Speed1 = speed1;
        Speed2 = speed2;
        ImpactIsForward = impactIsForward;

        body1.Rigidbody.position = new Vector3(-2.5f, 0, 0);
        body2.Rigidbody.position = new Vector3(2.5f, 0, 0);

        if (!impactIsForward)
            body2.Rigidbody.position += Vector3.up * 0.125f;

        body1.Rigidbody.mass = m1;
        body2.Rigidbody.mass = m2;

        body1.Rigidbody.velocity = Vector3.right * speed1;
        body2.Rigidbody.velocity = Vector3.left * speed2;

        body1.Rigidbody.angularVelocity = Vector3.zero;
        body2.Rigidbody.angularVelocity = Vector3.zero;

        body1.OnCollision += c => OnCollision(c.contacts.First().normal);
        body2.OnCollision += c => OnCollision(-c.contacts.First().normal);
    }

    private void OnCollision(Vector3 normal)
    {
        if (_wasCollision)
            return;

        _wasCollision = true;

        Debug.Log(1);

        float commonImpulse = 
            Body1.Rigidbody.mass * Body1.Rigidbody.velocity.magnitude
            + Body2.Rigidbody.mass * Body2.Rigidbody.velocity.magnitude;

        Debug.Log(Body1.Rigidbody.velocity);
        Debug.Log(Body2.Rigidbody.velocity);


        Body1.Rigidbody.velocity = normal * (commonImpulse / 2 / Body1.Rigidbody.mass);
        Body2.Rigidbody.velocity = -normal * (commonImpulse / 2 / Body2.Rigidbody.mass);
        
        Debug.Log(commonImpulse);
        Debug.Log(Body1.Rigidbody.velocity);
        Debug.Log(Body2.Rigidbody.velocity);

        Body1.Rigidbody.velocity = new Vector3(Body1.Rigidbody.velocity.x, Body1.Rigidbody.velocity.y, 0f);
        Body2.Rigidbody.velocity = new Vector3(Body2.Rigidbody.velocity.x, Body2.Rigidbody.velocity.y, 0f);
    }

    public override string GetParameter(OutputParameterType outputParameterType)
    {
        switch (outputParameterType)
        {
            case OutputParameterType.Lab6_Speed_Body_1:
                return $"{Body1.Rigidbody.velocity} ({Body1.Rigidbody.velocity.magnitude})";

            case OutputParameterType.Lab6_Speed_Body_2:
                return $"{Body2.Rigidbody.velocity} ({Body2.Rigidbody.velocity.magnitude})";

            case OutputParameterType.Lab6_Time:
                return $"{Time}";

            default:
                return string.Empty;
        }
    }
}
