using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace com.lockedroom.io.module.pc {

    public class SendImageButton : MonoBehaviour {
        
        void Awake() {
            
        }
        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
        public void MakeBtAppear() {
            gameObject.SetActive(true);
        }
        public void MakeBtDisappear() {
            gameObject.SetActive(false);
        }
    }
}
