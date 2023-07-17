using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInputs _playerControls;

    private Vector2 _firstFingerStartPosition;
    private Vector2 _secondFingerStartPosition;

    private int _secondFingerLastBeganTouchID;
    private int _secondFingerLastEndedTouchID;

    private int _firstFingerLastBeganTouchID;
    private int _firstFingerLastEndedTouchID;

    [Header("Player Inputs")]
    [SerializeField] private float swipeDistance = 100f;

    protected event EventHandler OnLeftSwipe;
    protected event EventHandler OnUpSwipe;
    protected event EventHandler OnRightSwipe;

    protected virtual void Awake()
    {
        _playerControls = new PlayerInputs();

        _playerControls.Map.FirstFinger.performed += FirstFinger_performed;
        _playerControls.Map.SecondFinger.performed += SecondFinger_performed;
    }

    private void SecondFinger_performed(InputAction.CallbackContext context)
    {
        TouchState finger = context.ReadValue<TouchState>();
        if (finger.phase == TouchPhase.Began)
        {
            if (_secondFingerLastBeganTouchID == finger.touchId)
                return;
            _secondFingerLastBeganTouchID = finger.touchId;

            _secondFingerStartPosition = finger.position;
        }

        if(finger.phase == TouchPhase.Moved)
        {
            if (_secondFingerLastEndedTouchID == finger.touchId)
                return;

            Vector2 fingerTrajectory = finger.position - _secondFingerStartPosition;
            float leftDot = Vector2.Dot(fingerTrajectory, -Vector2.right);
            float upDot = Vector2.Dot(fingerTrajectory, Vector2.up);
            float rightDot = Vector2.Dot(fingerTrajectory, Vector2.right);

            if (leftDot >= upDot + swipeDistance && leftDot >= rightDot + swipeDistance)
            {
                OnLeftSwipe?.Invoke(this, EventArgs.Empty);
                _secondFingerLastEndedTouchID = finger.touchId;
            }
            if (rightDot >= upDot + swipeDistance && rightDot >= leftDot + swipeDistance)
            {
                OnRightSwipe?.Invoke(this, EventArgs.Empty);
                _secondFingerLastEndedTouchID = finger.touchId;
            }
            if (upDot >= leftDot + swipeDistance && upDot >= rightDot + swipeDistance)
            {
                OnUpSwipe?.Invoke(this, EventArgs.Empty);
                _secondFingerLastEndedTouchID = finger.touchId;
            }
        }

        /*if (finger.phase == TouchPhase.Ended)
        {
            if (_secondFingerLastEndedTouchID == finger.touchId)
                return;
            _secondFingerLastEndedTouchID = finger.touchId;

            Vector2 fingerTrajectory = finger.position - _secondFingerStartPosition;
            float leftDot = Vector2.Dot(fingerTrajectory, -Vector2.right);
            float upDot = Vector2.Dot(fingerTrajectory, Vector2.up);
            float rightDot = Vector2.Dot(fingerTrajectory, Vector2.right);

            if (leftDot >= upDot + swipeDistance && leftDot >= rightDot + swipeDistance)
            {
                OnLeftSwipe?.Invoke(this, EventArgs.Empty);
            }
            if (rightDot >= upDot + swipeDistance && rightDot >= leftDot + swipeDistance)
            {
                OnRightSwipe?.Invoke(this, EventArgs.Empty);
            }
            if (upDot >= leftDot + swipeDistance && upDot >= rightDot + swipeDistance)
            {
                OnUpSwipe?.Invoke(this, EventArgs.Empty);
            }
        }*/
    }

    private void FirstFinger_performed(InputAction.CallbackContext context)
    {
        TouchState finger = context.ReadValue<TouchState>();
        if (finger.phase == TouchPhase.Began)
        {
            if (_firstFingerLastBeganTouchID == finger.touchId)
                return;
            _firstFingerLastBeganTouchID = finger.touchId;

            _firstFingerStartPosition = finger.position;
        }

        if(finger.phase == TouchPhase.Moved)
        {
            if (_firstFingerLastEndedTouchID == finger.touchId)
                return;

            Vector2 fingerTrajectory = finger.position - _firstFingerStartPosition;
            float leftDot = Vector2.Dot(fingerTrajectory, -Vector2.right);
            float upDot = Vector2.Dot(fingerTrajectory, Vector2.up);
            float rightDot = Vector2.Dot(fingerTrajectory, Vector2.right);

            if (leftDot >= upDot + swipeDistance && leftDot >= rightDot + swipeDistance)
            {
                OnLeftSwipe?.Invoke(this, EventArgs.Empty);
                _firstFingerLastEndedTouchID = finger.touchId;
            }
            if (rightDot >= upDot + swipeDistance && rightDot >= leftDot + swipeDistance)
            {
                OnRightSwipe?.Invoke(this, EventArgs.Empty);
                _firstFingerLastEndedTouchID = finger.touchId;
            }
            if (upDot >= leftDot + swipeDistance && upDot >= rightDot + swipeDistance)
            {
                OnUpSwipe?.Invoke(this, EventArgs.Empty);
                _firstFingerLastEndedTouchID = finger.touchId;
            }
        }

        /*if (finger.phase == TouchPhase.Ended)
        {
            if (_firstFingerLastEndedTouchID == finger.touchId)
                return;
            _firstFingerLastEndedTouchID = finger.touchId;

            Vector2 fingerTrajectory = finger.position - _firstFingerStartPosition;
            float leftDot = Vector2.Dot(fingerTrajectory, -Vector2.right);
            float upDot = Vector2.Dot(fingerTrajectory, Vector2.up);
            float rightDot = Vector2.Dot(fingerTrajectory, Vector2.right);

            if (leftDot >= upDot + swipeDistance && leftDot >= rightDot + swipeDistance)
            {
                OnLeftSwipe?.Invoke(this, EventArgs.Empty);
            }
            if (rightDot >= upDot + swipeDistance && rightDot >= leftDot + swipeDistance)
            {
                OnRightSwipe?.Invoke(this, EventArgs.Empty);
            }
            if (upDot >= leftDot + swipeDistance && upDot >= rightDot + swipeDistance)
            {
                OnUpSwipe?.Invoke(this, EventArgs.Empty);
            }
        }*/
    }

    protected virtual void OnEnable()
    {
        _playerControls.Enable();
    }

    protected virtual void OnDisable()
    {
        _playerControls.Disable();
    }
}
