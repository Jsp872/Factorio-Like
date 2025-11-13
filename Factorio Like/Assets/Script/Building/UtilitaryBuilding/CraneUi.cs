using UnityEngine;

public class CraneUi : MonoBehaviour
{
    [SerializeField] private GameObject cranePanelUi;

    private Crane crane;

    private void Start()
    {
        crane = GetComponent<Crane>();
    }

    public void ChangeAtomToSelected(GameObject atom)
    {
        if (!crane.isPlaced || atom == null) return;

        if (atom.TryGetComponent(out Ressource res))
        {
            crane.selectedResourceType = res.type;
        }
        else
        {
            crane.selectedResourceType = null;
        }
    }

    public void TogglePanel()
    {
        if (!crane.isPlaced) return;
        cranePanelUi.SetActive(!cranePanelUi.activeSelf);
    }
}