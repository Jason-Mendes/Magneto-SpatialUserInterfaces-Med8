using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Goal : MonoBehaviour
{
    public TMP_Text scoreText;
    int score = 0;

    Magnet magnet;

    private void Start()
    {
        magnet = GameObject.FindObjectOfType<Magnet>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        score++;
        magnet.RemoveItem(collision.gameObject);
        Destroy(collision.gameObject);
        scoreText.text = score.ToString();
    }
}
