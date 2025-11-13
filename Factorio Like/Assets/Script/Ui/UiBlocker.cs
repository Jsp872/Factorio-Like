using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class UIBlocker : MonoBehaviour
{
    public static bool IsOverUI { get; private set; }
    public LayerMask uiBlockerLayer;

    void Update()
    {
        if (EventSystem.current == null)
        {
            IsOverUI = false;
            return;
        }

        if (Mouse.current != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            PointerEventData pointerData = new(EventSystem.current) { position = mousePos };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            IsOverUI = false;
            foreach (var r in results)
            {
                if (((1 << r.gameObject.layer) & uiBlockerLayer) != 0)
                {
                    IsOverUI = true;
                    break;
                }
            }
        }
    }
}