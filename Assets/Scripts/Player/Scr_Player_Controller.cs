using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    WALK,
    INTERACT,
    AIMING_SOIL,
    WATERING
}

public class Scr_Player_Controller : MonoBehaviour {

    // Public
    public GameObject shovelDirt;
    public GameObject xCanvas;
    public bool inCutscene;
    public GameObject parabolaAndSoil;
    public float movSpd;
    public LayerMask interactLayer;
    public LayerMask digLayer;
    public LayerMask waterLayer;
    public LayerMask itemPlaceLayer;
    public LayerMask groundLayer;
    public GameObject seed, shovel, can, pot;

    public AudioSource aDig, aItemDrop, aPickup, aThrowDirt, aWaterGet, aWaterGive;
    
    // Private
    private PlayerStates state;

    Vector2 leftStick = new Vector2();
    Vector2 rightStick = new Vector2();

    bool interactBtn, actionBtnHeld, actionBtnDown;
    bool canInteract;
    bool canDig;
    bool canGetWater;
    bool canPlaceItem;
    bool parabolaActive;
    bool soilPickedUp;
    bool hasCan,hasShovel,hasSeed, hasPot;

    Scr_Manager_Item itemMan;
    Transform cam;
    CharacterController cc;
    Scr_Player_Items pItems;
    Animator anim;

    public PlayerStates State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        itemMan = GameObject.FindGameObjectWithTag("Item Man").GetComponent<Scr_Manager_Item>();
        anim = GetComponent<Animator>();
        pItems = GetComponent<Scr_Player_Items>();
        cc = GetComponent<CharacterController>();
        cam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
        xCanvas.SetActive(pItems.HasSoil && pItems.currentItem.Equals(Items.SHOVEL));
        shovelDirt.SetActive(pItems.HasSoil && pItems.currentItem.Equals(Items.SHOVEL));
        InputVarSet();
        if (!inCutscene)
        {
            StateMachine();
            AnimationVariables();
            if (pItems.HasSoil && pItems.currentItem.Equals(Items.SHOVEL) && actionBtnHeld)
            {
                state = PlayerStates.AIMING_SOIL;
            }
        }
        

    }

    void StateMachine()
    {
        switch (State)
        {
            case PlayerStates.WALK:
                Walking();
                break;
            case PlayerStates.INTERACT:
                break;
            case PlayerStates.AIMING_SOIL:
                AimingSoil();
                break;
            case PlayerStates.WATERING:
                Watering();
                break;
            default:
                break;
        }
    }

    void InputVarSet()
    {
        leftStick = new Vector2(Input.GetAxis("LeftHor"), Input.GetAxis("LeftVer"));
        rightStick = new Vector2(Input.GetAxis("RightHor"), Input.GetAxis("RightVer"));
        interactBtn = Input.GetButtonDown("Interact");
        actionBtnHeld = Input.GetButton("Action");
        actionBtnDown = Input.GetButtonDown("Action");
        

    }

    void AnimationVariables()
    {
        anim.SetFloat("XAxis", leftStick.x);
        anim.SetFloat("YAxis", leftStick.y);
        if (pItems.currentItem.Equals(Items.CAN))
        {
            hasCan = true;
        }
        else
        {
            hasCan = false;
        }
        if (pItems.currentItem.Equals(Items.SEEDS))
        {
            hasSeed = true;
        }
        else
        {
            hasSeed = false;
        }
        if (pItems.currentItem.Equals(Items.SHOVEL))
        {
            hasShovel = true;
        }
        else
        {
            hasShovel = false;
        }
        if (pItems.currentItem.Equals(Items.POT))
        {
            hasPot = true;
        }
        else
        {
            hasPot = false;
        }

        anim.SetBool("Has Can", hasCan);
        anim.SetBool("Has Seed", hasSeed);
        anim.SetBool("Has Shovel", hasShovel);
        anim.SetBool("Has Pot", hasPot);
        seed.SetActive(hasSeed);
        can.SetActive(hasCan);
        shovel.SetActive(hasShovel);
        pot.SetActive(hasPot);
    }

    void Interaction()
    {
       
        //Debug.DrawRay(transform.position - transform.forward + Vector3.up, (transform.forward) * 3.2f, Color.blue);
        //Debug.DrawRay(transform.position - transform.forward + Vector3.up, (transform.forward + transform.right/6f)* 3.2f, Color.blue);
        //Debug.DrawRay(transform.position - transform.forward + Vector3.up, (transform.forward - transform.right/6f) * 3.2f, Color.blue);
        

        if (interactBtn)
        {
            RaycastHit interactHit;
            RaycastHit digHit;
            RaycastHit waterHit;
            RaycastHit itemPlaceHit;
            canInteract = 
                Physics.Raycast(transform.position - transform.forward*1.2f, transform.forward + Vector3.up, out interactHit, 3.2f, interactLayer) ||
                Physics.Raycast(transform.position - transform.forward*1.2f + Vector3.up, transform.forward + transform.right/6f, out interactHit, 3.2f, interactLayer) ||
                Physics.Raycast(transform.position - transform.forward*1.2f + Vector3.up, transform.forward - transform.right/6f, out interactHit, 3.2f, interactLayer);

            canDig = Physics.Raycast(transform.position, -transform.up, out digHit, 1.2f, digLayer);

            canGetWater =
                Physics.Raycast(transform.position + transform.forward * 2, -transform.up, out waterHit, 1.2f, waterLayer) ||
                Physics.Raycast(transform.position + transform.forward * 2 + transform.right/6f, -transform.up, out waterHit, 1.2f, waterLayer) ||
                Physics.Raycast(transform.position + transform.forward * 2 - transform.right/6f, -transform.up, out waterHit, 1.2f, waterLayer);

            canPlaceItem = Physics.Raycast(transform.position + transform.forward * 2 + Vector3.up, -transform.up, out itemPlaceHit, 1.2f, itemPlaceLayer);


            if (canInteract)
            {
                if (interactHit.transform.tag.Equals("Potted"))
                {
                    Scr_Item_Pot pot = interactHit.transform.GetComponent<Scr_Item_Pot>();
                    if (pot.flowerLevel.Equals(pot.flowerLevelLimit))
                    {
                        pot.Interact(gameObject);
                        return;
                    }
                }
            }

            if (pItems.HasWater && pItems.currentItem.Equals(Items.CAN) && canInteract)
            {
                if (interactHit.transform.tag.Equals("Pot") || interactHit.transform.tag.Equals("Potted"))
                {
                    aWaterGive.Play();
                    anim.SetTrigger("Watering");
                    Scr_Item_Pot pot = interactHit.transform.GetComponent<Scr_Item_Pot>();
                    pot.AddItem(Items.WATER);
                    pItems.HasWater = false;
                    state = PlayerStates.WATERING;
                    return;
                }
            }
            if (pItems.currentItem.Equals(Items.SEEDS) && canInteract)
            {
                aItemDrop.Play();
                if (interactHit.transform.tag.Equals("Pot") || interactHit.transform.tag.Equals("Potted"))
                {
                    anim.SetTrigger("Place Seed");
                    interactHit.transform.GetComponent<Scr_Item_Pot>().Interact(gameObject);
                    pItems.currentItem = Items.NOTHING;
                    return;
                }
            }

            if (!canGetWater && !canInteract && !canDig && canPlaceItem)
            {
                if (!itemPlaceHit.transform.GetComponent<Scr_interact_Grass>().busy)
                {
                    if (!pItems.currentItem.Equals(Items.NOTHING))
                    {
                        GameObject cItem = gameObject;
                        switch (pItems.currentItem)
                        {
                            case Items.SHOVEL:
                                cItem = itemMan.shovel;
                                break;
                            case Items.POT:
                                anim.SetTrigger("Place Pot");
                                cItem = itemMan.potToFlower;
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
                        itemPlaceHit.transform.GetComponent<Scr_interact_Grass>().busy = true;
                        aItemDrop.Play();
                        GameObject placedItem = Instantiate(cItem, itemPlaceHit.transform.position + Vector3.up / 2, cItem.transform.rotation);
                        if (placedItem.GetComponent<Scr_Item_Pickup>() != null)
                        {
                            Scr_Item_Pickup placedCode = placedItem.GetComponent<Scr_Item_Pickup>();
                            placedCode.thisTile = itemPlaceHit.transform.GetComponent<Scr_interact_Grass>();
                        }
                        pItems.currentItem = Items.NOTHING;
                    }
                }
            }

            

            if (canInteract && !(pItems.HasSoil && pItems.currentItem.Equals(Items.SHOVEL)) && !(pItems.HasWater && pItems.currentItem.Equals(Items.CAN)))
            {
                if (!interactHit.transform.tag.Equals("Potted"))
                {
                    interactHit.transform.GetComponent<IInteract>().Interact(gameObject);
                }
                //state = PlayerStates.INTERACT;
            }
            if (canDig && !pItems.HasSoil && pItems.currentItem.Equals(Items.SHOVEL))
            {
                aDig.Play();
                anim.SetTrigger("Dig");
                digHit.transform.GetComponent<IInteract>().Interact(gameObject);
                state = PlayerStates.INTERACT;
            }
            if (canGetWater && !pItems.HasWater && pItems.currentItem.Equals(Items.CAN))
            {
                aWaterGet.Play();
                anim.SetTrigger("Watering");
                waterHit.transform.GetComponent<IInteract>().Interact(gameObject);
                state = PlayerStates.INTERACT;
            }
        }


        
    }
    
    void Watering()
    {
        
        Invoke("BackToWalkState", 0.5f);
    }

    void BackToWalkState()
    {
        state = PlayerStates.WALK;
    }

    void AimingSoil()
    {

        if (actionBtnHeld)
        {
            Rotation();
            if (!parabolaActive)
            {
                parabolaActive = false;
                parabolaAndSoil.SetActive(true);
            }
        }
        else
        {
            aThrowDirt.Play();
            anim.SetTrigger("Throw Dirt");
            pItems.HasSoil = false;
            parabola parCode = parabolaAndSoil.GetComponentInChildren<parabola>();
            parCode.startMov = true;
            state = PlayerStates.WALK;
        }
    }


    Vector3 vel = Vector3.zero;

    void Rotation()
    {
        if (leftStick.magnitude > 0.1f)
        {
            float stickAngle = (Mathf.Atan2(leftStick.x, leftStick.y) * 180 / Mathf.PI) + cam.eulerAngles.y;
            //float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, stickAngle, ref vel, pCon.smoothDelay * Time.deltaTime);
            transform.eulerAngles = new Vector3(0f, stickAngle, 0f);


        }
    }
    bool grounded;
    void Walking()
    {
        Rotation();        
        Interaction();
        Vector3 grav = new Vector3(0f,0f,0f);
        grounded = 
            Physics.Raycast(transform.position + Vector3.up, -Vector3.up, 1f,groundLayer) ||
            Physics.Raycast(transform.position + Vector3.up + transform.right, -Vector3.up, 1f, groundLayer) ||
            Physics.Raycast(transform.position + Vector3.up - transform.right, -Vector3.up, 1f, groundLayer) ||
            Physics.Raycast(transform.position + Vector3.up + transform.forward, -Vector3.up, 1f, groundLayer) ||
            Physics.Raycast(transform.position + Vector3.up - transform.forward, -Vector3.up, 1f, groundLayer);
        //Debug.DrawRay(transform.position + Vector3.up, -Vector3.up, Color.blue);
        if (!grounded)
        {
            grav.y += -9 * Time.deltaTime;
        } else
        {
            grav.y = 0f;
        }
        cc.Move((transform.forward * (Mathf.Abs(leftStick.x) + Mathf.Abs(leftStick.y)) * movSpd * Time.deltaTime + grav));
    }
}
