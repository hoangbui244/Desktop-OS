using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
#endif

namespace com.lockedroom.io.module.pc
{
    public class InputSystemChecker : MonoBehaviour
    {
        void Awake()
        {
#if ENABLE_INPUT_SYSTEM
            var newModule = gameObject.AddComponent<InputSystemUIInputModule>();
            newModule.enabled = false;
            newModule.enabled = true;
#else
            gameObject.AddComponent<StandaloneInputModule>();
#endif
        }
    }
}