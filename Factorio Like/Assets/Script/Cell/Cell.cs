using UnityEngine;

[System.Serializable]
public class Cell
{
    public Vector2 Position;
    public GameObject Prefab;

    public void Initialize(Vector2 position, GameObject values)
    {
        this.Position = position;
        this.Prefab = values;
    }

    public GameObject GetValue()
    {
        return Prefab;
    }

    public Vector2 GetPosition()
    {
        return Position;
    }

    public void ChangeValue(GameObject newPrefab)
    {
        Prefab = newPrefab;
    }
}
