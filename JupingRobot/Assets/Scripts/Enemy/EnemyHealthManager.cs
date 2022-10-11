using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{

    public int maxHealth = 1;
    private int currentHealt;

    public int deathSound;

    public GameObject deathEffect, itemDrop;


    void Start()
    {
        currentHealt = maxHealth;    
    }

    public void TakeDamage()
    {
        currentHealt--;
        if(currentHealt <=0)
        {
            AudioManager.instance.PlaySfx(deathSound);
            Destroy(gameObject);

            Instantiate(deathEffect, transform.position, transform.rotation);
            Instantiate(itemDrop, transform.position, transform.rotation);
        }
        PlayerController.instance.Bounce();
    }
}
