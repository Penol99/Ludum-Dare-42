using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scr_Item_Pot : MonoBehaviour, IInteract {

    public static int stageLevel = 0;
    public AudioSource aGotoNextLevel, aFlowerGrow;

    Scr_Player_Items pItems;
    public List<GameObject> flowerLevels = new List<GameObject>();
    public Transform transportPoint1;
    public Transform transportPoint2;
    public bool hasSoil, hasWater, hasSeeds;
    public GameObject potWater, potSeed, potDirt;
    public int flowerLevel = 0;
    public int flowerLevelLimit = 3;
    public int flowerXP = 0;
    public int flowerXPLimit = 5;

    public void Interact(GameObject whoInteracted)
    {
        pItems = whoInteracted.GetComponent<Scr_Player_Items>();
        if (flowerLevel.Equals(flowerLevelLimit))
        {
            if (stageLevel >= 2)
            {
                SceneManager.LoadScene(3);
                return;
            }
            else
            {
                
                if (stageLevel == 0)
                {
                    pItems.transform.position = transportPoint1.position;
                } else if (stageLevel == 1)
                {
                    pItems.transform.position = transportPoint2.position;
                }
                stageLevel++;
                aGotoNextLevel.Play();
                return;
            }
        }        
        else
        {
            switch (pItems.currentItem)
            {
                case Items.SEEDS:
                    if (!hasSeeds)
                        AddItem(Items.SEEDS);
                    break;
                case Items.WATER:
                    AddItem(Items.WATER);
                    break;
                default:
                    break;
            }
        }
    }

    void Start()
    {
        stageLevel = 0;
    }

    public void AddItem(Items item)
    {
        switch (item)
        {
            case Items.SOIL:
                hasSoil = true;
                potDirt.SetActive(true);
                break;
            case Items.SEEDS:
                hasSeeds = true;
                potSeed.SetActive(true);
                break;
            case Items.WATER:
                if (!flowerLevel.Equals(0))
                {
                    potWater.SetActive(true);
                    Invoke("TurnOfWater", .7f);
                    flowerXP++;
                    if (flowerXP >= flowerXPLimit)
                    {
                        flowerLevel++;
                        flowerXP = 0;

                        UpgradeFlower(flowerLevel);
                    }
                } else
                {
                    potWater.SetActive(true);
                }
                hasWater = true;
                
                break;
            default:
                break;
        }
        if (flowerLevel.Equals(0))
        {
            if (hasWater && hasSeeds && hasSoil)
            {
                flowerLevel = 1;
                UpgradeFlower(1);
            }
        }
        gameObject.tag = "Potted";
    }

    void TurnOfWater()
    {
        potWater.SetActive(false);
    }

    void UpgradeFlower(int level)
    {
        potSeed.SetActive(false);
        potWater.SetActive(false);
        if (level.Equals(1))
        {
            aFlowerGrow.Play();
            flowerLevels[1].SetActive(true);
        }else if (!level.Equals(flowerLevelLimit+1))
        {
            aFlowerGrow.Play();
            flowerLevels[level - 1].SetActive(false);
            flowerLevels[level].SetActive(true);
        }
    }
}
