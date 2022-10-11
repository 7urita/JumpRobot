using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossActivator : MonoBehaviour
{
    public GameObject entrance, theBoss, bossCamera;
    

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            entrance.SetActive(false);
            theBoss.SetActive(true);
            gameObject.SetActive(false);
            bossCamera.SetActive(true);
        }
    }

}
