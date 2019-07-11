using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Item_Pickup : MonoBehaviour, IInteract {


    public LayerMask detectLayer;
    public Items item;
    public Scr_Manager_Item itemMan;
    public Scr_interact_Grass thisTile;
    void Start()
    {
        itemMan = GameObject.FindGameObjectWithTag("Item Man").GetComponent<Scr_Manager_Item>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 5f, detectLayer))
        {
            thisTile = hit.transform.GetComponent<Scr_interact_Grass>();
            thisTile.busy = true;
            transform.position = hit.transform.position + Vector3.up / 2;
        }
    }

    public void Interact(GameObject whoInteracted)
    {
        
        Scr_Player_Items pItems = whoInteracted.GetComponent<Scr_Player_Items>();
        GameObject cItem = gameObject;
        
        switch (pItems.currentItem)
        {
            case Items.NOTHING:
                thisTile.busy = false;
                break;
            case Items.SHOVEL:
                cItem = itemMan.shovel;
                break;
            case Items.POT:
                cItem = itemMan.pot;
                break;
            case Items.SEEDS:
                cItem = itemMan.seed;
                break;
            case Items.CAN:
                cItem = itemMan.can;
                break;
            default:
                break;
        }
        if (!cItem.Equals(gameObject))
        {
            Instantiate(cItem, transform.position, cItem.transform.rotation);
        }
        pItems.currentItem = item;

        pItems.GetComponent<Scr_Player_Controller>().State = PlayerStates.WALK;
        Destroy(gameObject);
    }
}
