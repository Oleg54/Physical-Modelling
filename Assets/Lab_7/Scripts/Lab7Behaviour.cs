using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lab7Behaviour : PhysicBehaviourBase
{
    public readonly MovementBody Body;
    public readonly float T1;
    public readonly float F;
    public readonly float M;
    public readonly float A;

    public float CurrentF => Time >= T1 ? F + A * Mathf.Max(0, (Time - T1)) : 0f;

    private SurfaceType _lastSurfaceType = SurfaceType.Surface_1;

    public Lab7Behaviour(MovementBody body, float t1, float f, float m, float a)
    {
        Body = body;
        T1 = t1;
        F = f;
        M = m;
        A = a;

        Body.Rigidbody.mass = m;
    }

    protected override void OnTick(float time)
    {
        Body.Rigidbody.angularVelocity = Vector3.zero;

        Body.Rigidbody.AddForce(Body.Rigidbody.mass * 9.81f * Vector3.down, ForceMode.Force);

        if (Physics.Raycast(Body.Rigidbody.position, Vector3.down + Vector3.left * 0.2f, out RaycastHit hit))
        {
            Body.Rigidbody.rotation = 
                Quaternion.Slerp(Body.Rigidbody.rotation, 
                Quaternion.LookRotation(Quaternion.Euler(0f, 0f, 90f) * hit.normal), 
                UnityEngine.Time.fixedDeltaTime * 55f);

            Surface surface = hit.transform.GetComponent<Surface>();

            if (surface != null)
            {
                if (surface.SurfaceType != SurfaceType.Surface_3)
                    Body.Rigidbody.AddForce(Vector3.left * CurrentF, ForceMode.Force);

                _lastSurfaceType = surface.SurfaceType;
            }
        }
    }

    public override string GetParameter(OutputParameterType outputParameterType)
    {
        switch (outputParameterType)
        {
            case OutputParameterType.Lab7_Speed:
                return $"{Body.Rigidbody.velocity} ({Body.Rigidbody.velocity.magnitude.ToString("F2")})";

            case OutputParameterType.Lab7_Time:
                return $"{Mathf.Max(0, Time - T1).ToString("F2")}";

            case OutputParameterType.Lab7_Current_F:
                return $"{CurrentF.ToString("F2")}";

            default:
                return string.Empty;
        }
    }
}
