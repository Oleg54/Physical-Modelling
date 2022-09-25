using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BodyRotationScreen : ParametersScreenBase
{
    private StartButton _startButton;
    private TaskButton[] _taskButtons;
    private List<InputParameter> _inputParameters;
    private List<OutputParameter> _outputParameters;

    private MovementBody _movementBody;
    private AngularMovementBehaviour _angularMovementBehaviour;

    private void Start()
    {
        _movementBody = FindObjectOfType<MovementBody>(true);

        _startButton = GetComponentInChildren<StartButton>(true);
        _taskButtons = GetComponentsInChildren<TaskButton>(true);
        _inputParameters = GetComponentsInChildren<InputParameter>(true).ToList();
        _outputParameters = GetComponentsInChildren<OutputParameter>(true).ToList();

        _startButton.OnActivityChange += OnStartButtonClick;

        foreach (TaskButton taskButton in _taskButtons)
            taskButton.OnButtonClick += SetTask;

        SetTask(TaskNumber.Number_1);
    }

    private void OnStartButtonClick(StartButton startButton)
    {
        if (startButton.ActivityState is ActivityState.Stopped)
        {
            SetInputActive(true);
            NullifyAll();
        }
        else
        {
            SetInputActive(false);
            _angularMovementBehaviour = new AngularMovementBehaviour(
                _inputParameters.Find(param => param.InputParameterType is InputParameterType.Circle_Radius).Value,
                _inputParameters.Find(param => param.InputParameterType is InputParameterType.Rotation_Frequency).Value);
        }
    }

    private void SetInputActive(bool value)
    {
        foreach (InputParameter inputParameter in _inputParameters)
            inputParameter.Interactable = value;
    }

    private void FixedUpdate()
    {
        if (_startButton.ActivityState is ActivityState.Stopped)
            return;

        _angularMovementBehaviour.Tick(Time.fixedDeltaTime);
        _movementBody.SetPosition(_angularMovementBehaviour.Position);
        UpdateView();
    }

    private void SetTask(TaskNumber taskNumber)
    {
        InoutParametersData data = Config.InoutParametersData[taskNumber];

        foreach (TaskButton taskButton in _taskButtons)
            taskButton.SetViewActivity(taskButton.TaskNumber == taskNumber);

        foreach (InputParameter inputParameter in _inputParameters)
            inputParameter.gameObject.SetActive(data.InputParameterTypes.Contains(inputParameter.InputParameterType));

        foreach (OutputParameter outputParameter in _outputParameters)
            outputParameter.gameObject.SetActive(data.OutputParameterTypes.Contains(outputParameter.OutputParameterType));

        NullifyAll();
    }

    private void NullifyAll()
    {
        foreach (InputParameter inputParameter in _inputParameters)
            inputParameter.Nullify();

        foreach (OutputParameter outputParameter in _outputParameters)
            outputParameter.SetOutput(string.Empty);

        _startButton.SetState(ActivityState.Stopped);

        _angularMovementBehaviour = new AngularMovementBehaviour(
            _inputParameters.Find(param => param.InputParameterType is InputParameterType.Circle_Radius).Value,
            _inputParameters.Find(param => param.InputParameterType is InputParameterType.Rotation_Frequency).Value);

        UpdateView();
    }

    private void UpdateView()
    {
        foreach (OutputParameter outputParameter in _outputParameters)
            outputParameter.SetOutput(_angularMovementBehaviour.GetParameter(outputParameter.OutputParameterType));
    }
}
