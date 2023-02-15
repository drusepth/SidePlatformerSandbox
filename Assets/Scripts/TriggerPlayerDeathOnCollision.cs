using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerDeathOnCollision : MonoBehaviour
{
    public GameObject restartPoint;
    public GameObject player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Reduce lives by 1

            // Reload current scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

            // (or, keep scene loaded)
            // player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            // player.transform.position = restartPoint.transform.position;

        }
    }
}
