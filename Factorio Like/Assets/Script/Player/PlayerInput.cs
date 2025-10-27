using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private GridManager gridManager;

    private bool leftClickHeld;

    public void Move(InputAction.CallbackContext context)
    {
        playerMove.Move(context);
    }

    public void Pause(InputAction.CallbackContext context)
    {
        pauseMenu.Pause();
    }

    public void CreateBuilding(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!UIBlocker.IsOverUI)
        {
            gridManager.CreateBuilding();
        }
    }

    private void Update()
    {
        playerMove.rb2d.linearVelocity = playerMove.direction * playerMove.moveSpeed;
    }
}