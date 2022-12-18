using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour
{
    public string FriendId
    {
        set { this.NameLabel.text = value; }
        get { return this.NameLabel.text; }
    }

    public Text NameLabel;

    public void Awake() { }

    public void OnFriendStatusUpdate(int status, bool gotMessage, object message) { }
}
