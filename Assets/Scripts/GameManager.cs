using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int playerScore = 0, aiScore = 0;
    private float timer = 61f;
    public Text scoreText;
    public Text timeText;
    public Text finalText;
    public Text restartText;
    public Text goalText;
    private string score;
    public bool play = true;
    public Ball ball;
    private bool isRestarting = false;
    public AudioSource startWhisleAudio;
    public AudioSource finalWhisleAudio;

    // Start is called before the first frame update
    void Start()
    {
        finalText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);
        goalText.gameObject.SetActive(false);
        ball = FindObjectOfType<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timeText.text = Mathf.FloorToInt(timer).ToString();

        if (timer <= 1f)
        {
            goalText.gameObject.SetActive(false);
            play = false;

            timeText.text = "Game Over";
            if (playerScore > aiScore)
            {
                finalText.text = "¡¡¡GANASTE!!!";
            }
            else if (aiScore > playerScore)
            {
                finalText.text = "PERDISTE :(";
            }
            else
            {
                finalText.text = "EMPATE";
            }
            if(!isRestarting){
                finalWhisleAudio.Play();
                isRestarting = true;
                StartCoroutine(RestartGame());
            }
        }

        score = playerScore.ToString() + " - " + aiScore.ToString();

        scoreText.text = score.ToString();
    }
    IEnumerator RestartGame()
    {
        finalText.gameObject.SetActive(true);
        restartText.gameObject.SetActive(true);
        float i = 11;
        while (i > 1)
        {
            i -= Time.deltaTime;
            restartText.text = "Reiniciando el juego en " + Mathf.FloorToInt(i).ToString() + " segundos...";
            yield return null;

        }
        timer = 61f;
        play = true;
        playerScore = 0;
        aiScore = 0;
        finalText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);
        ball.Reset();
        isRestarting = false;
        startWhisleAudio.Play();
    }
}
