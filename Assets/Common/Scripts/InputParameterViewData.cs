using System;
using UnityEngine;

[Serializable]
public class InputParameterViewData
{
    [SerializeField] private InputParameterType _inputParameterType;
    [SerializeField] private string _parameterLabel;

    public InputParameterType InputParameterType => _inputParameterType;
    public string ParameterLabel => _parameterLabel;
}
