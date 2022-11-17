using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ParametersScreenBase : UIScreen
{
    [SerializeField] private InoutParametersViewConfig _config;

    protected StartButton _startButton;
    private TaskButton[] _taskButtons;
    private List<InputParameter> _inputParameters;
    private List<OutputParameter> _outputParameters;

    protected MovementBody _movementBody;

    protected abstract PhysicBehaviourBase _physicBehaviourBase { get; }
    protected ActivityState _activityState => _startButton.ActivityState;

    public TaskNumber CurrentTaskNumber { get; private set; }
    public InoutParametersViewConfig Config => _config;

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
        InitCallback();
    }

    protected virtual void InitCallback() { }

    private void OnStartButtonClick(StartButton startButton)
    {
        if (startButton.ActivityState is ActivityState.Started)
        {
            UpdatePhysicBehaviour();
            SubscribeOnBehaviourUpdate();
            SetInputActive(false);
            //NullifyAll();
        }
        else if (startButton.ActivityState is ActivityState.Stopped)
        {
            SetInputActive(true);
        }
    }

    private void SetInputActive(bool value)
    {
        foreach (InputParameter inputParameter in _inputParameters)
            inputParameter.Interactable = value;
    }

    private void SetTask(TaskNumber taskNumber)
    {
        CurrentTaskNumber = taskNumber;

        InoutParametersData data = Config.InoutParametersData[taskNumber];

        foreach (TaskButton taskButton in _taskButtons)
            taskButton.SetViewActivity(taskButton.TaskNumber == taskNumber);

        foreach (InputParameter inputParameter in _inputParameters)
            inputParameter.gameObject.SetActive(data.InputParameterTypes.Contains(inputParameter.InputParameterType));

        foreach (OutputParameter outputParameter in _outputParameters)
            outputParameter.gameObject.SetActive(data.OutputParameterTypes.Contains(outputParameter.OutputParameterType));

        OnTaskSet(taskNumber);
        NullifyAll();
    }

    protected virtual void OnTaskSet(TaskNumber taskNumber) { }

    private void NullifyAll()
    {
        foreach (InputParameter inputParameter in _inputParameters)
            inputParameter.Nullify();

        foreach (OutputParameter outputParameter in _outputParameters)
            outputParameter.SetOutput(string.Empty);

        UpdateView();
    }

    protected abstract void UpdatePhysicBehaviour();
    private void SubscribeOnBehaviourUpdate()
    {
        _physicBehaviourBase.OnUpdate += UpdateView;
    }

    protected void UpdateView()
    {
        foreach (OutputParameter outputParameter in _outputParameters)
            outputParameter.SetOutput(_physicBehaviourBase?.GetParameter(outputParameter.OutputParameterType));
    }

    protected float FindInputParameter(InputParameterType inputParameterType) =>
        _inputParameters.Find(param => param.InputParameterType == inputParameterType).Value;
}
