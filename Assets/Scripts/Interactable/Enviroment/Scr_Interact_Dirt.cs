using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Interact_Dirt : MonoBehaviour, IInteract {

    private float interactTime = 0.5f;

    Scr_Player_Controller pCon;
    Scr_Player_Items pItems;

    public void Interact(GameObject whoInteracted)
    {
        pItems = whoInteracted.GetComponent<Scr_Player_Items>();
        pCon = whoInteracted.GetComponent<Scr_Player_Controller>();
        pItems.HasSoil = true;
        Invoke("ChangePlayerState", interactTime);
    }

    void ChangePlayerState()
    {
        pCon.State = PlayerStates.WALK;
    }

}
