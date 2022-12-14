using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Lab8_2Behaviour : PhysicBehaviourBase
{
    public readonly LineRenderer Line;
    public float Angle { get; private set; }

    private Vector3 _lastNormal;
    private Point _finalPoint;

    public Lab8_2Behaviour(LineRenderer lineRenderer, float angle)
    {
        Line = lineRenderer;
        Angle = angle;
        _finalPoint = GameObject.FindObjectOfType<Point>();
        RecalculateRay(angle);
    }

    public void RecalculateRay(float angle)
    {
        Angle = angle;

        Vector3 origin = new Vector3(0f, 2.75f, 0f);

        List<Vector3> points = new List<Vector3>();
        bool active = false;
        ThrowRay(origin, 1f, Quaternion.Euler(0f, 0f, angle) * Vector3.down, ref points, ref active);
         if (active)
            _finalPoint?.SetPointColor(Color.green);
         else
            _finalPoint?.SetPointColor(Color.red);

        Line.positionCount = points.Count;
        Line.SetPositions(points.ToArray());
    }

    private bool CheckPoint(Vector3 origin, Vector3 direction, bool applyIfNot = false)
    {
        if (_finalPoint is null)
            _finalPoint = GameObject.FindObjectOfType<Point>(true);

        return (Physics.Raycast(origin, direction, out RaycastHit hit1, 500, 1 << 4));
    }

    private void ThrowRay(Vector3 origin, float originRefractionFactor, Vector3 direction, ref List<Vector3> points, ref bool pointContains)
    {
        direction = direction.SetZ(0f);
        origin = origin.SetZ(0f);

        points.Add(origin);

        if (!pointContains)
            pointContains = CheckPoint(origin, direction);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, 500, 1)
            && hit.transform.TryGetComponent(out RefractionSurface surface))
        {
            if (!surface.IsMirror)
            {
                float internalNextAngle =
                    CalculateNextAngle(Vector3.SignedAngle(-direction, hit.normal, Vector3.forward), originRefractionFactor, surface.RefractionFactor);

                points.Add(hit.point);
                Vector3 firstNextOrigin = hit.point;
                Vector3 firstNextDirection =
                    Quaternion.Euler(0f, 0f, -internalNextAngle) * -hit.normal;

                if (!pointContains)
                    pointContains = CheckPoint(firstNextOrigin + firstNextDirection * 100f, -firstNextDirection);

                RaycastHit reverseHit = Physics.RaycastAll(firstNextOrigin + firstNextDirection * 100f, -firstNextDirection)
                    .Find(h => h.transform == hit.transform);

                Vector3 outOrigin = reverseHit.point;

                _lastNormal = reverseHit.normal;

                if (Physics.Raycast(outOrigin - reverseHit.normal * 0.05f, firstNextDirection, 0.1f))
                    ThrowRay(outOrigin - reverseHit.normal * 0.05f, surface.RefractionFactor, firstNextDirection, ref points, 
                        ref pointContains);
                else
                {
                    float a1 = Vector3.SignedAngle(-firstNextDirection, -reverseHit.normal, Vector3.forward);

                    if (RefractedAngleIsNotReflected(a1, surface.RefractionFactor, 1f))
                    {
                        float a = CalculateNextAngle(a1, surface.RefractionFactor, 1f);
                        Vector3 d = Quaternion.Euler(0f, 0f, -a) * _lastNormal;

                        ThrowRay(outOrigin - reverseHit.normal * 0.05f, 1f, d, ref points, ref pointContains);
                    }
                    else
                    {
                        ThrowRay(outOrigin - reverseHit.normal * 0.05f, 1f, 
                            Vector3.Reflect(firstNextDirection, -reverseHit.normal), ref points, ref pointContains);
                    }
                }
            }
            else
            {
                points.Add(hit.point);
                _lastNormal = hit.normal;

                ThrowRay(hit.point - hit.normal * 0.02f, 1f, Vector3.Reflect(direction, hit.normal), ref points, ref pointContains);
            }
        }
        else
        {
            points.Add(origin + direction * 1000f);
        }
    }

    private float CalculateNextAngle(float originAngle, float originRefractionFactor, float toRefractionFactor)
    {
        if (toRefractionFactor is 0f)
            toRefractionFactor = 1f;

        return Mathf.Rad2Deg 
            * Mathf.Asin((originRefractionFactor * Mathf.Sin(Mathf.Deg2Rad * originAngle)) / toRefractionFactor);
    }

    private bool RefractedAngleIsNotReflected(float originAngle, float originRefractionFactor, float toRefractionFactor)
    {
        float value = (originRefractionFactor * Mathf.Sin(Mathf.Deg2Rad * originAngle)) / toRefractionFactor;
        return value >= -1 && value <= 1;
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
