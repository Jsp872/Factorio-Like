using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private BuildingManager buildingManager;

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
        if (!context.performed || UIBlocker.IsOverUI) return;
        buildingManager.CreateBuilding();
    }

    public void RemoveBuilding(InputAction.CallbackContext context)
    {
        if (!context.performed || UIBlocker.IsOverUI) return;
        buildingManager.RemoveBuilding();
    }

    public void RotateBuilding(InputAction.CallbackContext context)
    {
        if (!context.performed || UIBlocker.IsOverUI) return;
        buildingManager.RotateBuilding();
    }

    private void Update()
    {
        playerMove.rb.linearVelocity = playerMove.direction * playerMove.moveSpeed;
    }
}