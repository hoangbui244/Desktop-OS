using UnityEngine;
using System.Collections;
/// <summary>
/// 
/// </summary>
public class BloomAdd : MonoBehaviour {
    /// <summary>
    /// 
    /// </summary>
    Renderer[] m_rs;
    //??ParticleRenderer[] m_prs;
    /// <summary>
    /// 
    /// </summary>
    void OnEnable() {
        if (XPPostTextureCreate.Instance != null) {
            m_rs = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < m_rs.Length; i++) {
                XPPostTextureCreate.Instance.AddBloom(m_rs[i]);
            }
            //m_prs = GetComponentsInChildren<ParticleRenderer>();
            //for (int i = 0; i < m_prs.Length; i++) {
            //    XPPostTextureCreate.Instance.AddBloom(m_prs[i]);
            //}
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void OnDisable() {
        if (XPPostTextureCreate.Instance != null) {
            for (int i = 0; m_rs != null && i < m_rs.Length; i++) {
                XPPostTextureCreate.Instance.RemoveBloom(m_rs[i]);
            }
            //for (int i = 0; m_prs != null && i < m_prs.Length; i++) {
            //    XPPostTextureCreate.Instance.RemoveBloom(m_prs[i]);
            //}
            m_rs = null;
            //??m_prs = null;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void OnDestroy() {
        if (XPPostTextureCreate.Instance != null) {
            for (int i = 0; m_rs != null && i < m_rs.Length; i++) {
                XPPostTextureCreate.Instance.RemoveBloom(m_rs[i]);
            }
            //for (int i = 0; m_prs != null && i < m_prs.Length; i++) {
            //    XPPostTextureCreate.Instance.RemoveBloom(m_prs[i]);
            //}
            m_rs = null;
            //??m_prs = null;
        }
    }
}
