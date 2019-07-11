using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parabola : MonoBehaviour {

    public GameObject dirt;
    public Transform startPoint,midPoint,endPoint;
    LineRenderer lr;
    public float spd, inputSpd;
    private float initialSpd;
    Vector3 startPos, midPos, endPos;

    private bool beenInMid;
    public bool startMov;
    public bool resetMov;
    public bool drawParabola;
    
    private MeshRenderer mRender;
    private float parabolaRange = 6;

    // Use this for initialization
    void Start () {
        mRender = GetComponent<MeshRenderer>();
        initialSpd = Vector3.Distance(transform.position, midPos) * spd;
        lr = GetComponent<LineRenderer>();
        transform.position = startPoint.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (startMov)
            Movement();
        if (drawParabola)
            DrawParabola();

        if (resetMov)
        {
            transform.position = startPos;
            beenInMid = false;
            resetMov = false;
        }

    }

    void Movement()
    {
        drawParabola = false;
        lr.enabled = false;
        dirt.SetActive(true);
        float movSpd = Vector3.Distance(transform.position, midPos) * spd;
        movSpd = Mathf.Clamp(movSpd, initialSpd * 0.35f, Mathf.Infinity);
        if (Vector3.Distance(transform.position, midPos) > .5f && !beenInMid)
        {
            transform.position = Vector3.MoveTowards(transform.position, midPos, Time.deltaTime * movSpd);

        } else
        {
            beenInMid = true;          
            transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * movSpd);
        }
        if (Vector3.Distance(transform.position, endPos) < .2f)
        {
            ResetParabola();
        }
    }

    void DrawParabola()
    {
        lr.enabled = true;
        startPos = startPoint.position;
        midPos = midPoint.position;
        endPos = endPoint.position;
        float verMov = Input.GetAxis("LeftVer") * inputSpd * Time.deltaTime;
        float horMov = Input.GetAxis("LeftHor") * inputSpd * Time.deltaTime;
        endPos.x = Mathf.Clamp(endPos.x, startPos.x-parabolaRange, startPos.x+parabolaRange);
        endPos.z = Mathf.Clamp(endPos.z, startPos.z-parabolaRange, startPos.z+parabolaRange);
        endPos += ((startPoint.forward * (Mathf.Abs(verMov) + Mathf.Abs(horMov)))) * Time.deltaTime;// + (startPoint.right * horMov));
        endPos.y = startPos.y;
        

        float yMidPos = startPos.y + Vector3.Distance(startPos, endPos)/1.2f;
        yMidPos =  Mathf.Clamp(yMidPos, startPos.y + 2.5f, Mathf.Infinity);
        midPos = new Vector3((endPos.x + startPos.x) / 2, yMidPos, (endPos.z + startPos.z) / 2);


        startPoint.position = startPos;
        midPoint.position = midPos;
        endPoint.position = endPos;


        lr.SetPosition(0, startPos);
        lr.SetPosition(1, midPos);
        lr.SetPosition(2, endPos);
    }

    private void OnEnable()
    {
        drawParabola = true;
        transform.position = startPoint.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Pot") || other.tag.Equals("Potted"))
        {
            other.GetComponent<Scr_Item_Pot>().AddItem(Items.SOIL);
            ResetParabola();
        }
    }

    void ResetParabola()
    {
        beenInMid = false;
        resetMov = false;
        startMov = false;
        transform.parent.gameObject.SetActive(false);

    }
}
