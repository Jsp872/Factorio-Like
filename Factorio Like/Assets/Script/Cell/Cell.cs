using UnityEngine;

[System.Serializable]
public class Cell
{
    public Vector3 Position;
    public GameObject Prefab;
    public bool haveAtome;

    public void Initialize(Vector3 position, GameObject values)
    {
        this.Position = position;
        this.Prefab = values;
    }

    public GameObject GetValue()
    {
        return Prefab;
    }

    public Vector3 GetPosition()
    {
        return Position;
    }

    public bool GetAtome()
    {
        return haveAtome = true;
    }

    public void ChangeValue(GameObject newPrefab)
    {
        Prefab = newPrefab;
    }
}
