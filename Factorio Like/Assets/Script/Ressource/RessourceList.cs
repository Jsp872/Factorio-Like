using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "RessourceList", menuName = "Data/Ressource List")]
public class RessourceList : ScriptableObject
{
    [System.Serializable]
    public class Ressource
    {
        public string name;
        public bool active;
        public Color color = Color.white;
        public GameObject prefab;
        public Material material;
    }

    [FormerlySerializedAs("atoms")] public List<Ressource> ressources = new List<Ressource>();
}