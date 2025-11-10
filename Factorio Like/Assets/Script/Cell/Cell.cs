using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Cell
{
    public Vector3 Position;
    public GameObject Prefab;
    public bool haveAtom;
    public bool haveElectricity;
    public int electricitySources = 0;
    public bool isPurify;
    public int purifySources = 0;
    
    public List<RessourceList.Ressource> atoms = new List<RessourceList.Ressource>();

    public void Initialize(Vector3 position, GameObject values, RessourceList ressourceList)
    {
        this.Position = position;
        this.Prefab = values;
        this.atoms = new List<RessourceList.Ressource>();
        
        foreach (var a in ressourceList.ressources)
        {
            this.atoms.Add(new RessourceList.Ressource
            {
                name = a.name,
                color = a.color,
                prefab = a.prefab,
                active = false
            });
        }
    }



    public GameObject GetValue()
    {
        return Prefab;
    }

    public Vector3 GetPosition()
    {
        return Position;
    }

    public bool GetAtom()
    {
        return haveAtom = true;
    }

    public bool GetElectricity()
    {
        return haveElectricity = true;
    }

    public bool GetPurify()
    {
        return isPurify = true;
    }

    public void ChangeValue(GameObject newPrefab)
    {
        Prefab = newPrefab;
    }
}
