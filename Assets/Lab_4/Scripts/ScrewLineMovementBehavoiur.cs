using System;
using UnityEngine;

public class ScrewLineMovementBehavoiur : PhysicBehaviourBase
{
    public readonly float ScrewHeight;
    public readonly float Radius;
    public readonly float StartDelay;
    public readonly float StartSpeed;
    public readonly float StartAccelerate;
    public readonly float T1;
    public readonly float B;

    public float RealTime => Mathf.Max(0, Time - StartDelay);
    public float ScrewCircleLenght => Mathf.Sqrt(Mathf.Pow(ScrewHeight, 2f) + Mathf.Pow(CircleLenght, 2f));
    public float CircleLenght => Mathf.PI * Radius * 2f;
    public float CircleLenghtToHeightRatio => CircleLenght / ScrewHeight;
    public float Accelerate => StartAccelerate + B * RealTime;
    public float LinearSpeed { get; private set; }
    public Vector3 Position => new Vector3(
        Mathf.Cos(Mathf.Deg2Rad * ((AngularSpeed * RealTime) / CircleLenght) * 360f) * Radius,
        RealTime * VerticalSpeed,
        Mathf.Sin(Mathf.Deg2Rad * ((AngularSpeed * RealTime) / CircleLenght) * 360f) * Radius);
    public Vector3 Position_T1 { get; private set; }
    public float Path => (Position.y / ScrewHeight) * ScrewCircleLenght;

    public float VerticalSpeed => (1f / (CircleLenghtToHeightRatio + 1f)) * LinearSpeed;
    public float AngularSpeed => (1f - (1f / (CircleLenghtToHeightRatio + 1f))) * LinearSpeed;

    public ScrewLineMovementBehavoiur(float screwHeight, float startDelay, float radius, float startSpeed, float startAccelerate, float t1, float b)
    {
        ScrewHeight = screwHeight;
        StartDelay = startDelay;
        Radius = radius;
        StartSpeed = startSpeed;
        StartAccelerate = startAccelerate;
        T1 = t1;
        B = b;

        LinearSpeed = startSpeed;
    }

    protected override void OnTick(float time)
    {
        if (StartDelay > Time)
            return;

        LinearSpeed += Accelerate * time;

        if (RealTime <= T1)
            Position_T1 = Position;
    }

    public override string GetParameter(OutputParameterType outputParameterType)
    {
        switch (outputParameterType)
        {
            case OutputParameterType.Time:
                return $"{Time.ToString("F2")}";

            case OutputParameterType.Lab4_Path:
                return $"{Path.ToString("F2")}";

            case OutputParameterType.Lab4_Position_T1:
                return $"{Position_T1}";

            case OutputParameterType.Linear_Speed:
                return $"{LinearSpeed}";

            case OutputParameterType.Lab4_Accelerate:
                return $"{Accelerate}";
        }

        throw new InvalidOperationException();
    }
}
