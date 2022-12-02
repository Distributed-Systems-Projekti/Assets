using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour
{
    public TMP_Text PlayerNameText;
    public Image PlayerColorImage;
    private int ownerId;

    public void Initialize(int playerId, string playerName)
    {
        ownerId = playerId;
        PlayerNameText.text = playerName;
    }

}
