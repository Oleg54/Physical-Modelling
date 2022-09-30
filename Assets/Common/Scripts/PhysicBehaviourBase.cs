using System;
using UnityEngine;

public abstract class PhysicBehaviourBase
{
    public float Time { get; private set; }
    public event Action OnUpdate;

    public void Tick(float time)
    {
        Time += time;

        OnTick(time);
        OnUpdate?.Invoke();
    }

    protected virtual void OnTick(float time) { }

    public abstract string GetParameter(OutputParameterType outputParameterType);
}