using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTimer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Enemy" && other.tag != "EnemyWep")
        {
            other.transform.root.gameObject.BroadcastMessage("Hitstun");
            other.transform.root.gameObject.GetComponent<CharacterStats>().TakeDamage(10);
            other.transform.root.gameObject.GetComponent<CharacterStats>().IncreaseRage(20);
            Debug.Log("fireball hit");
            Object.Destroy(gameObject);
        }
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(1.5f);
        Object.Destroy(gameObject);
    }
}
