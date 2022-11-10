using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class HorizontalMovementBehaviour : PhysicBehaviourBase
{
    public readonly float StartHorizontalVelocity;
    public readonly float Height;
    public readonly float StartDelay;
    public readonly float StartAccelerateMagnitude;
    public readonly float HorizontalAngle;
    public readonly bool StartAccelerateIsContinuous;

    public float CurrentAccelerateMagnitude => (Time - StartDelay <= UnityEngine.Time.fixedDeltaTime || StartAccelerateIsContinuous ? StartAccelerateMagnitude : 0f);
    public Vector3 CurrentAccelerate => Physics.gravity +
        new Vector3(Mathf.Cos(Mathf.Deg2Rad * HorizontalAngle), Mathf.Sin(Mathf.Deg2Rad * HorizontalAngle)) * CurrentAccelerateMagnitude;

    public Vector3 CurrentVelocity { get; private set; }
    public Vector3 Position { get; private set; }
    public float Path { get; private set; }
    public float? FinalSpeed { get; private set; }
    public float AverageSpeed => _allVelocitySum / _ticksCount;

    private float _allVelocitySum;
    private int _ticksCount;

    public HorizontalMovementBehaviour(float startHorizontalVelocity, float height, float startDelay, float startAccelerateMagnitude,
        float horizontalAngle, bool startAccelerateIsContinuous)
    {
        StartHorizontalVelocity = startHorizontalVelocity;
        Height = height;
        StartDelay = startDelay;
        StartAccelerateMagnitude = startAccelerateMagnitude;
        HorizontalAngle = horizontalAngle;
        StartAccelerateIsContinuous = startAccelerateIsContinuous;

        CurrentVelocity = Vector3.right * StartHorizontalVelocity;
        Position = new Vector3(0f, Height, 0f);
    }

    protected override void OnTick(float time)
    {
        if (Time < StartDelay)
            return;

        CurrentVelocity += CurrentAccelerate * time;

        Position += CurrentVelocity * time;
        Path += CurrentVelocity.magnitude * time;

        _allVelocitySum += CurrentVelocity.magnitude;
        _ticksCount++;

        if (Position.y < 0f)
        {
            Position = new Vector3(Position.x, 0f, Position.z);

            if (!FinalSpeed.HasValue)
                FinalSpeed = CurrentVelocity.magnitude;
        }
    }

    public override string GetParameter(OutputParameterType outputParameterType)
    {
        switch (outputParameterType)
        {
            case OutputParameterType.Time:
                return $"{Time.ToString("F2")}";

            case OutputParameterType.Lab5_Path:
                return $"{Path.ToString("F2")}";

            case OutputParameterType.Lab5_Average_Speed:
                return $"{AverageSpeed.ToString("F2")}";

            case OutputParameterType.Lab5_Final_Speed:
                return FinalSpeed.HasValue ? $"{FinalSpeed.Value.ToString("F2")}" : "0,0";

            case OutputParameterType.Lab5_Fly_Distance:
                return $"{Mathf.Abs(Position.x).ToString("F2")}";
        }

        throw new InvalidOperationException();
    }
}