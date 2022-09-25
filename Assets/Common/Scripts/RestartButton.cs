using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    private Button _button;

    public UnityAction<RestartButton> OnClick;

    private void Awake()
    {
        _button = GetComponentInChildren<Button>(true);
        _button.onClick.AddListener(ButtonClick);
    }

    private void ButtonClick()
    {
        OnClick.Invoke(this);
    }
}
