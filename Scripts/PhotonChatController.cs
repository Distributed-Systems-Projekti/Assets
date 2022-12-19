using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Chat;
using Photon.Realtime;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif
// This part is not yet ready and working
public class PhotonChatController : MonoBehaviour, IChatClientListener
{
    public ChatClient chatClient;
    private string selectedChannelName;
    public RectTransform ChatPanel;
    public InputField InputFieldChat;
    public Text CurrentChannelText;
    public Toggle ChannelToggleToInstantiate;
    private readonly Dictionary<string, Toggle> channelToggles = new Dictionary<string, Toggle>();
    public int TestLength = 2048;
    private byte[] testBytes = new byte[2048];

    void Start()
    {
        chatClient = new ChatClient(this);
        chatClient.Connect(
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
            PhotonNetwork.AppVersion,
            new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName)
        );
    }

    // Update is called once per frame
    void Update()
    {
        chatClient.Service();
    }

    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message) { }

    public void OnDisconnected() { }

    public void OnConnected()
    {
        this.ChatPanel.gameObject.SetActive(true);
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages) { }

    public void OnClickSend()
    {
        if (this.InputFieldChat != null)
        {
            this.SendChatMessage(this.InputFieldChat.text);
            this.InputFieldChat.text = "";
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        this.InstantiateChannelButton(channelName);

        byte[] msgBytes = message as byte[];
        if (msgBytes != null)
        {
            Debug.Log("Message with byte[].Length: " + msgBytes.Length);
        }
        if (this.selectedChannelName.Equals(channelName))
        {
            this.ShowChannel(channelName);
        }
    }

    public void OnSubscribed(string[] channels, bool[] results) { }

    public void OnUnsubscribed(string[] channels) { }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) { }

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.LogFormat("OnUserSubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.LogFormat("OnUserUnsubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
    }

    private void InstantiateChannelButton(string channelName)
    {
        if (this.channelToggles.ContainsKey(channelName))
        {
            Debug.Log("Skipping creation for an existing channel toggle.");
            return;
        }

        Toggle cbtn = (Toggle)Instantiate(this.ChannelToggleToInstantiate);
        cbtn.gameObject.SetActive(true);
        cbtn.GetComponentInChildren<ChannelSelector>().SetChannel(channelName);
        cbtn.transform.SetParent(this.ChannelToggleToInstantiate.transform.parent, false);

        this.channelToggles.Add(channelName, cbtn);
    }

    private void SendChatMessage(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine))
        {
            return;
        }
        if ("test".Equals(inputLine))
        {
            if (this.TestLength != this.testBytes.Length)
            {
                this.testBytes = new byte[this.TestLength];
            }

            this.chatClient.SendPrivateMessage(
                this.chatClient.AuthValues.UserId,
                this.testBytes,
                true
            );
        }

        bool doingPrivateChat = this.chatClient.PrivateChannels.ContainsKey(
            this.selectedChannelName
        );
        string privateChatTarget = string.Empty;
        if (doingPrivateChat)
        {
            // the channel name for a private conversation is (on the client!!) always composed of both user's IDs: "this:remote"
            // so the remote ID is simple to figure out

            string[] splitNames = this.selectedChannelName.Split(new char[] { ':' });
            privateChatTarget = splitNames[1];
        }
        //UnityEngine.Debug.Log("selectedChannelName: " + selectedChannelName + " doingPrivateChat: " + doingPrivateChat + " privateChatTarget: " + privateChatTarget);

        if (doingPrivateChat)
        {
            this.chatClient.SendPrivateMessage(privateChatTarget, inputLine);
        }
        else
        {
            this.chatClient.PublishMessage(this.selectedChannelName, inputLine);
        }
    }

    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }

        this.selectedChannelName = channelName;
        this.CurrentChannelText.text = channel.ToStringMessages();
        Debug.Log("ShowChannel: " + this.selectedChannelName);

        foreach (KeyValuePair<string, Toggle> pair in this.channelToggles)
        {
            pair.Value.isOn = pair.Key == channelName ? true : false;
        }
    }

    public void OnChatStateChange(ChatState state) { }
}
