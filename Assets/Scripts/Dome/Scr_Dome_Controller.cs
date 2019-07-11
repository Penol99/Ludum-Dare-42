using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scr_Dome_Controller : MonoBehaviour {

    public int level;

    public float speed;

    public Vector3 direction;
    Scr_Player_Controller pCon; 

	// Use this for initialization
	void Start () {
        pCon = GameObject.FindObjectOfType<Scr_Player_Controller>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Scr_Item_Pot.stageLevel == level && !pCon.inCutscene)
        {
            Movement();
        }
    }

    void Movement()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerStay(Collider o)
    {
        if (o.tag.Equals("Player"))
        {
            SceneManager.LoadScene(2);
        }
    }
}
