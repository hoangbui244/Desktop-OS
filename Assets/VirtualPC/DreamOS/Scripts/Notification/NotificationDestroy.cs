using UnityEngine;

namespace com.lockedroom.io.module.pc
{
    public class NotificationDestroy : MonoBehaviour
    {
        void OnEnable()
        {
            Destroy(gameObject);
        }
    }
}