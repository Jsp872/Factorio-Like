using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public Vector3 direction;
    public Rigidbody rb;
    public float moveSpeed ;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>().normalized;
    }
}
