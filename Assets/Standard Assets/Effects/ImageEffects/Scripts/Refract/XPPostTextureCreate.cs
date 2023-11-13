using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
//含折射,辉光等的后期处理辅助脚本
public class XPPostTextureCreate : MonoBehaviour {
    //替换Screen RenderTarget到自己定义的RenderTarget(含DepthBuffer)
    CommandBuffer m_cmdGenerateSceneCopyBuf;
    RenderTargetIdentifier m_replaceRenderTargetID;
    public RenderTexture m_bloomAddTexture;
    public Shader m_BlurShader;
    private Material m_Material;
    List<Renderer> m_bloomAddRenders=new List<Renderer>();
    CommandBuffer m_cmdBloomAddBuf;
    int m_width = 1280;
    int m_height = 720;
    public float m_boolAddTextureScale = 0.25f;
    int m_enableSceneCopyBlurCount = 0;
    public static XPPostTextureCreate Instance;
    // Use this for initialization
    void Start()
    {
        m_width = (int)(Screen.width* m_boolAddTextureScale);
        m_height = (int)(Screen.height* m_boolAddTextureScale);
    }
    void RefreshSceneCopyCmd()
    {
        Camera camera = GetComponent<Camera>();
        if (m_cmdGenerateSceneCopyBuf==null)
        {
            m_cmdGenerateSceneCopyBuf = new CommandBuffer();
        }
        else
        {
            camera.RemoveCommandBuffer(CameraEvent.AfterSkybox, m_cmdGenerateSceneCopyBuf);
            m_cmdGenerateSceneCopyBuf.Clear();
        }
        if (m_enableSceneCopyBlurCount>0 && m_BlurShader != null)
        {
            m_cmdGenerateSceneCopyBuf.name = "GenerateSceneCopy_Blured";
            if (!m_Material)
            {
                m_Material = new Material(m_BlurShader);
                m_Material.hideFlags = HideFlags.HideAndDontSave;
            }
            // copy screen into temporary RT
            int screenCopyID = Shader.PropertyToID("_ScreenCopyTexture");
            m_cmdGenerateSceneCopyBuf.GetTemporaryRT(screenCopyID, -1, -1, 0, FilterMode.Bilinear);
            m_cmdGenerateSceneCopyBuf.Blit(BuiltinRenderTextureType.CurrentActive, screenCopyID);
            // get two smaller RTs
            int blurredID = Shader.PropertyToID("_Temp1");
            int blurredID2 = Shader.PropertyToID("_Temp2");
            m_cmdGenerateSceneCopyBuf.GetTemporaryRT(blurredID, -2, -2, 0, FilterMode.Bilinear);
            m_cmdGenerateSceneCopyBuf.GetTemporaryRT(blurredID2, -2, -2, 0, FilterMode.Bilinear);
            // downsample screen copy into smaller RT, release screen RT
            m_cmdGenerateSceneCopyBuf.Blit(screenCopyID, blurredID);
            m_cmdGenerateSceneCopyBuf.ReleaseTemporaryRT(screenCopyID);
            // horizontal blur
            m_cmdGenerateSceneCopyBuf.SetGlobalVector("offsets", new Vector4(2.0f / Screen.width, 0, 0, 0));
            m_cmdGenerateSceneCopyBuf.Blit(blurredID, blurredID2, m_Material);
            // vertical blur
            m_cmdGenerateSceneCopyBuf.SetGlobalVector("offsets", new Vector4(0, 2.0f / Screen.height, 0, 0));
            m_cmdGenerateSceneCopyBuf.Blit(blurredID2, blurredID, m_Material);
            // horizontal blur
            m_cmdGenerateSceneCopyBuf.SetGlobalVector("offsets", new Vector4(4.0f / Screen.width, 0, 0, 0));
            m_cmdGenerateSceneCopyBuf.Blit(blurredID, blurredID2, m_Material);
            // vertical blur
            m_cmdGenerateSceneCopyBuf.SetGlobalVector("offsets", new Vector4(0, 4.0f / Screen.height, 0, 0));
            m_cmdGenerateSceneCopyBuf.Blit(blurredID2, blurredID, m_Material);
            m_cmdGenerateSceneCopyBuf.SetGlobalTexture("_GrabBlurTexture", blurredID);
        }
        else
        {
            m_cmdGenerateSceneCopyBuf.name = "GenerateSceneCopy";
            int screenCopyID = Shader.PropertyToID("_GrabBlurTexture");
            m_cmdGenerateSceneCopyBuf.GetTemporaryRT(screenCopyID, -1, -1, 0, FilterMode.Bilinear);
            m_cmdGenerateSceneCopyBuf.Blit(BuiltinRenderTextureType.CurrentActive, screenCopyID);
        }
        camera.AddCommandBuffer(CameraEvent.AfterSkybox, m_cmdGenerateSceneCopyBuf);
        //camera.AddCommandBuffer(CameraEvent.AfterForwardAlpha, m_cmdBuf);
    }
    void Cleanup()
    {
        Camera camera = GetComponent<Camera>();
        if (m_cmdGenerateSceneCopyBuf != null)
        {
            camera.RemoveCommandBuffer(CameraEvent.AfterSkybox, m_cmdGenerateSceneCopyBuf);
            //camera.RemoveCommandBuffer(CameraEvent.AfterForwardAlpha, m_cmdBuf);
            m_cmdGenerateSceneCopyBuf = null;
        }
        if (m_cmdBloomAddBuf != null)
        {
            m_cmdBloomAddBuf.Clear();
            camera.RemoveCommandBuffer(CameraEvent.AfterSkybox, m_cmdBloomAddBuf);
        }
        if (m_bloomAddTexture != null)
        {
            DestroyImmediate(m_bloomAddTexture);
            m_bloomAddTexture = null;
            camera.targetTexture = null;
        }
    }
    public void EnableSceneCopyTextureBlur(bool isBlur)
    {
        if (isBlur)
        {
            m_enableSceneCopyBlurCount++;
        }
        else
        {
            m_enableSceneCopyBlurCount--;
        }
        RefreshSceneCopyCmd();
    }
    void OnEnable()
    {
        RefreshSceneCopyCmd();
        Instance = this;
    }
    void OnDisable()
    {
        Cleanup();
        if (Instance == this)
        {
            Instance = null;
        }
    }
    void OnDestroy()
    {
        Cleanup();
        if (Instance == this)
        {
            Instance = null;
        }
    }
    public void RemoveBloom(Renderer g)
    {
        m_bloomAddRenders.Remove(g);
    }
    public void AddBloom(Renderer g)
    {
        m_bloomAddRenders.Add(g);
    }
    void RefreshBloomAddCmd()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            return;
        }
        Camera camera = GetComponent<Camera>();
        if (m_cmdBloomAddBuf != null)
        {
            m_cmdBloomAddBuf.Clear();
            camera.RemoveCommandBuffer(CameraEvent.AfterSkybox, m_cmdBloomAddBuf);
        }
        else
        {
            m_cmdBloomAddBuf = new CommandBuffer();
            m_cmdBloomAddBuf.name = "GenerateBloom";
        }
        if (m_bloomAddTexture == null)
        {
            m_bloomAddTexture = new RenderTexture(m_width, m_height,0);
            m_replaceRenderTargetID = new RenderTargetIdentifier(m_bloomAddTexture);
        }
        m_cmdBloomAddBuf.SetRenderTarget(m_replaceRenderTargetID);
        m_cmdBloomAddBuf.ClearRenderTarget(false, true, Color.clear);
        for (int i = 0; i < m_bloomAddRenders.Count; i++)
        {
            Renderer r = m_bloomAddRenders[i];
            m_cmdBloomAddBuf.DrawRenderer(r, r.sharedMaterial);
        }
        camera.AddCommandBuffer(CameraEvent.AfterSkybox, m_cmdBloomAddBuf);
    }
    // Update is called once per frame
    void Update()
    {
        RefreshBloomAddCmd();
    }
}
