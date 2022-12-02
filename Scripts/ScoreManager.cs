using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;

    void OnTriggerEnter2D(Collider2D other) {
        score += 10;
        Debug.Log(score);
        Destroy(other.gameObject);
    }

}
