using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public InputActionReference inputActionMove;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public Vector3 direction;
    
    private void FixedUpdate()
    {
        direction = inputActionMove.action.ReadValue<Vector2>().normalized;

        Vector3 move = new Vector3(direction.x,0, direction.y) * 5 * Time.deltaTime;
        transform.position += move;
        
    }

}
