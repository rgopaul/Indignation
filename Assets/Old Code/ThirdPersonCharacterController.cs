using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NO LONGER USED.
/// </summary>
/*
public class ThirdPersonCharacterController : MonoBehaviour
{
    public float Speed;
    private Animator Ani;
    public GameObject wep_Collider;

    // Start is called before the first frame update
    void Start()
    {
        Ani = GetComponent<Animator> ();

        if(wep_Collider == null)
        {
            Debug.Log("No Weapon Assigned To Character!!");
        }
        else
        {
            wep_Collider.GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerInput();
    }

    // Handles Player Movement
    void PlayerMovement()
    {
        if (Ani.GetBool("attacking") == true)
        {
            return;
        }
        else if (Ani.GetBool("attacking") == false)
        {
            if (Input.GetKey(KeyCode.W))
            {
                Ani.SetBool("running", true);
                Ani.SetInteger("condition", 1);

                // Grabs Horizonal / Vertical Axis  
                float hor = Input.GetAxis("Horizontal");
                float ver = Input.GetAxis("Vertical");

                // Applies Values to a Vector
                Vector3 playerMovement = new Vector3(hor, 0f, ver) * Speed * Time.deltaTime;

                // Moves Player
                transform.Translate(playerMovement, Space.Self);
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                Ani.SetBool("running", false);
                Ani.SetInteger("condition", 0);
            }
        }
    }

    void PlayerInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Ani.GetBool("running") == true)
            {
                Ani.SetBool("running", false);
                Ani.SetInteger("condition", 0);
            }
            if (Ani.GetBool("running") == false && Ani.GetBool("attacking") == false)
            {
                StartCoroutine(AttackFreeze());
            }
            
        }
    }

    IEnumerator AttackFreeze()
    {
        Ani.SetBool("attacking", true);
        Ani.SetInteger("condition", 2);
        yield return new WaitForSeconds(0.28f);
        wep_Collider.GetComponent<CapsuleCollider>().enabled = true;
        yield return new WaitForSeconds(0.50f);
        Ani.SetInteger("condition", 0);
        Ani.SetBool("attacking", false);
        wep_Collider.GetComponent<CapsuleCollider>().enabled = false;
    }
}*/
