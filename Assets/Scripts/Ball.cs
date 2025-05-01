using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{

    GameManager gm;
    Vector2 startPos;
    private Rigidbody2D rb;
    public float pushForce = 20f;
    private PlayerMovement[] players;
    private AI[] playersAI;
    private bool canGoal = true;
    private float notBouncing = 0;
    public AudioSource hitPlayer;
    public AudioSource goalAudio;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        players = FindObjectsOfType<PlayerMovement>();
        playersAI = FindObjectsOfType<AI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Check if is on ground for more than 1 sec
        if (Mathf.Abs(rb.velocity.y) < 0.01f) // casi cero, evita errores por precisión
        {
            notBouncing += Time.deltaTime;
        }
        else
        {
            notBouncing = 0f;
        }

        if (notBouncing > 1f)
        {
            rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
            notBouncing = 0f; // Reiniciar para que no lo repita cada frame
        }

        //Add max velocity
        Vector2 clampedVelocity = rb.velocity;

        // Limitar velocidad en X
        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -15, 15);

        // Limitar velocidad en Y
        clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -15, 15);

        rb.velocity = clampedVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canGoal && gm.play)
        {
            canGoal = false;
            if (collision.gameObject.name == "Goal Detector Left")
            {
                gm.aiScore++;
                goalAudio.Play();
                StartCoroutine(WaitAfterGoal());
            }
            else if (collision.gameObject.name == "Goal Detector Right")
            {
                gm.playerScore++;
                goalAudio.Play();
                StartCoroutine(WaitAfterGoal());
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (hitPlayer != null)
            {
                hitPlayer.Play();
            }
        }
    }
    IEnumerator WaitAfterGoal()
    {
        gm.play = false;
        gm.goalText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        gm.goalText.gameObject.SetActive(false);
        Reset();
    }
    public void Reset()
    {
        canGoal = true;
        rb.velocity = Vector2.zero;
        transform.position = startPos;
        gm.play = true;
        foreach (PlayerMovement player in players)
        {
            player.ResetPosition(); // Cada jugador vuelve a su lugar
            player.rb.velocity = Vector2.zero;
        }
        foreach (AI playerAI in playersAI)
        {
            playerAI.ResetPosition(); // Cada jugador vuelve a su lugar
            playerAI.rb.velocity = Vector2.zero;
        }
    }
}
