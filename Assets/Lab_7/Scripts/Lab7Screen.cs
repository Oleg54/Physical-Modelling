using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lab7Screen : ParametersScreenBase
{
    [SerializeField] private LevelPreset[] _presets;

    private LevelPreset _currentLevelPreset;
    private IReadOnlyDictionary<SurfaceType, Surface> _currentSurfaces;

    private Lab7Behaviour _behaviour;

    protected override PhysicBehaviourBase _physicBehaviourBase => _behaviour;

    protected override void InitCallback()
    {
        _startButton.OnActivityChange += c =>
        {
            if (c.ActivityState != ActivityState.Started)
                CreateLevel();

            UpdateAngle();
        };
    }

    protected override void OnInputParameterChange(InputParameter inputParameter)
    {
        if (inputParameter.InputParameterType is InputParameterType.Lab7_Surface_Angle)
            UpdateAngle();
    }

    private void UpdateAngle()
    {
        if (CurrentTaskNumber is TaskNumber.Number_2)
        {
            float angle = Mathf.Clamp(FindInputParameter(InputParameterType.Lab7_Surface_Angle), 0f, 89f);
            float rotatedSurfaceWidth = _currentSurfaces[SurfaceType.Surface_2].transform.localScale.x;

            Vector3 firstPoint = _currentSurfaces[SurfaceType.Surface_3].transform.position
                    + Vector3.right * _currentSurfaces[SurfaceType.Surface_3].transform.localScale.x / 2f;

                Vector3 secondPoint = firstPoint
                    + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f) * rotatedSurfaceWidth;

                Vector3 averagePoint = (firstPoint + secondPoint) / 2f;

                _currentSurfaces[SurfaceType.Surface_2].transform.position = averagePoint;
                _currentSurfaces[SurfaceType.Surface_2].transform.eulerAngles = Vector3.forward * angle;

                _currentSurfaces[SurfaceType.Surface_1].transform.position = secondPoint
                    + Vector3.right * _currentSurfaces[SurfaceType.Surface_1].transform.localScale.x / 2f;
        }
        else if (CurrentTaskNumber is TaskNumber.Number_3)
        {
            float angle = Mathf.Clamp(FindInputParameter(InputParameterType.Lab7_Surface_Angle), 0f, 89f);
            float rotatedSurfaceWidth = _currentSurfaces[SurfaceType.Surface_2].transform.localScale.x;

            Vector3 firstPoint = _currentSurfaces[SurfaceType.Surface_1].transform.position
                    + Vector3.left * _currentSurfaces[SurfaceType.Surface_1].transform.localScale.x / 2f;

            Vector3 secondPoint = firstPoint
                + new Vector3(-Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f) * rotatedSurfaceWidth;

            Vector3 averagePoint = (firstPoint + secondPoint) / 2f;

            _currentSurfaces[SurfaceType.Surface_2].transform.position = averagePoint;
            _currentSurfaces[SurfaceType.Surface_2].transform.eulerAngles = Vector3.back * angle;

            _currentSurfaces[SurfaceType.Surface_3].transform.position = secondPoint
                + Vector3.left * _currentSurfaces[SurfaceType.Surface_3].transform.localScale.x / 2f;

            _movementBody.Rigidbody.position = _currentSurfaces[SurfaceType.Surface_1].transform.position
                + Vector3.up * 0.6f
                + Vector3.right * 2.5f;
        }

        if (CurrentTaskNumber is TaskNumber.Number_1 || CurrentTaskNumber is TaskNumber.Number_2)
        {
            Vector3 position = _currentSurfaces[SurfaceType.Surface_1].transform.position
                + Vector3.up * 0.5f
                + Vector3.left *
                (CurrentTaskNumber is TaskNumber.Number_1 ? 3f : _currentSurfaces[SurfaceType.Surface_1].transform.localScale.x / 2f + 0.5f);
            _movementBody.transform.position = position;
            _movementBody.Rigidbody.position = position;
        }

        _movementBody.Rigidbody.rotation = Quaternion.LookRotation(Vector3.left);
        _movementBody.Rigidbody.velocity = Vector3.zero;
        _movementBody.Rigidbody.angularVelocity = Vector3.zero;
    }

    protected override void OnTaskSet(TaskNumber taskNumber)
    {
        CreateLevel();
        _movementBody.Rigidbody.velocity = Vector3.zero;
        _movementBody.Rigidbody.angularVelocity = Vector3.zero;
        UpdateAngle();
    }

    private void CreateLevel()
    {
        if (_currentLevelPreset != null)
            Destroy(_currentLevelPreset.gameObject);

        _currentLevelPreset = Instantiate(_presets[(int)CurrentTaskNumber - 1]);
        _currentSurfaces = _currentLevelPreset.GetComponentsInChildren<Surface>(true).ToDictionary(s => s.SurfaceType, s => s);
    }

    protected override void UpdatePhysicBehaviour()
    {
        _behaviour = new Lab7Behaviour(_movementBody,
            FindInputParameter(InputParameterType.Lab7_T1),
            FindInputParameter(InputParameterType.Lab7_F),
            FindInputParameter(InputParameterType.Lab7_Mass),
            FindInputParameter(InputParameterType.Lab7_A));

        _currentSurfaces[SurfaceType.Surface_1].GetComponent<Collider>().material.staticFriction
            = FindInputParameter(InputParameterType.Lab7_Friction1);

        _currentSurfaces[SurfaceType.Surface_1].GetComponent<Collider>().material.dynamicFriction
            = FindInputParameter(InputParameterType.Lab7_Friction1);

        if (CurrentTaskNumber != TaskNumber.Number_1)
        {
            _currentSurfaces[SurfaceType.Surface_2].GetComponent<Collider>().material.staticFriction
                = FindInputParameter(InputParameterType.Lab7_Friction2);

            _currentSurfaces[SurfaceType.Surface_2].GetComponent<Collider>().material.dynamicFriction
                = FindInputParameter(InputParameterType.Lab7_Friction2);


            _currentSurfaces[SurfaceType.Surface_3].GetComponent<Collider>().material.staticFriction
                = FindInputParameter(InputParameterType.Lab7_Friction3);

            _currentSurfaces[SurfaceType.Surface_3].GetComponent<Collider>().material.dynamicFriction
                = FindInputParameter(InputParameterType.Lab7_Friction3);
        }

        UpdateAngle();

        float startSpeed = FindInputParameter(InputParameterType.Lab7_Start_Speed);
        _movementBody.Rigidbody.AddForce(Vector3.left * startSpeed, ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        if (_activityState is ActivityState.Started && CurrentTaskNumber is TaskNumber.Number_1)
        {
            if (FindInputParameter(InputParameterType.Lab7_Mass) 
                * FindInputParameter(InputParameterType.Lab7_Friction1) * 9.81f < _behaviour.CurrentF)
                _movementBody.Rigidbody.velocity = Vector3.left;
            else
                _movementBody.Rigidbody.velocity = Vector3.zero;

        }

        if (_activityState != ActivityState.Started)
        {
            _movementBody.Rigidbody.velocity = Vector3.zero;
            _movementBody.Rigidbody.angularVelocity = Vector3.zero;

            return;
        }

        _behaviour?.Tick(Time.deltaTime);
    }
}
