using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scr_Menu_Start_Quit : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Interact"))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetButtonDown("Quit"))
        {
            Application.Quit();
        }
    }
}
