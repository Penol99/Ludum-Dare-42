using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Player_Cam_Base : MonoBehaviour {

    Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }
    // Update is called once per frame
    void Update () {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cam.eulerAngles.y, transform.eulerAngles.z);
    }
}
