using System.Collections.Generic;
using UnityEngine;

public class Lab8_1Behaviour : PhysicBehaviourBase
{
    public readonly LineRenderer Line;
    public float Angle { get; private set; }

    private Vector3 _lastNormal;

    public Lab8_1Behaviour(LineRenderer lineRenderer, float angle)
    {
        Line = lineRenderer;
        Angle = angle;
        RecalculateRay(angle);
    }

    public void RecalculateRay(float angle)
    {
        Angle = angle;

        Vector3 origin = new Vector3(0f, 2.75f, 0f);

        List<Vector3> points = new List<Vector3>();
        ThrowRay(origin, 1f, Quaternion.Euler(0f, 0f, angle) * Vector3.down, ref points);

        Line.positionCount = points.Count;
        Line.SetPositions(points.ToArray());
    }

    private void ThrowRay(Vector3 origin, float originRefractionFactor, Vector3 direction, ref List<Vector3> points)
    {
        points.Add(origin);

        if (Physics.Raycast(origin, direction, out RaycastHit hit)
            && hit.transform.TryGetComponent(out RefractionSurface surface))
        {
            float internalNextAngle = 
                CalculateNextAngle(Vector3.SignedAngle(-direction, hit.normal, Vector3.forward), originRefractionFactor, surface.RefractionFactor);

            points.Add(hit.point);
            Vector3 firstNextOrigin = hit.point;
            Vector3 firstNextDirection = 
                Quaternion.Euler(0f, 0f, -internalNextAngle) * -hit.normal;

            RaycastHit reverseHit = Physics.RaycastAll(firstNextOrigin + firstNextDirection * 100f, -firstNextDirection)
                .Find(h => h.transform == hit.transform);

            Vector3 outOrigin = reverseHit.point;

            _lastNormal = reverseHit.normal;
            ThrowRay(outOrigin + Vector3.up * 0.05f, surface.RefractionFactor, firstNextDirection, ref points);
        }
        else
        {
            float internalNextAngle =
                CalculateNextAngle(Vector3.SignedAngle(-direction, -_lastNormal, Vector3.forward), originRefractionFactor, 1f);

            Vector3 firstNextDirection = Quaternion.Euler(0f, 0f, -internalNextAngle) * _lastNormal;
            points.Add(origin + firstNextDirection * 1000f);
        }
    }

    private float CalculateNextAngle(float originAngle, float originRefractionFactor, float toRefractionFactor)
    {
        if (toRefractionFactor is 0f)
            toRefractionFactor = 1f;

        return Mathf.Rad2Deg * Mathf.Asin((originRefractionFactor * Mathf.Sin(Mathf.Deg2Rad * originAngle)) / toRefractionFactor);
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
