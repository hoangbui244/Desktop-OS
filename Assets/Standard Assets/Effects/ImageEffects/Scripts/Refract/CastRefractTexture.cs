using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class CastRefractTexture : MonoBehaviour {
    public Camera curCam;
    public Camera mainCamera;
    public Shader castRefractShader;
    public float refractTextureScale=1.0f;
    RenderTexture refractTex;
    public bool isRendering;
    public static CastRefractTexture Instance;
    void OnEnable()
    {
        Instance = this;
        if (curCam != null)
        {
            curCam.clearFlags = CameraClearFlags.Nothing;
            curCam.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0);
            curCam.depthTextureMode = DepthTextureMode.None;
            curCam.renderingPath = RenderingPath.Forward;
            InitCamera(curCam);
        }
        RenderObjects();
    }
    void OnDisable()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        Clear();
    }
    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        Clear();
    }
    public RenderTexture GetTargetTexture()
    {
        return refractTex;
    }
    void OnGUI()
    {
        //GUI.DrawTexture(new Rect(0, 0, Screen.width * 0.8f, Screen.height * 0.8f), refractTex);
    }
    void Clear()
    {
        if (refractTex)
        {
            DestroyImmediate(refractTex);
            refractTex = null;
        }
    }
    void InitCamera(Camera cam)
    {
        if (mainCamera != null)
        {
            cam.farClipPlane = mainCamera.farClipPlane;
            cam.nearClipPlane = mainCamera.nearClipPlane;
            cam.orthographic = mainCamera.orthographic;
            cam.fieldOfView = mainCamera.fieldOfView;
            cam.aspect = mainCamera.aspect;
            cam.orthographicSize = mainCamera.orthographicSize;
            cam.transform.position = mainCamera.transform.position;
            cam.transform.rotation = mainCamera.transform.rotation;
            mainCamera.depthTextureMode = DepthTextureMode.Depth;
        }
        if (!refractTex)
        {
            int shadowWidth = (int)(Screen.width * refractTextureScale);
            int shadowHeight = (int)(Screen.height * refractTextureScale);
            refractTex = new RenderTexture(shadowWidth, shadowHeight, 0);
            refractTex.hideFlags = HideFlags.DontSave;
        }
    }
    void LateUpdate()
    {
        //RenderObjects();
    }
    public void OnPostRender()
    {
        RenderObjects();
    }
    void RenderObjects()
    {
        if (isRendering)
        {
            return;
            //this.enabled = false;
        }
            
        isRendering = true;
        
        if (curCam == null)
        {
            isRendering = false;
            return;
        }
        
        //RenderTexture currentRT = RenderTexture.active;
        curCam.SetTargetBuffers(refractTex.colorBuffer, Graphics.activeDepthBuffer);
        InitCamera(curCam);
        curCam.targetTexture = refractTex;
        if(castRefractShader != null)
        {
            curCam.RenderWithShader(castRefractShader, "RenderType");
        }
        else
        {
            curCam.Render();
        }
        isRendering = false;
    }
}
