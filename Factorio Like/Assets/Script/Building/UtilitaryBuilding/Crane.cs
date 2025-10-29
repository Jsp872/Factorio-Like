using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : UtilityBuildings
{
    [SerializeField] private GameObject grabZone;
    [SerializeField] private Vector2 grabZonePosition;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float moveDistance = 1f; 
    private bool isGrabbing = false;
    private List<GameObject> grabbedObjects = new List<GameObject>();

    private void Start()
    {
        grabZone.transform.localPosition = grabZonePosition;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 6 || grabbedObjects.Contains(other.gameObject)) return;
        Atom atom = other.gameObject.GetComponent<Atom>();
        if (atom.isGrabbed) return;
        grabbedObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 6 || grabbedObjects.Contains(other.gameObject)) return;
        grabbedObjects.Remove(other.gameObject);
    }

    private void Update()
    {
        if (grabbedObjects.Count == 0 || isGrabbing) return;
        
        GrabAtom(grabbedObjects[0]);
    }

    private void GrabAtom(GameObject atom)
    {
        Atom atomComponent = atom.GetComponent<Atom>();
        
        if (atomComponent.isGrabbed) return;
        
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
        atomData.isGrabbed = false;
        atom.transform.SetParent(null);
        grabZone.transform.localPosition = startPos;
        isGrabbing = false;
        grabbedObjects.Remove(atom);
    }
    
    
}
