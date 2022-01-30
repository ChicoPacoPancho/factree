using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        // Do every second (approximately)
        if (Mathf.Round(Time.time) != Mathf.Round(Time.time - Time.deltaTime)) {
            
            GridManagement grid = FindObjectOfType<GridManagement>();

            // Tally up all used and produced resources
            for (int x = 0; x < grid.cityGrid.Width; x++)
            {
                for (int y = 0; y < grid.cityGrid.Height; y++)
                {
                    var obj = grid.cityGrid.GetGridObject(x, y);
                    if (obj != null && obj.buildableSO != null)
                    {
                        if (obj.buildableSO.CheckUpkeep())
                        {
                            obj.buildableSO.SubtractUpkeep();
                            obj.buildableSO.AddIncome();
                        } else
                        {
                            //Debug.Log("Not enough resources for upkeep!");
                        }
                    }
                }
            }
        }
    }
    public void AddTimeResourceChange(ResourceType type, int resourceChangePerSecond, float totalSeconds)
    {
        StartCoroutine(ResourceChangeOverTime(type, resourceChangePerSecond, totalSeconds));
    }

    private IEnumerator ResourceChangeOverTime(ResourceType type, int amountPerSecond, float totalSeconds)
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
        public int amountChanged;
        public int newAmount;
    }

    private int m_waterAmount = 30;
    public int waterAmount
    {
        get { return m_waterAmount; }
        set {
            int amountChanged = value - m_waterAmount;
            m_waterAmount = value;

            if (OnResourceChanged != null)
                OnResourceChanged(this, new ResourceChangedEventArgs { type = ResourceType.Water, amountChanged = amountChanged, newAmount = value });
        }
    }
    private int m_nitrogenAmount = 30;
    public int nitrogenAmount
    {
        get { return m_nitrogenAmount; }
        set
        {
            int amountChanged = value - m_nitrogenAmount;
            m_nitrogenAmount = value;

            if (OnResourceChanged != null)
                OnResourceChanged(this, new ResourceChangedEventArgs { type = ResourceType.Nitrogen, amountChanged = amountChanged, newAmount = value });
        }
    }
    private int m_phosphorusAmount = 0;
    public int phosphorusAmount
    {
        get { return m_phosphorusAmount; }
        set
        {
            int amountChanged = value - m_phosphorusAmount;
            m_phosphorusAmount = value;

            if (OnResourceChanged != null)
                OnResourceChanged(this, new ResourceChangedEventArgs { type = ResourceType.Phosphorous, amountChanged = amountChanged, newAmount = value });
        }
    }
    private int m_sulfurAmount = 0;
    public int sulfurAmount
    {
        get { return m_sulfurAmount; }
        set
        {
            int amountChanged = value - m_sulfurAmount;
            m_sulfurAmount = value;

            if (OnResourceChanged != null)
                OnResourceChanged(this, new ResourceChangedEventArgs { type = ResourceType.Sulfur, amountChanged = amountChanged, newAmount = value });
        }
    }
    private int m_energyAmount = 100;
    public int energyAmount
    {
        get { return m_energyAmount; }
        set
        {
            int amountChanged = value - m_energyAmount;
            m_energyAmount = value;

            if (OnResourceChanged != null)
                OnResourceChanged(this, new ResourceChangedEventArgs { type = ResourceType.Energy, amountChanged = amountChanged, newAmount = value });
        }
    }
    private int m_potassiumAmount = 0;
    public int potassiumAmount
    {
        get { return m_potassiumAmount; }
        set
        {
            int amountChanged = value - m_potassiumAmount;
            m_potassiumAmount = value;

            if (OnResourceChanged != null)
                OnResourceChanged(this, new ResourceChangedEventArgs { type = ResourceType.Potassium, amountChanged = amountChanged, newAmount = value });
        }
    }

    public int GetResourceAmountByType(ResourceType type)
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

    public void SetResourceAmountByType(ResourceType type, int number)
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

    public void AddResourceAmountByType(ResourceType type, int number)
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
