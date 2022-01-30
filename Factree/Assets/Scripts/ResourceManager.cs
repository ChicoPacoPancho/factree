using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private bool victoryYet = false;

    private void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        // Do every second (approximately)
        if (Mathf.Round(Time.time) != Mathf.Round(Time.time - Time.deltaTime)) {
            
            GridManagement grid = FindObjectOfType<GridManagement>();

            bool containsAllGrass = true;

            // Tally up all used and produced resources
            for (int x = 0; x < grid.cityGrid.Width; x++)
            {
                for (int y = 0; y < grid.cityGrid.Height; y++)
                {
                    var obj = grid.cityGrid.GetGridObject(x, y);
                    if (obj != null && (GroundDictionary.Instance.GetTileType(obj.BaseTile) == BaseTileType.Asphalt || 
                        GroundDictionary.Instance.GetTileType(obj.BaseTile) == BaseTileType.Concrete ||
                        GroundDictionary.Instance.GetTileType(obj.BaseTile) == BaseTileType.Soil))
                    {
                        containsAllGrass = false;
                    }
                    if (obj != null && obj.PlantTile != null)
                    {
                        if (obj.PlantTile.CheckUpkeep())
                        {
                            obj.PlantTile.SubtractUpkeep();
                            obj.PlantTile.AddIncome();
                        } else
                        {
                            //Debug.Log("Not enough resources for upkeep!");
                        }
                    }
                }
            }

            Debug.Log(containsAllGrass);
            if (!victoryYet && containsAllGrass)
            {
                victoryYet = true;
                FindObjectOfType<VictoryScreen>().ShowVictory();
            }
        }
    }
    public void AddTimeResourceChange(ResourceType type, float resourceChangePerSecond, float totalSeconds)
    {
        StartCoroutine(ResourceChangeOverTime(type, resourceChangePerSecond, totalSeconds));
    }

    private IEnumerator ResourceChangeOverTime(ResourceType type, float amountPerSecond, float totalSeconds)
    {
        for (int i = 0; i < totalSeconds; i++)
        {
            AddResourceAmountByType(type, amountPerSecond);

            yield return new WaitForSeconds(1);
        }
    }


    public event EventHandler<ResourceChangedEventArgs> OnResourceChanged;
    public class ResourceChangedEventArgs : EventArgs
    {
        public ResourceType type;
        public float amountChanged;
        public float newAmount;
    }

    private float m_waterAmount = 30;
    public float waterAmount
    {
        get { return m_waterAmount; }
        set {
            float amountChanged = value - m_waterAmount;
            m_waterAmount = value;

            if (OnResourceChanged != null)
                OnResourceChanged(this, new ResourceChangedEventArgs { type = ResourceType.Water, amountChanged = amountChanged, newAmount = value });
        }
    }
    private float m_nitrogenAmount = 30;
    public float nitrogenAmount
    {
        get { return m_nitrogenAmount; }
        set
        {
            float amountChanged = value - m_nitrogenAmount;
            m_nitrogenAmount = value;

            if (OnResourceChanged != null)
                OnResourceChanged(this, new ResourceChangedEventArgs { type = ResourceType.Nitrogen, amountChanged = amountChanged, newAmount = value });
        }
    }
    private float m_phosphorusAmount = 0;
    public float phosphorusAmount
    {
        get { return m_phosphorusAmount; }
        set
        {
            float amountChanged = value - m_phosphorusAmount;
            m_phosphorusAmount = value;

            if (OnResourceChanged != null)
                OnResourceChanged(this, new ResourceChangedEventArgs { type = ResourceType.Phosphorous, amountChanged = amountChanged, newAmount = value });
        }
    }
    private float m_sulfurAmount = 0;
    public float sulfurAmount
    {
        get { return m_sulfurAmount; }
        set
        {
            float amountChanged = value - m_sulfurAmount;
            m_sulfurAmount = value;

            if (OnResourceChanged != null)
                OnResourceChanged(this, new ResourceChangedEventArgs { type = ResourceType.Sulfur, amountChanged = amountChanged, newAmount = value });
        }
    }
    private float m_energyAmount = 100;
    public float energyAmount
    {
        get { return m_energyAmount; }
        set
        {
            float amountChanged = value - m_energyAmount;
            m_energyAmount = value;

            if (OnResourceChanged != null)
                OnResourceChanged(this, new ResourceChangedEventArgs { type = ResourceType.Energy, amountChanged = amountChanged, newAmount = value });
        }
    }
    private float m_potassiumAmount = 0;
    public float potassiumAmount
    {
        get { return m_potassiumAmount; }
        set
        {
            float amountChanged = value - m_potassiumAmount;
            m_potassiumAmount = value;

            if (OnResourceChanged != null)
                OnResourceChanged(this, new ResourceChangedEventArgs { type = ResourceType.Potassium, amountChanged = amountChanged, newAmount = value });
        }
    }

    public float GetResourceAmountByType(ResourceType type)
    {
        switch(type){
            case ResourceType.Water:
                return waterAmount;

            case ResourceType.Energy:
                return energyAmount;

            case ResourceType.Nitrogen:
                return nitrogenAmount;

            case ResourceType.Phosphorous:
                return phosphorusAmount;

            case ResourceType.Potassium:
                return potassiumAmount;

            case ResourceType.Sulfur:
                return sulfurAmount;

            default:
                return 0;
        }
    }

    public void SetResourceAmountByType(ResourceType type, float number)
    {
        switch (type)
        {
            case ResourceType.Water:
                waterAmount = number;
                return;

            case ResourceType.Energy:
                energyAmount = number;
                return;

            case ResourceType.Nitrogen:
                nitrogenAmount = number;
                return;

            case ResourceType.Phosphorous:
                phosphorusAmount = number;
                return;

            case ResourceType.Potassium:
                potassiumAmount = number;
                return;

            case ResourceType.Sulfur:
                sulfurAmount = number;
                return;

            default:
                return;
        }
    }

    public void AddResourceAmountByType(ResourceType type, float number)
    {
        switch (type)
        {
            case ResourceType.Water:
                waterAmount += number;
                return;

            case ResourceType.Energy:
                energyAmount += number;
                return;

            case ResourceType.Nitrogen:
                nitrogenAmount += number;
                return;

            case ResourceType.Phosphorous:
                phosphorusAmount += number;
                return;

            case ResourceType.Potassium:
                potassiumAmount += number;
                return;

            case ResourceType.Sulfur:
                sulfurAmount += number;
                return;

            default:
                return;
        }
    }
}
