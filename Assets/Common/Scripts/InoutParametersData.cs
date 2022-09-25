using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InoutParametersData
{
    [SerializeField] private TaskNumber _taskNumber;
    [SerializeField] private InputParameterType[] _inputParameterTypes;
    [SerializeField] private OutputParameterType[] _outputParameters;

    public TaskNumber TaskNumber => _taskNumber;
    public IReadOnlyList<InputParameterType> InputParameterTypes => _inputParameterTypes;
    public IReadOnlyList<OutputParameterType> OutputParameterTypes => _outputParameters;
}