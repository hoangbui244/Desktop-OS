using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace com.lockedroom.io.module.pc {
    public class AdditionalButton : MonoBehaviour {
        public GameObject MessageObject;
        private MessagingManager messagingManager;
        // Start is called before the first frame update
        public void Awake() {
            messagingManager = MessageObject.GetComponentInChildren<MessagingManager>();
        }

        // Update is called once per frame
        void Update() {

        }
        public void InsGallery() {
            //GameObject Title = GameObject.FindGameObjectWithTag("Title");
            //if (Title != null) {
            //    TextMeshProUGUI titleText = Title.GetComponent<TextMeshProUGUI>();
            //    if (titleText.text == "Dolphins") {
            //        messagingManager.InsGallery(null, 0, ImageMessage.Instance.DolphinsSp, "", "", "");
            //    } else if (titleText.text == "Evening") {
            //        messagingManager.InsGallery(null, 0, ImageMessage.Instance.EveningSp, "", "", "");
            //    } else if (titleText.text == "Focused") {
            //        messagingManager.InsGallery(null, 0, ImageMessage.Instance.FocusSp, "", "", "");
            //    } else if (titleText.text == "Glass Cube") {
            //        messagingManager.InsGallery(null, 0, ImageMessage.Instance.GlassSp, "", "", "");
            //    } else if (titleText.text == "Good Boy") {
            //        messagingManager.InsGallery(null, 0, ImageMessage.Instance.GoodBoySp, "", "", "");
            //    } else if (titleText.text == "Mountain Fuji") {
            //        messagingManager.InsGallery(null, 0, ImageMessage.Instance.FujiSp, "", "", "");
            //    } else if (titleText.text == "Peaceful") {
            //        messagingManager.InsGallery(null, 0, ImageMessage.Instance.PeacefullSp, "", "", "");
            //    } else if (titleText.text == "Raindrops") {
            //        messagingManager.InsGallery(null, 0, ImageMessage.Instance.RainSp, "", "", "");
            //    } else if (titleText.text == "Evening") {
            //        messagingManager.InsGallery(null, 0, ImageMessage.Instance.EveningSp, "", "", "");
            //    } else if (titleText.text == "The Cooler Daniel") {
            //        messagingManager.InsGallery(null, 0, ImageMessage.Instance.TheCoolerSp, "", "", "");
            //    } else if (titleText.text == "Evening") {
            //        messagingManager.InsGallery(null, 0, ImageMessage.Instance.EveningSp, "", "", "");
            //    } else if (titleText.text == "To The Moon... After I Wake Up!") {
            //        messagingManager.InsGallery(null, 0, ImageMessage.Instance.ToTheMoonSp, "", "", "");
            //    }
            //}
            messagingManager.InsGallery(null, 0, null, "", "", "");
        }
        //public void MakeBtAppear() {
        //    messagingManager.gameObject.SetActive(true);
        //}
        //public void MakeBtDisappear() {
        //    messagingManager.gameObject.SetActive(false);
        //}
        public void InsAudio() {
            messagingManager.InsAu(null, 0, AudioMessage.Instance.aClip, "");
        }
    }
}
