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

    private float swipeDistance = 100f;

    protected event EventHandler OnLeftSwipe;
    protected event EventHandler OnUpSwipe;
    protected event EventHandler OnRightSwipe;

    protected event EventHandler OnFirstFingerOnRightPart;
    protected event EventHandler OnFirstFingerOnLeftPart;
    protected event EventHandler OnFirstFingerCanceled;

    protected event EventHandler OnSecondFingerOnRightPart;
    protected event EventHandler OnSecondFingerOnLeftPart;
    protected event EventHandler OnSecondFingerCanceled;

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
            _secondFingerStartPosition = finger.position;

            if (finger.position.x > Screen.width * .5f)
            {
                OnSecondFingerOnRightPart?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnSecondFingerOnLeftPart?.Invoke(this, EventArgs.Empty);
            }
        }

        if (finger.phase == TouchPhase.Ended)
        {
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

            OnSecondFingerCanceled?.Invoke(this, EventArgs.Empty);
        }
    }

    private void FirstFinger_performed(InputAction.CallbackContext context)
    {
        TouchState finger = context.ReadValue<TouchState>();
        if (finger.phase == TouchPhase.Began)
        {
            _firstFingerStartPosition = finger.position;

            if (finger.position.x > Screen.width * .5f)
            {
                OnFirstFingerOnRightPart?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnFirstFingerOnLeftPart?.Invoke(this, EventArgs.Empty);
            }
        }

        if (finger.phase == TouchPhase.Ended)
        {
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

            OnFirstFingerCanceled?.Invoke(this, EventArgs.Empty);
        }
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
