using UnityEngine;

public class AccelerationParametersScreen : ParametersScreenBase
{
    private AccelerationMovementBehaviour _movementBehaviour;
    protected override PhysicBehaviourBase _physicBehaviourBase => _movementBehaviour;

    protected override void UpdatePhysicBehaviour()
    {
        _movementBehaviour = new AccelerationMovementBehaviour(
            FindInputParameter(InputParameterType.A_Value),
            FindInputParameter(InputParameterType.B_Value),
            FindInputParameter(InputParameterType.C_Value),
            FindInputParameter(InputParameterType.D_Value),
            FindInputParameter(InputParameterType.Time_T1),
            FindInputParameter(InputParameterType.Time_T2),
            FindInputParameter(InputParameterType.Time_T3));
    }

    private void FixedUpdate()
    {
        if (_activityState != ActivityState.Started)
            return;

        _movementBehaviour.Tick(Time.fixedDeltaTime);
        _movementBody.SetPosition(_movementBehaviour.Position);

        if (_movementBehaviour.T2 > 0 && _movementBehaviour.Time >= _movementBehaviour.T2)
        {
            _startButton.SetState(ActivityState.Stopped);
        }
    }
}