using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIBlocker : MonoBehaviour
{
    public static bool IsOverUI { get; private set; }

    void Update()
    {
        if (EventSystem.current == null)
        {
            IsOverUI = false;
            return;
        }
        
        if (Mouse.current != null)
        {
            IsOverUI = EventSystem.current.IsPointerOverGameObject(Mouse.current.deviceId);
            return;
        }
        
        IsOverUI = EventSystem.current.IsPointerOverGameObject();
    }
}