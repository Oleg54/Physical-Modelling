using System;
using UnityEngine;

public class AccelerationMovementBehaviour : PhysicBehaviourBase
{
    public readonly float AValue;
    public readonly float BValue;
    public readonly float CValue;
    public readonly float DValue;
    public readonly float T1;
    public readonly float T2;
    public readonly float T3;

    public Vector2 CurrentAccelerate => new Vector2(AValue + BValue * Time, CValue + DValue * Time);
    public Vector2 CurrentVelocity { get; private set; }
    public Vector2 Position { get; private set; }
    public float Path { get; private set; }

    public float Path_T2 { get; private set; }
    public Vector2 Accelerate_T3 { get; private set; }
    public Vector2 Speed_T3 { get; private set; }

    public AccelerationMovementBehaviour(float aValue, float bValue, float cValue, float dValue, float t1, float t2, float t3)
    {
        AValue = aValue;
        BValue = bValue;
        CValue = cValue;
        DValue = dValue;
        T1 = t1;
        T2 = t2;
        T3 = t3;
    }

    protected override void OnTick(float time)
    {
        Position += CurrentVelocity * time;

        Path += CurrentVelocity.magnitude * time;

        //путь через интегрирование
        //функция ускорения: a(t) = A + Bt
        //функция скорости (первообразная): v(t) = At + Bt^2 / 2
        //путь: интеграл A-B (At + Bt^2 / 2)
        //
        //сразу:
        //Path = ((T2 * T2 * T2) / 3 + (T2 * T2) / 2) - ((T1 * T1 * T1) / 3 + (T1 * T1) / 2);
        //
        //по степени увеличения T2 (от 0 до конечного значения)
        //float t2 = Mathf.Max(Mathf.Min(Time, T2), T1);
        //Path = ((t2 * t2 * t2) / 3 + (t2 * t2) / 2) - ((T1 * T1 * T1) / 3 + (T1 * T1) / 2);

        if (Time >= T1)
            CurrentVelocity += CurrentAccelerate * time;

        if (Time - time <= T2)
            Path_T2 = Path;

        if (Time - time <= T3)
        {
            Accelerate_T3 = CurrentAccelerate;
            Speed_T3 = CurrentVelocity;
        }
    }

    public override string GetParameter(OutputParameterType outputParameterType)
    {
        switch (outputParameterType)
        {
            case OutputParameterType.Path_T2:
                return $"{Path_T2.ToString("F2")}";
            case OutputParameterType.Speed_T3:
                return $"{Speed_T3} ({Speed_T3.magnitude.ToString("F2")})";
            case OutputParameterType.Accelerate_T3:
                return $"{Accelerate_T3} ({Accelerate_T3.magnitude.ToString("F2")})";
        }

        throw new ArgumentException();
    }
}