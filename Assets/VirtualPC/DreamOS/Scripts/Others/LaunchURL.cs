using UnityEngine;

namespace com.lockedroom.io.module.pc
{
    public class LaunchURL : MonoBehaviour
    {
        public void OpenURL(string urlLink)
        {
            Application.OpenURL(urlLink);
        }
    }
}