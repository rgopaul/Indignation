using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceStateManager : MonoBehaviour
{
    private Animator LanceAnim;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        LanceAnim = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
        // this returns prevents any further updates after Lancer dies
        if (dead == true)
        {
            return;
        }
    }
    
    public void Die()
    {
        Debug.Log("dying 2 death");
        dead = true;
        LanceAnim.SetBool("death",true);

        Rigidbody rbody = GetComponent<Rigidbody>();
        Destroy(rbody);

        Object.Destroy(gameObject, 3.0f);

        //transform.rotation = Quaternion.identity;
    }
    private void OnDestroy()
    {
        Debug.Log("Removing Boss Health");
        GameObject.Find("HUD").transform.Find("LancerBossHealth").gameObject.SetActive(false);
        GameObject.Find("LancerAggroRange").gameObject.SetActive(false);
    }
}
