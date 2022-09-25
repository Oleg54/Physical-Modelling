using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class InputParameter : MonoBehaviour
{
    [SerializeField] private InputParameterType _inputParameterType;

    private TMP_InputField _textField;
    private TMP_Text _text;

    public InputParameterType InputParameterType => _inputParameterType;
    public float Value { get; private set; }
    public bool Interactable { get => _textField.interactable; set => _textField.interactable = value; }

    public event Action<InputParameter> OnParameterChange;

    private void Awake()
    {
        _textField = GetComponentInChildren<TMP_InputField>(true);
        _text = GetComponentInChildren<TMP_Text>(true);

        if (_textField is null)
            throw new NullReferenceException();

        _textField.onValueChanged.AddListener(OnInputChange);
        _text.text = GetComponentInParent<ParametersScreenBase>().Config.InputParametersViewData[_inputParameterType].ParameterLabel;
    }

    private void OnValidate()
    {
        gameObject.name = $"[INPUT] {_inputParameterType}";
    }

    private void OnInputChange(string value)
    {
        Value = 0f;

        if (float.TryParse(value.Replace(".", ","), out float num))
            Value = num;

        OnParameterChange?.Invoke(this);
    }

    public void Nullify()
    {
        _textField.text = "0,0";
    }
}
