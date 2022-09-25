using System;
using UnityEngine;

public class AngularMovementBehaviour
{
    public readonly float RotationFrequency;
    public readonly float Radius;

    public float Time { get; private set; }
    public float Angle => Time * RotationFrequency * 360f;
    public float TurnoversCount => RotationFrequency * Time;
    public float AngularVelocity => Time > 0 ? Angle / Time : 0;
    public Vector2 Position => new Vector2(Mathf.Sin(Angle * Mathf.Deg2Rad), Mathf.Cos(Angle * Mathf.Deg2Rad)) * Radius;
    public float Path => (Mathf.PI * 2f * Radius) * (Angle / 360f);
    public float LinearSpeed => Time > 0 ? Path / Time : 0;

    public AngularMovementBehaviour(float radius, float rotationFrequency)
    {
        RotationFrequency = rotationFrequency;
        Radius = radius;
    }

    public void Tick(float time)
    {
        Time += time;
    }

    public string GetParameter(OutputParameterType outputParameterType)
    {
        switch (outputParameterType)
        {
            case OutputParameterType.Time:
                return $"{Time}";
            case OutputParameterType.Linear_Speed:
                return $"{LinearSpeed}";
            case OutputParameterType.Traveled_Path:
                return $"{Path}";
            case OutputParameterType.Position:
                return $"{Position}";
            case OutputParameterType.Turnovers_Count:
                return $"{TurnoversCount}";
            case OutputParameterType.Angle:
                return $"{Angle % 360}";
            case OutputParameterType.Angle_Speed:
                return $"{AngularVelocity}";
        }

        throw new ArgumentException();
    }
}