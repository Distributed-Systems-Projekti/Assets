using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChannelSelector : MonoBehaviour, IPointerClickHandler
{
    public string Channel;

    public void SetChannel(string channel)
    {
        this.Channel = channel;
        Text t = this.GetComponentInChildren<Text>();
        t.text = this.Channel;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PhotonChatController handler = FindObjectOfType<PhotonChatController>();
        handler.ShowChannel(this.Channel);
    }
}
