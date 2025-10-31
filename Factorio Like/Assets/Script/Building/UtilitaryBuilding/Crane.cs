using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : UtilityBuildings
{
    [Header("Crane Settings")]
    [SerializeField] private GameObject grabZone;
    [SerializeField] private Vector2 grabZonePosition;
    [SerializeField] private Vector3 grabZoneSize = new Vector3(1f, 1f, 1f);
    [SerializeField] private float duration = 1f;
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private LayerMask atomLayer;
    [SerializeField] private LayerMask chestLayer; // ✅ pour détecter les coffres

    private bool isGrabbing = false;
    private readonly List<GameObject> grabbedObjects = new();
    private readonly Collider[] hitsBuffer = new Collider[10];

    private void Start()
    {
        grabZone.transform.localPosition = grabZonePosition;
    }

    private void OnEnable()
    {
        StartCoroutine(CheckAtomsLoop());
    }

    private IEnumerator CheckAtomsLoop()
    {
        while (gameObject.activeInHierarchy)
        {
            if (isPlaced && !isGrabbing)
            {
                // 1️⃣ Vérifie d'abord s’il y a un coffre
                if (TryGrabFromChest())
                {
                    yield return new WaitForSecondsRealtime(0.2f);
                    continue;
                }

                // 2️⃣ Sinon, détecte les atomes normaux
                DetectAtomsInZone();

                if (grabbedObjects.Count > 0)
                    GrabAtom(grabbedObjects[0]);
            }

            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    // 🔸 Cherche un coffre à portée et tente de récupérer un objet
    private bool TryGrabFromChest()
    {
        Collider[] chests = Physics.OverlapBox(
            grabZone.transform.position,
            grabZoneSize / 2f,
            grabZone.transform.rotation,
            chestLayer
        );

        foreach (Collider chestCol in chests)
        {
            if (chestCol.TryGetComponent(out Chest chest))
            {

                // Exemple : on demande un "Atom" générique (ou selon ton système)
                GameObject item = chest.GetAnyItem();

                if (item != null)
                {
                    GrabAtom(item);
                    return true; // ✅ on a trouvé un coffre et pris un item
                }
            }
        }

        return false;
    }

    private void DetectAtomsInZone()
    {
        grabbedObjects.Clear();

        int hitCount = Physics.OverlapBoxNonAlloc(
            grabZone.transform.position,
            grabZoneSize / 2f,
            hitsBuffer,
            grabZone.transform.rotation,
            atomLayer
        );

        for (int i = 0; i < hitCount; i++)
        {
            Collider hit = hitsBuffer[i];
            if (hit.TryGetComponent(out Atom atom) && !atom.isGrabbed)
                grabbedObjects.Add(hit.gameObject);
        }
    }

    private void GrabAtom(GameObject atom)
    {
        Atom atomComponent = atom.GetComponent<Atom>();
        if (atomComponent != null && atomComponent.isGrabbed) return;

        if (atomComponent != null)
            atomComponent.isGrabbed = true;

        isGrabbing = true;

        atom.transform.SetParent(grabZone.transform);
        atom.transform.localPosition = Vector3.zero;
        atom.transform.localRotation = Quaternion.identity;

        if (atom.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (atom.TryGetComponent(out BoxCollider box))
            box.enabled = false;

        StartCoroutine(MoveAtom(atom, atomComponent));
    }

    private IEnumerator MoveAtom(GameObject atom, Atom atomData)
    {
        Vector3 startPos = grabZone.transform.localPosition;
        Vector3 endPos = startPos + Vector3.right * moveDistance;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            grabZone.transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        ReleaseAtom(atom, startPos, atomData);
    }

    private void ReleaseAtom(GameObject atom, Vector3 startPos, Atom atomData)
    {
        if (atom.TryGetComponent(out BoxCollider box))
            box.enabled = true;

        if (atomData != null)
            atomData.isGrabbed = false;

        atom.transform.SetParent(null);
        grabZone.transform.localPosition = startPos;
        isGrabbing = false;
        grabbedObjects.Remove(atom);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = grabZone.transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, grabZoneSize);
    }
}
