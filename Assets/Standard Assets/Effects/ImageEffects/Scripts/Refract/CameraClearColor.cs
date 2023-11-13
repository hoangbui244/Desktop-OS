using UnityEngine;
using System.Collections;
public class CameraClearColor : MonoBehaviour {
    public Color clearColor = Color.black;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnPreRender()
    {
        GL.Clear(false, true, clearColor);
    }
}
