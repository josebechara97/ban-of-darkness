using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float maxDuration;
    public float countDown;
    public static bool isGameOver = false;
    public Text txtCountDown;
    public Text txtFinalMessage;
    public AudioClip winSFX;
    public AudioClip loseSFX;
    public string nextLevel;
    public AudioClip enemySFX;
    public Text scoretxt;
    public int startingEnemyCount;
    public int enemiesKilled;
    public bool playOnce;

    // Start is called before the first frame update
    void Start()
    {

        playOnce = false;
        isGameOver = false;
        this.countDown = maxDuration;
        UpdateTimerText();
        startingEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemiesKilled = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            if (this.countDown > 0)
            {
                this.countDown -= Time.deltaTime;
                UpdateTimerText();
                UpdateScoreText();
            }
            else
            {
                LevelLost();
            }
        }
        if (enemiesKilled >= startingEnemyCount)
        {
            LevelWon();
        }
    }

    void UpdateTimerText()
    {
        this.txtCountDown.text = this.countDown.ToString("F2");
    }

    void UpdateScoreText()
    {
        this.scoretxt.text = enemiesKilled + "/" + startingEnemyCount;
    }


    public void LevelLost()
    {
        if (!playOnce)
        {
            AudioSource.PlayClipAtPoint(this.loseSFX, GameObject.FindGameObjectWithTag("MainCamera").transform.position);
            this.countDown = 0.00f;
            UpdateTimerText();
            isGameOver = true;
            txtFinalMessage.text = "GAME OVER!";
            txtFinalMessage.enabled = true;
            Invoke("RepeatLevel", 2);
            playOnce = true;
        }
    }

    public void LevelWon()
    {
        if (!playOnce)
        {
            AudioSource.PlayClipAtPoint(this.winSFX, GameObject.FindGameObjectWithTag("MainCamera").transform.position);
            isGameOver = true;
            txtFinalMessage.text = "YOU WON!";
            txtFinalMessage.enabled = true;
            Invoke("LoadLevel", 2);
            playOnce = true;
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(this.nextLevel);
    }

    public void RepeatLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;
        SceneManager.LoadScene(sceneName);
    }

    public void EnemyDestroyed()
    {
        AudioSource.PlayClipAtPoint(this.enemySFX, GameObject.FindGameObjectWithTag("MainCamera").transform.position);
    }

}
