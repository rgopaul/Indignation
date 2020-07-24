using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AggroRadius : MonoBehaviour
{
    public GameObject bossHealth;
    public AudioSource audioData;
    public GameObject doorLock;
    public LockOnTarget bossToLockOn;

    private void OnDestroy()
    {
        if(doorLock != null)
            doorLock.SetActive(false);
        if(bossHealth != null)
            bossHealth.SetActive(false);
        if(audioData != null)
            audioData.Pause();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Entered Boss Area");
            if (bossHealth != null)
                bossHealth.SetActive(true);
            if (audioData != null)
                audioData.Play(0);
            if(doorLock != null)
                doorLock.SetActive(true);

            other.transform.root.gameObject.BroadcastMessage("SetLockOn", bossToLockOn);
        }
    }

    /* Removed because rolling would cause this to be triggered.
    public void OnTriggerExit(Collider other)
    {    
        if(other.tag == "Player")
        {
            bossHealth.SetActive(false);
            audioData.Pause();
        }
    }*/

}
