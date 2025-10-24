using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public Vector3 direction;
    public Rigidbody2D rb2d;
    public float moveSpeed ;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>().normalized;
    }
}
