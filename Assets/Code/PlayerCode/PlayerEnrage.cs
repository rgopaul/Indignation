using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Oh My God. This is probably the messiest thing I've done in this entire project
 * I use IEnumerator Timers to trigger Animations to make sure things go smoothly
 * and I'm Having Animation events talk to this Class once Enrage has been met for the Player
*/
public class PlayerEnrage : MonoBehaviour
{
    #pragma warning disable 0649
    [SerializeField]
    ParticleSystem Aura;
    [SerializeField]
    ParticleSystem Lightning;
    #pragma warning restore 0649

    bool isEnraged = false;
    Animator anim;
    CharacterStats CStats;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CharacterStats>().InvokeEnrage += InvokeEnrage;
        anim = GetComponent<Animator>();
        CStats = GetComponent<CharacterStats>();
    }

    void InvokeEnrage(bool isEnraged)
    {
        if (this.isEnraged == false && isEnraged)
        {
            this.isEnraged = true;
            Enrage();
            Debug.Log("Player is Enraged");
            anim.Play("Rage Fall", 0);
            anim.SetFloat("vertical", 0);
            anim.SetFloat("horizontal", 0);
            anim.SetFloat("VelX", 0);
            anim.SetFloat("VelZ", 0);
            anim.SetBool("hit", false);
            anim.SetBool("LockOn", false);
            anim.SetBool("block", false);
            anim.SetBool("inEnrageAnim", true);
        }
    }

    // The Entire Enrage Sequence
    void Enrage()
    {
        StartCoroutine(EnrageAnimationTimer());
        CStats.damage.SetValue(15);
        CStats.rageDecayRate.SetValue(0);
    }

    private IEnumerator EnrageAnimationTimer()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Lightning Called");
        Lightning.gameObject.SetActive(true);
        yield return new WaitForSeconds(6f);
        Lightning.gameObject.SetActive(false);
        anim.Play("Enrage Part 2");
        
    }

    void ActivateAura()
    {
        Aura.gameObject.SetActive(true);
    }

    void EndEnrage()
    {
        anim.SetBool("inEnrageAnim", false);
        anim.SetBool("block", false);
    }
}
