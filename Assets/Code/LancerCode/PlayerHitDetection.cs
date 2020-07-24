using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    private bool hasCollided = false;
    public LancerController lancer;
    RaycastHit hit;
    private CharacterStats charStats;
    private void OnEnable()
    {
        hasCollided = false;
        charStats = transform.root.GetComponent<CharacterStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Block" && !playerHit)
        //{
        //    if (hasCollided == false)
        //    {
        //        hasCollided = true;
        //        blocked = true;
        //        blockedBy = other.gameObject;
        //        //BroadcastMessage("Hitstun");
        //        //other.transform.root.gameObject.GetComponent<Health>().DecreaseHealth(5);
        //        other.transform.root.gameObject.GetComponent<Stamina>().DecreaseStam(10);
        //        other.transform.root.gameObject.GetComponent<PlayerRage>().IncreaseRage(5);
        //        StartCoroutine(CollisionTimer());
        //        //Debug.Log("hit");
        //        Debug.Log("Hit Shield");
        //        //Debug.Log(other.transform.root);
        //    }
        //}

        //if (other.tag == "Player" && !blocked)
        //{
        //    if (hasCollided == false)
        //    {
        //        hasCollided = true;
        //        playerHit = true;
        //        player = other.gameObject;
        //        //BroadcastMessage("Hitstun");
        //        player.GetComponent<Health>().DecreaseHealth(10);
        //        player.GetComponent<PlayerRage>().IncreaseRage(15);
        //        StartCoroutine(CollisionTimer());
        //        //Debug.Log("hit");
        //        Debug.Log(other.tag);
        //        //Debug.Log(other.transform.root);
        //    }
        //}

        if (other.tag == "Player" && lancer.blocked == true && lancer.isUnblockable == false||
            other.tag == "Block" && lancer.isUnblockable == false)
        {
            if(hasCollided == false)
            {
                hasCollided = true;
                StartCoroutine(CollisionTimer());
                other.transform.root.gameObject.GetComponent<CharacterStats>().DecreaseStamina(10);
                other.transform.root.gameObject.GetComponent<CharacterStats>().IncreaseRage(5);
                Debug.Log("Hit Shield");
            }
        }
        if(other.tag == "Player" && lancer.blocked == false || other.tag == "Player" && lancer.isUnblockable == true)
        {
            if (hasCollided == false)
            {
                hasCollided = true;
                StartCoroutine(CollisionTimer());
                other.transform.root.gameObject.BroadcastMessage("Hitstun");
                other.GetComponent<CharacterStats>().TakeDamage(charStats.damage.GetValue());
                other.GetComponent<CharacterStats>().IncreaseRage(10);
                Debug.Log("Hit Player");
            }
        }
    }


    private IEnumerator CollisionTimer()
    {
        yield return new WaitForSeconds(0.20f);
        hasCollided = false;
    }
}
