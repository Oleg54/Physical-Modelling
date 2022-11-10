using UnityEngine;

public class HorizontalThrowScreen : ParametersScreenBase
{
    private HorizontalMovementBehaviour _horizontalMovementBehaviour;
    protected override PhysicBehaviourBase _physicBehaviourBase => _horizontalMovementBehaviour;

    protected override void UpdatePhysicBehaviour()
    {
        _horizontalMovementBehaviour = new HorizontalMovementBehaviour(
            FindInputParameter(InputParameterType.Lab5_Start_Speed), 
            FindInputParameter(InputParameterType.Lab5_Height), 
            FindInputParameter(InputParameterType.Lab5_Start_Delay), 
            FindInputParameter(InputParameterType.Lab5_Acceleration), 
            FindInputParameter(InputParameterType.Lab5_Horizontal_Angle), 
            CurrentTaskNumber is TaskNumber.Number_3);
    }

    private void FixedUpdate()
    {
        if (_activityState != ActivityState.Started)
            return;

        if (CurrentTaskNumber != TaskNumber.Number_3 && _horizontalMovementBehaviour.Position.y <= 0f)
            _startButton.SetState(ActivityState.Stopped);

        _horizontalMovementBehaviour.Tick(Time.fixedDeltaTime);
        _movementBody.SetPosition(_horizontalMovementBehaviour.Position);
    }
}
