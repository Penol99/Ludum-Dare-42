using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Menu_Clouds : MonoBehaviour {

    public float speed;
    public Vector3 direction;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(direction * speed * Time.deltaTime);
	}
}
