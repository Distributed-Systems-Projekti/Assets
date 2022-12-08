using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        score += 10;
        Hashtable hash = new Hashtable();
        hash.Add("score", score);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
}
