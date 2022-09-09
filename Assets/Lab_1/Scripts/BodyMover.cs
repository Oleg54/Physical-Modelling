using System;
using UnityEngine;

public class BodyMover : MonoBehaviour
{
    private BodyMovementCharacteristicsScreen _screen;
    private MovementBody _movementBody;

    private MovementBehaviour _movementBehaviour;

    private bool _isPause = true;
    private TaskNumber _currentTaskNumber;

    private void Awake()
    {
        _movementBody = FindObjectOfType<MovementBody>(true);
        _movementBehaviour = new MovementBehaviour(Vector3.zero, Vector3.zero, Vector3.zero);

        _screen = UIScreenRepository.GetScreen<BodyMovementCharacteristicsScreen>();
        _screen.OnPauseStateChange += OnPauseStateChange;
        _screen.OnTaskButtonClick += OnTaskNumberChange;
        _screen.OnRestartButtonClick += OnRestartButtonClick;

        _screen.OnSpeedXFieldChange += value => 
            _movementBehaviour.SetVelocity(new Vector3(value, _movementBehaviour.Velocity.y));
        _screen.OnSpeedYFieldChange += value =>
            _movementBehaviour.SetVelocity(new Vector3(_movementBehaviour.Velocity.x, value));

        _screen.OnAccelerateXFieldChange += value =>
            _movementBehaviour.SetAccelerate(new Vector3(value, _movementBehaviour.Accelerate.y));
        _screen.OnAccelerateYFieldChange += value =>
            _movementBehaviour.SetAccelerate(new Vector3(_movementBehaviour.Accelerate.x, value));

        _screen.OnStartPositionXFieldChange += value =>
            _movementBehaviour.SetStartPosition(new Vector3(value, _movementBehaviour.StartPosition.y));
        _screen.OnStartPositionYFieldChange += value =>
            _movementBehaviour.SetStartPosition(new Vector3(_movementBehaviour.StartPosition.x, value));
    }

    private void OnRestartButtonClick()
    {
        OnTaskNumberChange(TaskNumber.Number_1);
        _movementBody.transform.position = Vector3.zero;
    }

    private void OnPauseStateChange()
    {
        _isPause = !_isPause;
    }

    private void OnTaskNumberChange(TaskNumber taskNumber)
    {
        _currentTaskNumber = taskNumber;
        _movementBehaviour = new MovementBehaviour(Vector3.zero, Vector3.zero, Vector3.zero);
    }

    private void FixedUpdate()
    {
        _screen.RefreshFieldsContent(_movementBehaviour);

        if (_isPause)
            return;

        _movementBody.transform.position += _movementBehaviour.GetMoveOffcet(Time.fixedDeltaTime);
    }
}
