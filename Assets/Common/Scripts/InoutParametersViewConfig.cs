using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "InoutParametersViewConfig", menuName = "Config/InoutParametersViewConfig")]
public class InoutParametersViewConfig : ScriptableObject
{
    [SerializeField] private InoutParametersData[] _inoutParametersData;
    [SerializeField] private InputParameterViewData[] _inputParametersViewData;
    [SerializeField] private OutputParameterViewData[] _outputParametersViewData;

    public IReadOnlyDictionary<TaskNumber, InoutParametersData> InoutParametersData { get; private set; }
    public IReadOnlyDictionary<InputParameterType, InputParameterViewData> InputParametersViewData { get; private set; }
    public IReadOnlyDictionary<OutputParameterType, OutputParameterViewData> OutputParametersViewData { get; private set; }

    private void OnEnable()
    {
        InoutParametersData = _inoutParametersData?.ToDictionary(data => data.TaskNumber, data => data);
        InputParametersViewData = _inputParametersViewData?.ToDictionary(data => data.InputParameterType, data => data);
        OutputParametersViewData = _outputParametersViewData?.ToDictionary(data => data.OutputParameterType, data => data);
    }
}
