using System;
using System.Linq;
using UnityEngine;

public class Lab8_2Screen : ParametersScreenBase
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private RefractionLense[] _lenses;
    [SerializeField] private LevelPreset[] _presets;

    private LevelPreset _currentLevelPreset;
    private Lab8_2Behaviour _behaviour;

    protected override PhysicBehaviourBase _physicBehaviourBase => _behaviour;

    protected override void InitCallback()
    {
        UpdatePhysicBehaviour();
    }

    protected override void UpdatePhysicBehaviour()
    {
        _behaviour = new Lab8_2Behaviour(
            _lineRenderer,
            FindInputParameter(InputParameterType.Lab8_2_Angle));

        Refresh();
    }

    protected override void OnTaskSet(TaskNumber taskNumber)
    {
        CreateLevel();
        Refresh();
    }

    protected override void OnInputParameterChange(InputParameter inputParameter)
    {
        Refresh();
    }

    private void Refresh()
    {
        //RefractionSurface[] surfaces = _currentLevelPreset.GetComponentsInChildren<RefractionSurface>(true);

        //if (surfaces.Length > 0)
        //{
        //    surfaces[0].SetRefractionFactor(FindInputParameter(InputParameterType.Lab8_2_Refraction_Factor_1));

        //    if (CurrentTaskNumber != TaskNumber.Number_3)
        //    {
        //        float s = FindInputParameter(InputParameterType.Lab8_2_S_1);

        //        if (s < 0.01f)
        //            s = 1f;

        //        surfaces[0].transform.Translate(Vector3.up * surfaces[0].transform.localScale.y / 2f);
        //        surfaces[0].transform.Translate(Vector3.down * s / 2f);
        //        surfaces[0].transform.localScale = surfaces[0].transform.localScale.SetY(s);
        //    }
        //}

        //if (surfaces.Length > 1)
        //{
        //    surfaces[1].SetRefractionFactor(FindInputParameter(InputParameterType.Lab8_2_Refraction_Factor_2));

        //    if (CurrentTaskNumber != TaskNumber.Number_3)
        //    {
        //        float s = FindInputParameter(InputParameterType.Lab8_2_S_2);
                
        //        if (s < 0.01f)
        //            s = 1f;

        //        surfaces[1].transform.localScale = surfaces[1].transform.localScale.SetY(s);
        //        surfaces[1].transform.position = surfaces[0].transform.position
        //            - Vector3.up * (surfaces[0].transform.localScale.y + surfaces[1].transform.localScale.y) / 2f;
        //    }
        //}

        if (CurrentTaskNumber is TaskNumber.Number_2)
        {
            RefractionLense[] refractionLenses = _currentLevelPreset.GetComponentsInChildren<RefractionLense>(true);

            RefractionLense newLense1 = Instantiate(
                _lenses[Mathf.Clamp((int)FindInputParameter(InputParameterType.Lab8_2_Lense_Type_1), 0, _lenses.Length - 1)],
                refractionLenses[0].transform.position,
                refractionLenses[0].transform.rotation,
                _currentLevelPreset.transform);

            RefractionLense newLense2 = Instantiate(
                _lenses[Mathf.Clamp((int)FindInputParameter(InputParameterType.Lab8_2_Lense_Type_2), 0, _lenses.Length - 1)],
                refractionLenses[1].transform.position,
                refractionLenses[1].transform.rotation,
                _currentLevelPreset.transform);

            DestroyImmediate(refractionLenses[0].gameObject);
            DestroyImmediate(refractionLenses[1].gameObject);

            newLense1.SetRefractionFactor(FindInputParameter(InputParameterType.Lab8_2_Refraction_Factor_1));
            newLense2.SetRefractionFactor(FindInputParameter(InputParameterType.Lab8_2_Refraction_Factor_2));
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
        _behaviour?.RecalculateRay(FindInputParameter(InputParameterType.Lab8_2_Angle));
    }
}
