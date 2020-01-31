using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FixerController : MonoBehaviour
{
    private Vector2 _inputDirection;

    [SerializeField]
    private float _baseMovementSpeed;

    public float MovementSpeed
    {
        get
        {
            return _baseMovementSpeed;
        }
    }

    void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;

        var velocity = new Vector3(_inputDirection.x, 0f, _inputDirection.y) * MovementSpeed;

        transform.Translate(velocity * dt);
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        _inputDirection = ctx.ReadValue<Vector2>();
        Debug.Log(_inputDirection);
    }

    public void OnRepair(InputAction.CallbackContext ctx)
    {
        Debug.Log("OnRepair");
    }
}
