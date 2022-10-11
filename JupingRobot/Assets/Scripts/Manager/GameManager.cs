using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Vector3 respawnPosition;

    public GameObject deathEffect;

    public int currentCoins;

    public int levelEndMusic;

    public int soundToPlay;

    public string levelToLoad;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        respawnPosition = PlayerController.instance.transform.position;

        AddCoins(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpase();
        }
    }

    public void Respawn()
    {
        StartCoroutine(RespawnWaiter());
        HealthManager.instance.PlayerKilled();
    }

    public IEnumerator RespawnWaiter()
    {
        PlayerController.instance.gameObject.SetActive(false);

        CameraController.instance.cmBrain.enabled = false;

        UIManager.instance.fadeToBlack = true;

        Instantiate(deathEffect, PlayerController.instance.transform.position + new Vector3(0f, 1f, 0f), PlayerController.instance.transform.rotation);
        AudioManager.instance.PlaySfx(soundToPlay);


        yield return new WaitForSeconds(2f);

        UIManager.instance.fadeFromBlack = true;

        PlayerController.instance.transform.position = respawnPosition;

        CameraController.instance.cmBrain.enabled = true;

        PlayerController.instance.gameObject.SetActive(true);        
    
        HealthManager.instance.ResetHealth();
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        respawnPosition = newSpawnPoint;
        Debug.Log("SpawnSet");
    }

    public void AddCoins(int conisToAdd)
    {
        currentCoins += conisToAdd;
        UIManager.instance.coinText.text = "" + currentCoins;
    }

    public void PauseUnpase()
    {
        if(UIManager.instance.pauseScreen.activeInHierarchy)
        {
            UIManager.instance.pauseScreen.SetActive(false);
            Time.timeScale = 1f;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            UIManager.instance.pauseScreen.SetActive(true);
            UIManager.instance.ColseOptions();
            Time.timeScale = 0f;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public IEnumerator LevelEndWaiter()
    {
        AudioManager.instance.PlayMusic(levelEndMusic);
        PlayerController.instance.stopMove = true;
        yield return new WaitForSeconds(4f);


        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked", 1);

        if(PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_conins"))
        {
            if(currentCoins > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_conins"))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_conins", currentCoins);
            }
        }
        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_conins", currentCoins);
        }

        SceneManager.LoadScene(levelToLoad);

    }
}
