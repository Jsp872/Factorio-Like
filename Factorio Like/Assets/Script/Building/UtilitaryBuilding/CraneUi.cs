using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CraneUi : MonoBehaviour
{
    private Crane crane;
    
    [SerializeField] private GameObject cranePanelUi;

    private void Start()
    {
        crane = GetComponent<Crane>();
    }
    
    public void ChangeAtomToSelected(GameObject atom)
    {
        Debug.Log("a");
        if (!crane.isPlaced) return;
        Debug.Log("b");
        if (atom.TryGetComponent(out Ressource res))
        {
            Debug.Log("c");
            crane.selectedResourceType = res.type;
        }
        else crane.selectedResourceType = null;
    }

    public void ChangeStatusOfPanel()
    {
        if (!crane.isPlaced) return;
        cranePanelUi.SetActive(!cranePanelUi.activeSelf);
    }
}
