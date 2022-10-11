using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LSLevelEntry : MonoBehaviour
{
  
    public string levelName, levelToCheck, displayName;

    public bool canLoadLevel;

    public GameObject mapPointActive, mapPointInactive;

    private bool levelUnlocked;

    private bool levelLoading;

    private void Start() 
    {
        if(PlayerPrefs.GetInt(levelToCheck + "_unlocked") == 1 || levelToCheck == "")
        {
            mapPointActive.SetActive(true);
            mapPointInactive.SetActive(false);
            levelUnlocked = true;
        }
        else
        {
            mapPointActive.SetActive(true);
            mapPointInactive.SetActive(true);
            levelUnlocked = false;
        }

        if(PlayerPrefs.GetString("CurrentLevel") == levelName)
        {
            PlayerController.instance.transform.position = transform.position;
            LSResetPlayer.instance.respawnPosition = transform.position;
        }
    }

    private void Update() 
    {
        if(Input.GetButtonDown("Jump") && canLoadLevel && levelUnlocked && !levelLoading)
        {
            StartCoroutine("LevelLoadWaiter");
            levelLoading = true;

        }

        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if        (other.tag == "Player")
        {
            canLoadLevel = true;

            LSUIManager.instance.lNamePanel.SetActive(true);
            LSUIManager.instance.lNameText.text = displayName;

            if(PlayerPrefs.HasKey(levelName + "_coins"))
            {
                LSUIManager.instance.coinsText.text = PlayerPrefs.GetInt(levelName + "_coins").ToString();
            }
            else
            {
                LSUIManager.instance.coinsText.text = "???";
            }
        }
    }

        private void OnTriggerExit(Collider other) 
    {
        if        (other.tag == "Player")
        {
            canLoadLevel = false;

            LSUIManager.instance.lNamePanel.SetActive(false);
        }
    }

    public IEnumerator LevelLoadWaiter()
    {
        PlayerController.instance.stopMove = true;
        UIManager.instance.fadeToBlack = true;

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(levelName);
        PlayerPrefs.SetString("CurrentLevel", levelName);
    }

}
