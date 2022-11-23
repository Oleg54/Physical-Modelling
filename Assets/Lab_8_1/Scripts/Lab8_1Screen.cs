using System;
using System.Linq;
using UnityEngine;

public class Lab8_1Screen : ParametersScreenBase
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LevelPreset[] _presets;

    private LevelPreset _currentLevelPreset;
    private Lab8_1Behaviour _behaviour;

    protected override PhysicBehaviourBase _physicBehaviourBase => _behaviour;

    protected override void InitCallback()
    {
        UpdatePhysicBehaviour();
    }

    protected override void UpdatePhysicBehaviour()
    {
        _behaviour = new Lab8_1Behaviour(
            _lineRenderer,
            FindInputParameter(InputParameterType.Lab8_1_Angle));

        Refresh();
    }

    protected override void OnTaskSet(TaskNumber taskNumber)
    {
        CreateLevel();
        Refresh();
    }

    protected override void OnInputParameterChange(InputParameter inputParameter)
    {
        if (inputParameter.InputParameterType is InputParameterType.Lab8_1_Angle)
        {
            float angle = FindInputParameter(InputParameterType.Lab8_1_Angle);
            _behaviour?.RecalculateRay(angle);
        }

        Refresh();
    }

    private void Refresh()
    {
        if (CurrentTaskNumber != TaskNumber.Number_3)
        {
            RefractionSurface[] surfaces = _currentLevelPreset.GetComponentsInChildren<RefractionSurface>(true);

            if (surfaces.Length > 0)
            {
                surfaces[0].SetRefractionFactor(FindInputParameter(InputParameterType.Lab8_1_Refraction_Factor_1));

                float s = FindInputParameter(InputParameterType.Lab8_1_S_1);
                surfaces[0].transform.Translate(Vector3.up * surfaces[0].transform.localScale.y / 2f);
                surfaces[0].transform.Translate(Vector3.down * s / 2f);
                surfaces[0].transform.localScale = surfaces[0].transform.localScale.SetY(s);
            }

            if (surfaces.Length > 1)
            {
                surfaces[1].SetRefractionFactor(FindInputParameter(InputParameterType.Lab8_1_Refraction_Factor_2));

                float s = FindInputParameter(InputParameterType.Lab8_1_S_2);
                surfaces[1].transform.localScale = surfaces[1].transform.localScale.SetY(s);
                surfaces[1].transform.position = surfaces[0].transform.position
                    - Vector3.up * (surfaces[0].transform.localScale.y + surfaces[1].transform.localScale.y) / 2f;
            }
        }
    }

    private void CreateLevel()
    {
        if (_currentLevelPreset != null)
            Destroy(_currentLevelPreset.gameObject);

        _currentLevelPreset = Instantiate(_presets[(int)CurrentTaskNumber - 1]);
    }

    private void FixedUpdate()
    {

    }
}
