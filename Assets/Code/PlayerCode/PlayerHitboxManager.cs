using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerHitboxManager : MonoBehaviour
{
    [Header("Colliders")]
    public GameObject wep_Collider; //Weapon Hitbox
    public GameObject parry_Collider; //Parry Hitbox
    public GameObject shield_Collider; //Shield Hitbox
    public GameObject shield_Particles; //Shield Particles
    public CharacterController CharControl; //Character Controller / hitbox
    GameObject projectilePrefab;
    public Transform projectileSpawn;

    void Start()
    {
        if (wep_Collider == null)
            Debug.Log("No Weapon Collider Assigned To Character!!");
        else
            AttackHitboxDisable();

        if (parry_Collider == null)
            Debug.Log("No Parry Collider Assigned To Character!!");
        else
            ParryHitboxDisable();

        CharControl = GetComponent<CharacterController>();
        projectilePrefab = Resources.Load("RageProjectile") as GameObject;
    }

    void AttackHitboxEnable()
    {
        wep_Collider.GetComponent<CapsuleCollider>().enabled = true; //Enable Weapon Hitbox
    }

    void AttackHitboxDisable()
    {
        wep_Collider.GetComponent<CapsuleCollider>().enabled = false; //Disable Weapon Hitbox
    }

    void ParryHitboxEnable()
    {
        parry_Collider.SetActive(true); //Enable Parry Hitbox
    }

    void ParryHitboxDisable()
    {
        parry_Collider.SetActive(false); //Disable Parry Hitbox
    }

    void BlockHitboxEnable()
    {
        shield_Collider.SetActive(true); //Enable Block Hitbox
        shield_Particles.SetActive(true); //Enable Block Particle Effect
    }

    void BlockHitboxDisable()
    {
        shield_Collider.SetActive(false); //Disable Block Hitbox
        shield_Particles.SetActive(false); //Disable Block Particle Effect
    }

    void PlayerEnable()
    {
        //CharControl.enabled = true; //Enable Player's hitbox and movement
        CharControl.GetComponent<Collider>().enabled = true; //Enable Player's hitbox
    }

    void PlayerDisable()
    {
        CharControl.GetComponent<Collider>().enabled = false; //Disables Player's hitbox
    }

    void CreateProjectile()
    {
        GameObject RP = Instantiate(projectilePrefab) as GameObject;
        RP.transform.position = projectileSpawn.transform.position;
        Vector3 rotation = RP.transform.rotation.eulerAngles;
        RP.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        Rigidbody rbody = RP.GetComponent<Rigidbody>();
        rbody.AddForce(projectileSpawn.forward * 1f, ForceMode.Impulse);
    }
}
