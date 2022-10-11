using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtPickup : MonoBehaviour
{

    public int healtAmount;
    public bool isFullHeal;

    public GameObject healthEffect;

    public int soundToPlay;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);

            Instantiate(healthEffect, PlayerController.instance.transform.position + new Vector3(0f, 1f, 0f), PlayerController.instance.transform.rotation);

            if (isFullHeal)
            {
                HealthManager.instance.ResetHealth();
            }
            else
            {
                HealthManager.instance.AddHealth(healtAmount);
            }
            AudioManager.instance.PlaySfx(soundToPlay);
        }

    }


}
