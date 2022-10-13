using UnityEngine;

public class ScrewLineScreen : ParametersScreenBase
{
    private ScrewLineMovementBehavoiur _movementBehaviour;
    protected override PhysicBehaviourBase _physicBehaviourBase => _movementBehaviour;

    protected override void UpdatePhysicBehaviour()
    {
        _movementBehaviour = new ScrewLineMovementBehavoiur(
            FindInputParameter(InputParameterType.Lab4_Screw_Height),
            FindInputParameter(InputParameterType.Lab4_T),
            FindInputParameter(InputParameterType.Lab4_R),
            FindInputParameter(InputParameterType.Lab4_Speed),
            FindInputParameter(InputParameterType.Lab4_Start_Acceleration),
            FindInputParameter(InputParameterType.Lab4_T1),
            FindInputParameter(InputParameterType.Lab4_B));
    }

    private void FixedUpdate()
    {
        if (_activityState != ActivityState.Started)
            return;

        _movementBehaviour.Tick(Time.fixedDeltaTime);
        _movementBody.SetPosition(_movementBehaviour.Position);
    }
}