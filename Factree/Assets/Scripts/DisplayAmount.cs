using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayAmount : MonoBehaviour
{
    public TextMeshProUGUI amountDisplay;
    public float amount;
    // Start is called before the first frame update
    public void SetAmount(float count)
    {
        amount = count;

        if (count > 0)
        {        
            amountDisplay.text = count.ToString();
        }
        else
        {
            amountDisplay.text = "";
        }
    }

    public float GetAmount()
    {
        return amount;
    }
}
