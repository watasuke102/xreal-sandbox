using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverrideCamera : MonoBehaviour
{
  [SerializeField] Drawer drawer;
  [SerializeField] bool is_left_side_camera;

  Texture2D texture = null;

  void Start()
  {
    Debug.Log(drawer.GetTexture(this.is_left_side_camera));
  }
  void Update()
  {
    if (texture) return;
    var cam = GetComponent<Camera>();
    if (!cam) return;
    Debug.Log(cam.name + ": " + cam.pixelWidth + ", " + cam.pixelHeight);
    texture = new Texture2D(cam.pixelWidth, cam.pixelHeight);
    Color c = is_left_side_camera ? new Color(0.88f, 0.42f, 0.46f) : new Color(0.60f, 0.76f, 0.48f);
    for (int x = 0; x < cam.pixelWidth; ++x)
    {
      for (int y = 0; y < cam.pixelHeight; ++y)
      {
        texture.SetPixel(x, y, c);
      }
    }
    texture.Apply();
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
