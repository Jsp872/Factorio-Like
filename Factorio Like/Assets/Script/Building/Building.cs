using UnityEngine;

[System.Serializable]
public class Building : MonoBehaviour
{
    [SerializeField] protected int Health;
    [SerializeField] protected bool isWorking;
    public int cost;
}
