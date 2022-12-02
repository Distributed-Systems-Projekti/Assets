using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class RoomListItem : MonoBehaviour
{
    public TMP_Text name;
    public RoomInfo info;

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        name.text = _info.Name;
    }

    public void OnClick() { 
        LobbyScript.Instance.JoinRoom(info);
    }
}
