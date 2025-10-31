using UnityEngine;

[System.Serializable]
public class Building : MonoBehaviour
{
    [SerializeField] protected int Health;
    public int cost;
    public bool isPlaced { get; private set; } = false;

    public virtual void Place()
    {
        isPlaced = true;
    }
}
