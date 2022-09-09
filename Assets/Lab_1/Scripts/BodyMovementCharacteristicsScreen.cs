using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BodyMovementCharacteristicsScreen : UIScreen
{
    [SerializeField] private TMP_InputField _speedXField;
    [SerializeField] private TMP_InputField _speedYField;
    [SerializeField] private TMP_InputField _startPositionXField;
    [SerializeField] private TMP_InputField _startPositionYField;
    [SerializeField] private TMP_InputField _accelerateXField;
    [SerializeField] private TMP_InputField _accelerateYField;

    [SerializeField] private TMP_Text _currentWayText;
    [SerializeField] private TMP_Text _currentTimeText;
    [SerializeField] private TMP_Text _currentPositionText;

    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _restartButton;

    private IReadOnlyList<TaskButton> _taskButtons;

    public event Action OnPauseStateChange;
    public event Action OnRestartButtonClick;

    public event Action<float> OnSpeedXFieldChange;
    public event Action<float> OnSpeedYFieldChange;
    public event Action<float> OnAccelerateXFieldChange;
    public event Action<float> OnAccelerateYFieldChange;
    public event Action<float> OnStartPositionXFieldChange;
    public event Action<float> OnStartPositionYFieldChange;

    public event Action<TaskNumber> OnTaskButtonClick;

    public TMP_Text CurrentWayText => _currentWayText;
    public TMP_Text CurrentTimeText => _currentTimeText;

    private void Awake()
    {
        _pauseButton.onClick.AddListener(OnPauseButtonClick);
        _restartButton.onClick.AddListener(RestartButtonClick);

        _taskButtons = GetComponentsInChildren<TaskButton>(true);
        foreach (TaskButton taskButton in _taskButtons)
            taskButton.OnButtonClick += TaskButtonClick;

        _taskButtons[0].SetViewActivity(true);

        _speedXField.onDeselect.AddListener(value => TryCallFieldDeselectEvent(_speedXField, OnSpeedXFieldChange));
        _speedYField.onDeselect.AddListener(value => TryCallFieldDeselectEvent(_speedYField, OnSpeedYFieldChange));

        _accelerateXField.onDeselect.AddListener(value => TryCallFieldDeselectEvent(_accelerateXField, OnAccelerateXFieldChange));
        _accelerateYField.onDeselect.AddListener(value => TryCallFieldDeselectEvent(_accelerateYField, OnAccelerateYFieldChange));

        _startPositionXField.onDeselect.AddListener(value => TryCallFieldDeselectEvent(_startPositionXField, OnStartPositionXFieldChange));
        _startPositionYField.onDeselect.AddListener(value => TryCallFieldDeselectEvent(_startPositionYField, OnStartPositionYFieldChange));

        RefreshFieldsActivity(TaskNumber.Number_1);
    }

    private void TryCallFieldDeselectEvent(TMP_InputField inputField, Action<float> action)
    {
        if (float.TryParse(inputField.text, out float result) || inputField.text == "")
            action(result);
        else
            inputField.text = $"";
    }

    private void RestartButtonClick()
    {
        if (_pauseButton.GetComponentInChildren<TMP_Text>(true).text == "Стоп")
            OnPauseButtonClick();

        OnRestartButtonClick?.Invoke();

        foreach (TMP_InputField inputField in GetComponentsInChildren<TMP_InputField>(true))
            inputField.text = "";
    }

    private void OnPauseButtonClick()
    {
        TMP_Text text = _pauseButton.GetComponentInChildren<TMP_Text>(true);

        if (text.text is "Пуск")
            text.text = "Стоп";
        else
            text.text = "Пуск";

        OnPauseStateChange?.Invoke();
    }

    private void TaskButtonClick(TaskNumber taskNumber)
    {
        foreach (TaskButton taskButton in _taskButtons)
            taskButton.SetViewActivity(taskButton.TaskNumber == taskNumber);

        RefreshFieldsActivity(taskNumber);

        OnTaskButtonClick?.Invoke(taskNumber);
    }

    private void RefreshFieldsActivity(TaskNumber taskNumber)
    {
        _speedYField.transform.parent.gameObject.SetActive(taskNumber is TaskNumber.Number_4);
        _startPositionXField.transform.parent.gameObject.SetActive(taskNumber > TaskNumber.Number_1);
        _startPositionYField.transform.parent.gameObject.SetActive(taskNumber is TaskNumber.Number_4);
        _accelerateXField.transform.parent.gameObject.SetActive(taskNumber > TaskNumber.Number_2);
        _accelerateYField.transform.parent.gameObject.SetActive(taskNumber is TaskNumber.Number_4);
    }

    public void RefreshFieldsContent(IMovementInfo movementInfo)
    {
        _currentWayText.text = $"{movementInfo.Way}";
        _currentTimeText.text = $"{movementInfo.Time}";
        _currentPositionText.text = $"{movementInfo.Position}";
    }
}