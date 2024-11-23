using System;
using UnityEngine;

public class OverrideCamera : MonoBehaviour
{
  [SerializeField] Drawer drawer;
  Texture2D texture = null;
  UInt32 texture_id = 0;

  void Start()
  {
  }
  void Update()
  {
    var cam = GetComponent<Camera>();
    if (!cam) return;

    if (!texture)
    {
      Debug.Log(cam.name + ": " + cam.pixelWidth + ", " + cam.pixelHeight);
      texture = new Texture2D(cam.pixelWidth, cam.pixelHeight, TextureFormat.RGBA32, false);
      texture.filterMode = FilterMode.Point;
      Color c = new Color(0.60f, 0.76f, 0.48f);
      for (int x = 0; x < cam.pixelWidth; ++x)
      {
        for (int y = 0; y < cam.pixelHeight; ++y)
        {
          texture.SetPixel(x, y, c);
        }
      }
      texture.Apply();
      texture_id = drawer.RegisterTexture(texture);
      Debug.Log(">>> registerd : " + texture_id);
    }

    var m = transform.localToWorldMatrix;
    var v = cam.worldToCameraMatrix;
    var p = GL.GetGPUProjectionMatrix(cam.projectionMatrix, true);
    var mvp_mat = p * v * m;
    drawer.SetMVP(texture_id, mvp_mat);
  }

  void OnRenderImage(RenderTexture _, RenderTexture dst)
  {
    if (this.texture == null)
    {
      return;
    }
    Graphics.Blit(this.texture, dst);
  }
}
