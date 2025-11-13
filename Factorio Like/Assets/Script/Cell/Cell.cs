using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    public Vector3 Position;
    public GameObject Prefab;
    public bool haveAtom;
    public bool haveElectricity;
    public int electricitySources;
    public bool isPurify;
    public int purifySources;
    public List<RessourceList.Ressource> atoms = new List<RessourceList.Ressource>();

    public void Initialize(Vector3 position, GameObject values, RessourceList ressourceList)
    {
        Position = position;
        Prefab = values;
        atoms = new List<RessourceList.Ressource>();

        foreach (var a in ressourceList.ressources)
        {
            atoms.Add(new RessourceList.Ressource
            {
                name = a.name,
                color = a.color,
                prefab = a.prefab,
                active = false
            });
        }
    }

    public GameObject GetValue() => Prefab;
    public Vector3 GetPosition() => Position;
    public bool GetAtom() { haveAtom = true; return true; }
    public bool GetElectricity() { haveElectricity = true; return true; }
    public bool GetPurify() { isPurify = true; return true; }
    public void ChangeValue(GameObject newPrefab) => Prefab = newPrefab;
}