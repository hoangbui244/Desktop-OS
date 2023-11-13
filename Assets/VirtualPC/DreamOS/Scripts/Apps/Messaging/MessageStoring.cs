using UnityEngine;
using System.IO;

namespace com.lockedroom.io.module.pc
{
    [AddComponentMenu("DreamOS/Apps/Messaging/Message Storing")]
    public class MessageStoring : MonoBehaviour
    {
        [Header("Resources")]
        public MessagingManager messagingManager;
        public string fullPath;

        string currentID;
        string currentType;
        string currentAuthor;
        string currentMessage;
        string currentTime;

        void Awake()
        {
            if (messagingManager == null) { 
                messagingManager = gameObject.GetComponent<MessagingManager>(); 
            }
        }

        public void CheckData() {
            
            if (!File.Exists(fullPath)) {
                FileInfo dataFile = new FileInfo(fullPath);
                dataFile.Directory.Create();
                File.WriteAllText(fullPath, "MSG_DATA");
            }
        }

        public void ReadMessageData()
        {
            CheckData();
            messagingManager.enabled = false;

            foreach (string option in File.ReadLines(fullPath))
            {
                if (option.Contains("MessageID: "))
                {
                    string tempID = option.Replace("MessageID: ", "");  
                    currentID = tempID;
                }

                else if (option.Contains("[Type]"))
                {
                    string tempType = option.Replace("[Type] ", "");
                    currentType = tempType;
                }

                else if (option.Contains("[Author]"))
                {
                    string tempAuthor = option.Replace("[Author] ", "");
                    currentAuthor = tempAuthor;
                }

                else if (option.Contains("[Message]"))
                {
                    string tempMsg = option.Replace("[Message] ", "");
                    currentMessage = tempMsg;
                }

                else if (option.Contains("[Time]"))
                {
                    string tempTime = option.Replace("[Time] ", "");
                    currentTime = tempTime;
                    messagingManager.timeHelper = currentTime;

                    if (currentAuthor == "self" && currentType == "standard")
                        messagingManager.CreateStoredMessage(currentID, currentMessage, currentTime, true);
                    else if (currentAuthor == "individual" && currentType == "standard")
                        messagingManager.CreateStoredMessage(currentID, currentMessage, currentTime, false);
                }
            }
            messagingManager.enabled = true;
        }

        public void ApplyMessageData(string msgID, string msgType, string author, string message, string msgTime)
        {
            File.AppendAllText(fullPath, "\n\nMessageID: " + msgID +
                "\n{" +
                "\n[Type] " + msgType +
                "\n[Author] " + author +
                "\n[Message] " + message +
                "\n[Time] " + msgTime +
                "\n}");
        }

        public void ResetData()
        {
            File.WriteAllText(fullPath, "MSG_DATA");
        }
    }
}