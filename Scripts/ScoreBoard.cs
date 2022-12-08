using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ScoreBoard : MonoBehaviour
{
    public Transform ScoreContainer;
    public GameObject ScoreBoardItemPrefab;

    void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScoreboardItem(player);
        }
    }

    void AddScoreboardItem(Player player)
    {
        ScoreBoardItem item = Instantiate(ScoreBoardItemPrefab, ScoreContainer)
            .GetComponent<ScoreBoardItem>();
        item.Initialize(player);
    }

}
