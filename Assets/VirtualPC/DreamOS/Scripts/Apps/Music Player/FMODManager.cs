using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace com.lockedroom.io.module.pc {
    public class FMODManager : MonoBehaviour {
        [field: Header("Music")]
        [field: SerializeField]
        public EventReference musicAnemo {
            get; private set;
        }
        [field: SerializeField]
        public EventReference musicDeck {
            get; private set;
        }
        [field: SerializeField]
        public EventReference musicJack {
            get; private set;
        }
        [field: SerializeField]
        public EventReference musicSilent {
            get; private set;
        }
        [field: SerializeField]
        public EventReference musicTalking {
            get; private set;
        }
        public static FMODManager instance {
            get; private set;
        }

        private void Awake() {
            instance = this;
        }
    }
}