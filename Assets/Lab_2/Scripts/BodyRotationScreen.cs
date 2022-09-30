using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BodyRotationScreen : ParametersScreenBase
{
    private AngularMovementBehaviour _angularMovementBehaviour;
    protected override PhysicBehaviourBase _physicBehaviourBase => _angularMovementBehaviour;

    private void FixedUpdate()
    {
        if (_activityState is ActivityState.Stopped)
            return;

        _angularMovementBehaviour.Tick(Time.fixedDeltaTime);
        _movementBody.SetPosition(_angularMovementBehaviour.Position);
    }

    protected override void UpdatePhysicBehaviour()
    {
        _angularMovementBehaviour = new AngularMovementBehaviour(
            FindInputParameter(InputParameterType.Circle_Radius),
            FindInputParameter(InputParameterType.Rotation_Frequency));
    }
}
