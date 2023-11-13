using System;
using System.Collections.Generic;
using UnityEngine;
namespace UnityStandardAssets.Water
{
    public enum WaterQuality
    {
        High = 2,
        Medium = 1,
        Low = 0,
    }
    [ExecuteInEditMode]
    public class WaterBase : MonoBehaviour
    {
        public Material sharedMaterial;
        public WaterQuality waterQuality = WaterQuality.High;
        public bool edgeBlend = true;
        public void UpdateShader()
        {
            if (waterQuality > WaterQuality.Medium)
            {
                sharedMaterial.shader.maximumLOD = 501;
            }
            else if (waterQuality > WaterQuality.Low)
            {
                sharedMaterial.shader.maximumLOD = 301;
            }
            else
            {
                sharedMaterial.shader.maximumLOD = 201;
            }
            // If the system does not support depth textures (ie. NaCl), turn off edge bleeding,
            // as the shader will render everything as transparent if the depth texture is not valid.
            if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
            {
                edgeBlend = false;
            }
            if (edgeBlend)
            {
                Shader.EnableKeyword("WATER_EDGEBLEND_ON");
                Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
                // just to make sure (some peeps might forget to add a water tile to the patches)
                if (Camera.main)
                {
                    Camera.main.depthTextureMode |= DepthTextureMode.Depth;
                }
            }
            else
            {
                Camera.main.depthTextureMode = DepthTextureMode.None;
                Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
                Shader.DisableKeyword("WATER_EDGEBLEND_ON");
            }
        }
        public void WaterTileBeingRendered(Transform tr, Camera currentCam)
        {
            if (currentCam && edgeBlend)
            {
                currentCam.depthTextureMode |= DepthTextureMode.Depth;
            }
        }
        public void Update()
        {
            if (sharedMaterial)
            {
                UpdateShader();
            }
        }
        public enum WaterQualityUser
        {
            //ɶ��������
            low,
            //�������
            medium,
            //�������,����,����Զ��ữ��Ե
            high,
            //����ȫ��,����Զ��ữ��Ե
            veryHigh,
        }
        public static WaterQualityUser m_cfgWaterQuality = WaterQualityUser.high;
        //
        public static List<MonoBehaviour> m_waterComponent = new List<MonoBehaviour>();
        public static void RemoveWaterComponent(MonoBehaviour m)
        {
            m_waterComponent.Remove(m);
        }
        public static void AddWaterComponent(MonoBehaviour m)
        {
            m_waterComponent.Add(m);
        }
        private void Start()
        {
            AddWaterComponent(this);
            ApplyFromConfig();
        }
        public void OnDestroy()
        {
            RemoveWaterComponent(this);
        }
        //--tgame
        public void ApplyFromConfig()
        {
            if(m_cfgWaterQuality == WaterQualityUser.veryHigh)
            {
                edgeBlend = true;
            }
            else
            {//m_cfgWaterQuality == WaterQualityUser.high||
                edgeBlend = false;
            }
            //if()
        }
        public static void SetWaterQualityUser(WaterQualityUser q)
        {
            m_cfgWaterQuality = q;
            for (int i = 0; i < m_waterComponent.Count; i++)
            {
                MonoBehaviour m = m_waterComponent[i];
                if (m is UnityStandardAssets.Water.WaterBase)
                {
                    UnityStandardAssets.Water.WaterBase w = (UnityStandardAssets.Water.WaterBase)m;
                    w.ApplyFromConfig();
                }
                else if (m is UnityStandardAssets.Water.PlanarReflection)
                {
                    UnityStandardAssets.Water.PlanarReflection w = (UnityStandardAssets.Water.PlanarReflection)m;
                    w.ApplyFromConfig();
                }
            }
        }
    }
}
