using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreBoardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text UsernameText;
    public TMP_Text ScoreText;

    Player player;

    public void Initialize(Player player)
    {
        UsernameText.text = player.NickName;
        this.player = player;
    }

    void UpdateStats()
    {
        if (player.CustomProperties.TryGetValue("score", out object score))
        {
            ScoreText.text = score.ToString();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        if (target == player)
        {
            if (changedProps.ContainsKey("score"))
            {
                UpdateStats();
            }
        }
    }
}
