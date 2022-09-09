using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TaskButton : MonoBehaviour
{
    [SerializeField] private TaskNumber _taskNumber;
    [SerializeField] private Color _enableColor;
    [SerializeField] private Color _disableColor;

    public TaskNumber TaskNumber => _taskNumber;

    private Button _button;
    private Image _image;

    public UnityAction<TaskNumber> OnButtonClick;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();

        _button.onClick.AddListener(() => OnButtonClick(_taskNumber));

        SetViewActivity(false);
    }

    public void SetViewActivity(bool value)
    {
        _image.color = value ? _enableColor : _disableColor;
    }
}
