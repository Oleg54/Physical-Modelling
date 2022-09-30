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
        RefreshText();
    }

    private void ButtonClick()
    {
        ActivityState = ActivityState is ActivityState.Started ? ActivityState.Stopped : ActivityState.Started;
        RefreshText();

        OnActivityChange.Invoke(this);
    }

    private void RefreshText()
    {
        _text.text = ActivityState is ActivityState.Started ? "Стоп" : "Старт";
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
