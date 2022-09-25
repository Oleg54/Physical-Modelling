using System;
using UnityEngine;

[Serializable]
public class OutputParameterViewData
{
    [SerializeField] private OutputParameterType _outputParameterType;
    [SerializeField] private string _parameterLabel;

    public OutputParameterType OutputParameterType => _outputParameterType;
    public string ParameterLabel => _parameterLabel;
}