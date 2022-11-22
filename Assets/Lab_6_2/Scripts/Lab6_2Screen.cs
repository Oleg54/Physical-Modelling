using System;
using UnityEngine;

public class Lab6_2Screen : ParametersScreenBase
{
    [SerializeField] private LevelPreset[] _presets;

    private LevelPreset _currentLevelPreset;

    private Lab6_2Behaviour _behaviour;

    protected override PhysicBehaviourBase _physicBehaviourBase => _behaviour;

    protected override void InitCallback()
    {
        _startButton.OnActivityChange += c =>
        {
            if (c.ActivityState != ActivityState.Started)
                CreateLevel();
        };
    }

    protected override void OnTaskSet(TaskNumber taskNumber)
    {
        CreateLevel();
        _movementBody.Rigidbody.velocity = Vector3.zero;
        _movementBody.Rigidbody.angularVelocity = Vector3.zero;
    }

    private void CreateLevel()
    {
        if (_currentLevelPreset != null)
            Destroy(_currentLevelPreset.gameObject);

        _currentLevelPreset = Instantiate(_presets[(int)CurrentTaskNumber - 1]);
    }

    protected override void UpdatePhysicBehaviour()
    {
        _behaviour = new Lab6_2Behaviour(
            _movementBody,
            FindInputParameter(InputParameterType.Lab6_2_Start_Speed),
            FindInputParameter(InputParameterType.Lab6_2_Horizontal_Angle));
    }

    private void FixedUpdate()
    {
        if (_activityState != ActivityState.Started)
            _movementBody.Rigidbody.velocity = Vector3.zero;
    }
}
