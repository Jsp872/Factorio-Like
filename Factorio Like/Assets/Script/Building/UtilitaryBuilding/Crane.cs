using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : UtilityBuildings
{
    [SerializeField] private GameObject grabZone;
    [SerializeField] private Vector2 grabZonePosition;
    [SerializeField] private Vector3 grabZoneSize = new Vector3(1f,1f,1f);
    [SerializeField] private float duration = 1f;
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private LayerMask atomLayer;
    [SerializeField] private LayerMask chestLayer;

    [HideInInspector] public RessourceType? selectedResourceType = null;

    private bool isGrabbing = false;
    private readonly List<GameObject> grabbedObjects = new();
    private readonly Collider[] hitsBuffer = new Collider[10];

    public override void Start()
    {
        base.Start();
        grabZone.transform.localPosition = grabZonePosition;
        StartCoroutine(CheckAtomsLoop());
    }

    private IEnumerator CheckAtomsLoop()
    {
        while (gameObject.activeInHierarchy)
        {
            if (isPlaced && !isGrabbing)
            {
                if (TryGrabFromChest()) 
                { 
                    yield return new WaitForSecondsRealtime(0.2f);
                    continue; 
                }

                DetectAtomsInZone();
                if (grabbedObjects.Count > 0)
                    GrabAtom(grabbedObjects[0]);
            }

            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    private bool TryGrabFromChest()
    {
        Collider[] chests = Physics.OverlapBox(grabZone.transform.position, grabZoneSize / 2f, grabZone.transform.rotation, chestLayer);
        foreach (Collider chestCol in chests)
        {
            if (chestCol.TryGetComponent(out Chest chest))
            {
                GameObject item = chest.GetAnyItem();
                if (item != null)
                {
                    GrabAtom(item);
                    return true;
                }
            }
        }
        return false;
    }

    private void DetectAtomsInZone()
    {
        grabbedObjects.Clear();
        int hitCount = Physics.OverlapBoxNonAlloc(grabZone.transform.position, grabZoneSize / 2f, hitsBuffer, grabZone.transform.rotation, atomLayer);
        for (int i = 0; i < hitCount; i++)
        {
            if (hitsBuffer[i].TryGetComponent(out Ressource atom) && !atom.isGrabbed)
                grabbedObjects.Add(hitsBuffer[i].gameObject);
        }
    }

    private void GrabAtom(GameObject atom)
    {
        if (!atom.TryGetComponent(out Ressource res) || res.isGrabbed) return;
        if (selectedResourceType != null && res.type != selectedResourceType) return;

        res.isGrabbed = true;
        isGrabbing = true;

        atom.transform.SetParent(grabZone.transform);
        atom.transform.localPosition = Vector3.zero;
        atom.transform.localRotation = Quaternion.identity;

        if (atom.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (atom.TryGetComponent(out Collider col))
            col.enabled = false;

        StartCoroutine(MoveAtom(atom, res));
    }

    private IEnumerator MoveAtom(GameObject atom, Ressource res)
    {
        Vector3 startPos = grabZone.transform.localPosition;
        Vector3 endPos = startPos + Vector3.right * moveDistance;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            grabZone.transform.localPosition = Vector3.Lerp(startPos, endPos, Mathf.Clamp01(elapsed / duration));
            yield return null;
        }

        ReleaseAtom(atom, startPos, res);
    }

    private void ReleaseAtom(GameObject atom, Vector3 startPos, Ressource res)
    {
        if (atom.TryGetComponent(out Collider col))
            col.enabled = true;

        res.isGrabbed = false;
        atom.transform.SetParent(null);
        grabZone.transform.localPosition = startPos;
        isGrabbing = false;
        grabbedObjects.Remove(atom);
    }

    private void OnDrawGizmosSelected()
    {
        if (grabZone == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.matrix = grabZone.transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, grabZoneSize);
    }
}
