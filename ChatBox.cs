using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using BloonsTD6_Mod_Helper.Extensions;
using Assets.Scripts.Unity;
using UnityEngine.EventSystems;
using System;
using In_Game_Chat.Messages;

namespace In_Game_Chat
{
    public class ChatBox
    {
        public bool instantiated = false;
        private AssetBundle assetBundle;
        private GameObject chatBoxCanvasGo;

        private GameObject instantiatedChatBox;
        //private GameObject chatBox;
        private Text messageDisplay;
        Image textArea;
        ScrollRect textAreaScrollRect;

        private InputField messageInput;
        private KeyCode SendKey = KeyCode.Return;
        private KeyCode SendKeyAlt = KeyCode.KeypadEnter;

        public bool IsFocused { get { return IsChatFocused(); } }
        public bool IsVisible { get { return instantiatedChatBox.activeSelf; } set { SetVisibility(value); } }

        private float timeToIgnoreHotkeys = 0f;
        const float timeToIgnoreAfterSendMessage = 1f;

        public void InitializeChatBox()
        {
            if (assetBundle == null)
                assetBundle = AssetBundle.LoadFromMemory(Properties.Resources.chatbox_final);

            if (chatBoxCanvasGo == null)
                chatBoxCanvasGo = assetBundle.LoadAsset("Canvas_Final").Cast<GameObject>();

            if (instantiatedChatBox == null)
                instantiatedChatBox = GameObject.Instantiate(chatBoxCanvasGo);


            /*chatBox = instantiatedChatBox.transform.Find("/ChatBox").GetComponent<GameObject>();
            MelonLogger.Log($"chatBox == null: {chatBox == null}");*/

            textArea = instantiatedChatBox.transform.Find("ChatBox/Image").GetComponent<Image>();
            textAreaScrollRect = instantiatedChatBox.transform.Find("ChatBox/Image").GetComponent<ScrollRect>();
            messageDisplay = instantiatedChatBox.transform.Find("ChatBox/Image/MessageHistory").GetComponent<Text>();

            messageDisplay.text = "";
            messageInput = instantiatedChatBox.transform.Find("ChatBox/Input").GetComponent<InputField>();

            //messageDisplay.rectTransform.sizeDelta = new Vector2(messageDisplay.rectTransform.sizeDelta.x, textAreaScrollRect.rectTransform.sizeDelta.y);

            instantiatedChatBox.SetActive(false);
            instantiated = true;
        }

        /*public void InitializeOldChatBox()
        {
            if (assetBundle == null)
                assetBundle = AssetBundle.LoadFromMemory(Properties.Resources.chatbox);

            if (chatBoxCanvasGo == null)
                chatBoxCanvasGo = assetBundle.LoadAsset("Canvas").Cast<GameObject>();

            if (instantiatedChatBox == null)
                instantiatedChatBox = GameObject.Instantiate(chatBoxCanvasGo);

            chatBox = instantiatedChatBox.transform.Find("ChatBox").GetComponent<GameObject>();
            textArea = instantiatedChatBox.transform.Find("ChatBox/TextArea").GetComponent<Image>();
            textAreaScrollRect = instantiatedChatBox.transform.Find("ChatBox/TextArea").GetComponent<ScrollRect>();

            messageDisplay = instantiatedChatBox.transform.Find("ChatBox/TextArea/Text").GetComponent<Text>();
            messageDisplay.text = "";

            messageInput = instantiatedChatBox.transform.Find("ChatBox/InputField").GetComponent<InputField>();

            //messageDisplay.rectTransform.sizeDelta = new Vector2(messageDisplay.rectTransform.sizeDelta.x, textAreaScrollRect.rectTransform.sizeDelta.y);

            instantiatedChatBox.SetActive(false);
            instantiated = true;
        }*/


        public void Update() => CheckSendMessages();

        private void CheckSendMessages()
        {
            if (messageInput == null || !messageInput.IsActive())
                return;

            if (Input.GetKeyDown(SendKey) || Input.GetKeyDown(SendKeyAlt))
            {
                SendMessage();
                FocusOnTextInput();
            }
        }

        public void ShowHideChatBox()
        {
            if (!instantiated)
                InitializeChatBox();
                //InitializeOldChatBox();

            bool visibility = (instantiatedChatBox.activeSelf) ? false : true;
            instantiatedChatBox.SetActive(visibility);

            messageInput.text = "";
            if (visibility == true)
                FocusOnTextInput();
        }



        public void SendMessage()
        {
            string message = messageInput.text;

            if (string.IsNullOrEmpty(message))
                return;

            new Chat_Message(message);
        }

        public void SendMessage(Chat_Message message)
        {
            byte? peerID = GetPeerID(message.Message);
            if (peerID.HasValue)
                message.Message = message.Message.Remove(0, 3);

            var json = message.Serialize();
            SendMessageToPeers(json, peerID);
            messageInput.text = "";
            UpdateChatLog(message.Message, "Me");
        }

        private void SendMessageToPeers(string message, byte? peerId)
        {
            if (Game.instance?.nkGI != null)
                Game.instance.nkGI.SendMessage(message, peerId, code: Chat_Message.chatCoopCode);
        }

        private byte? GetPeerID(string message)
        {
            if (!message.StartsWith("1: ") && !message.StartsWith("2: ") && !message.StartsWith("3: ") && !message.StartsWith("4: "))
                return null;

            Int32.TryParse(message.Split(':')[0].Replace(": ", ""), out int id);
            return (byte?)id;
        }

        public void UpdateChatLog(string message, string sender)
        {
            ShowMessage($"{sender}:  {message}");
            IncreaseChatlogSize();

            textAreaScrollRect.normalizedPosition = new Vector2(0, 0);
            timeToIgnoreHotkeys = Time.time + timeToIgnoreAfterSendMessage;
        }

        public void ShowMessage(string message)
        {
            if (!instantiatedChatBox.activeSelf)
                ShowHideChatBox();

            var newTest = $"{messageDisplay.text}\n  {message}";
            messageDisplay.text = newTest;
        }


        private void IncreaseChatlogSize()
        {
            var msgDisplaySize = messageDisplay.rectTransform.sizeDelta;

            const float bufferSpace = 1.001f;
            var spaceToAdd = messageDisplay.fontSize + bufferSpace;

            messageDisplay.rectTransform.sizeDelta = new Vector2(msgDisplaySize.x, msgDisplaySize.y + spaceToAdd);
        }

        public void ClearChatLog()
        {
            if (messageDisplay != null)
                messageDisplay.text = "";
        }

        

        private bool IsChatFocused()
        {
            if (messageInput == null || !messageInput.IsActive())
                return false;

            bool inputFocused = messageInput.isFocused;
            bool ignoreHotkeys = Time.time < timeToIgnoreHotkeys;
            return inputFocused || ignoreHotkeys;
        }

        private void FocusOnTextInput()
        {
            messageInput.OnPointerClick(new PointerEventData(EventSystem.current));
        }

        private void SetVisibility(bool visible)
        {
            if (instantiatedChatBox != null)
                instantiatedChatBox.SetActive(visible);
        }
    }
}