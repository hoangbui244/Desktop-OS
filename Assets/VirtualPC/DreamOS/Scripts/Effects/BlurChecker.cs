using UnityEngine;
using UnityEngine.Rendering;

namespace com.lockedroom.io.module.pc
{
    [ExecuteInEditMode]
    public class BlurChecker : MonoBehaviour
    {
        void OnEnable()
        {
            if (GraphicsSettings.renderPipelineAsset != null && gameObject.activeSelf == true)
                gameObject.SetActive(false);
        }
    }
}