using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDictionary : MonoBehaviour
{

    [System.Serializable]
    public struct ResourceEntry
    {
        [SerializeField]
        public ResourceType type;
        [SerializeField]
        public Sprite sprite;
        [SerializeField]
        public Material material;
    }

    public List<ResourceEntry> dictionary;

    public static ResourceDictionary Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }


    public Sprite GetSprite(ResourceType type)
    {
        foreach (var dict in dictionary)
        {
            if (dict.type == type)
            {
                return dict.sprite;
            }
        }
        return null;
    }

    public Material GetMaterial(ResourceType type)
    {
        foreach (var dict in dictionary)
        {
            if (dict.type == type)
            {
                return dict.material;
            }
        }
        return null;
    }
}
