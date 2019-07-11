using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Items
{
    NOTHING,
    SHOVEL,
    POT,
    SEEDS,
    CAN,
    WATER,
    SOIL
}

public class Scr_Player_Items : MonoBehaviour {

    // Public
    public Items currentItem;
    public GameObject waterBlob;
    // Private
    private bool hasSoil, hasWater;

    public bool HasSoil
    {
        get
        {
            return hasSoil;
        }

        set
        {
            hasSoil = value;
        }
    }

    public bool HasWater
    {
        get
        {
            return hasWater;
        }

        set
        {
            waterBlob.SetActive(value);
            hasWater = value;
            
        }
    }


}
