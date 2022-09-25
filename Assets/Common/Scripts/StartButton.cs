using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button _button;
    private TMP_Text _text;

    public UnityAction<StartButton> OnActivityChange;

    public ActivityState ActivityState { get; private set; }

    private void Awake()
    {
        _button = GetComponentInChildren<Button>(true);
        _text = GetComponentInChildren<TMP_Text>(true);
        _button.onClick.AddListener(ButtonClick);
        
        ActivityState = ActivityState.Stopped;
    }

    private void ButtonClick()
    {
        ActivityState = ActivityState is ActivityState.Stopped ? ActivityState.Started : ActivityState.Stopped;
        RefreshText();

        OnActivityChange.Invoke(this);
    }

    private void RefreshText()
    {
        _text.text = ActivityState is ActivityState.Stopped ? "Старт" : "Стоп";
    }

    public void SetState(ActivityState activityState)
    {
        if (ActivityState == activityState)
            return;

        ActivityState = activityState;
        RefreshText();

        OnActivityChange.Invoke(this);
    }
}
