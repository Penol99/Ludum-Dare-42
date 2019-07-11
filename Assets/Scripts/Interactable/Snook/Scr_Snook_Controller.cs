using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class Scr_Snook_Controller : MonoBehaviour {

    public CinemachineFreeLook cam;

    public GameObject canvas;
    public Text diagText;

    Scr_Player_Controller pCon;

    Animator anim;
    private List<string> dialogueList1 = new List<string>();
    private List<string> dialogueList2 = new List<string>();
    private List<string> currentList = new List<string>();

    public bool useFirstDialogue;

    private int textIndex;

    private bool hasActivated,startTalking;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        anim.SetBool("Hiding",true);
        pCon = GameObject.FindObjectOfType<Scr_Player_Controller>();

        dialogueList1.Add("Yo!");
        dialogueList1.Add("Nice day to grow flowers, i know~");
        dialogueList1.Add("I have a curious need, for sunflowers...");
        dialogueList1.Add("and i need YOU to grow me some.");
        dialogueList1.Add("I'm sure you know how to plant, yes?");
        dialogueList1.Add("Grow a flower by placing a pot in a plantable spot");
        dialogueList1.Add("put a seed in it");
        dialogueList1.Add("take a shovel to ...shovel soil in it,");
        dialogueList1.Add("and water it a couple times with the watering can.");
        dialogueList1.Add("In any order!");
        dialogueList1.Add("easy, yes?");

        dialogueList1.Add("My flower satiation changed depending on the place");
        dialogueList1.Add("Since it's the first place, One full-grown flower is enough.");
        dialogueList1.Add("'But why' you may ask.");
        dialogueList1.Add("well...");
        dialogueList1.Add("It's not only for my need, more than it is a stategy.");
        dialogueList1.Add("this place is getting covered by darkness.");
        dialogueList1.Add("we must escape by climbing!");
        dialogueList1.Add("it is not only in my interest to escape the black void, yes?");
        dialogueList1.Add("so if you were to kind, we could both escape it together!");
        dialogueList1.Add("I'm counting on you, miss.");


        if (useFirstDialogue)
        {
            currentList = dialogueList1;
        } else
        {
            currentList = dialogueList2;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(transform.position,pCon.transform.position) < 8 && !hasActivated && !startTalking)
        {
            
            anim.SetBool("Hiding", false);
            canvas.SetActive(true);
            startTalking = true;
            pCon.inCutscene = true;
            anim.SetTrigger("Rise");
            //cam.LookAt = canvas.transform;
            //canvas.transform.LookAt(new Vector3(cam.transform.position.x, canvas.transform.position.y, cam.transform.position.z));
            //canvas.transform.eulerAngles += new Vector3(0f,180f,0f);
        }



		if (!hasActivated && startTalking)
        {
            if (textIndex >= currentList.Count)
            {
                anim.SetTrigger("Sink");
                textIndex = 0;
                hasActivated = true;
                startTalking = false;
                canvas.SetActive(false);
                pCon.inCutscene = false;
                //cam.LookAt = pCon.transform;
                return;
            }
            if (Input.GetButtonDown("Interact"))
            {
                anim.SetTrigger("Idle");

                textIndex++;
            }
            diagText.text = currentList[textIndex];
        }
	}
}
