using System;
using UnityEngine;

public enum RessourceType
{
    fe,
    h,
    h2,
    fepur,
    ironplate
}

public class Ressource : MonoBehaviour
{
    public RessourceType type;
    public bool isGrabbed;
}

