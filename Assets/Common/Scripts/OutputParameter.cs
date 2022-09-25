using System;
using TMPro;
using UnityEngine;

public class OutputParameter : MonoBehaviour
{
    [SerializeField] private OutputParameterType _outputParameterType;
    [SerializeField] private TMP_Text _textLabel;
    [SerializeField] private TMP_Text _textValue;

    public OutputParameterType OutputParameterType => _outputParameterType;

    private void Awake()
    {
        _textLabel.text = GetComponentInParent<ParametersScreenBase>().Config.OutputParametersViewData[_outputParameterType].ParameterLabel;
    }

    private void OnValidate()
    {
        gameObject.name = $"[OUTPUT] {_outputParameterType}";
    }

    public void SetOutput(string value)
    {
        _textValue.text = value;
    }
}
