using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIBlocker : MonoBehaviour
{
    public static bool IsOverUI { get; private set; }

    public LayerMask uiBlockerLayer; // Layer "UIBlocker" à assigner dans l'inspecteur

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

            // Raycast sur les éléments UI qui bloquent
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = mousePos
            };

            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            IsOverUI = false;
            foreach (var r in results)
            {
                // Vérifie si l'objet est dans le Layer "UIBlocker"
                if (((1 << r.gameObject.layer) & uiBlockerLayer) != 0)
                {
                    IsOverUI = true;
                    break;
                }
            }
        }
    }
}