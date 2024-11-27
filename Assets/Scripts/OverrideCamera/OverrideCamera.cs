using System;
using NRKernal;
using UnityEngine;

public enum EyeType
{
  Left, Center, Right,
}

public class OverrideCamera : MonoBehaviour
{
  [SerializeField] Drawer drawer;
  [SerializeField] EyeType eye_type;
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
      Debug.Log("[New Camera] " + cam.name + ": " + cam.pixelWidth + ", " + cam.pixelHeight);
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
      texture_id = drawer.RegisterTexture(ref texture);
      Debug.Log(">>> registerd : " + texture_id);
    }

    bool is_succeeded;
    var proj = NRFrame.GetEyeProjectMatrix(out is_succeeded, 0.01f, 100.0f);
    Matrix4x4 proj_mat;
    // is_succeeded = false; // debug
    if (is_succeeded)
    {
      switch (this.eye_type)
      {
        case EyeType.Left: proj_mat = proj.LEyeMatrix; break;
        case EyeType.Center: proj_mat = proj.CEyeMatrix; break;
        case EyeType.Right: proj_mat = proj.REyeMatrix; break;
        default:
          Debug.LogError($"Invalid eye_type of `{this.name}` : {this.eye_type}");
          return;
      }
    }
    else
    {
      proj_mat = Matrix4x4.zero;
      // from real XREAL Air 2 Pro; NRFrame.GetEyeProjectMatrix(out is_succeeded, 0.01f, 100.0f);
      // set [0, 2] and [1, 2] to 0, swap [2, 3] and [3, 2] => works fine
      // without setting zero => weird clipping
      // proj_mat[0, 0] = +2.80947f; proj_mat[0, 1] = +0.00000f; proj_mat[0, 2] = +0.03892f; proj_mat[0, 3] = +0.00000f;
      // proj_mat[1, 0] = +0.00000f; proj_mat[1, 1] = +5.02696f; proj_mat[1, 2] = +0.04567f; proj_mat[1, 3] = +0.00000f;
      // proj_mat[2, 0] = +0.00000f; proj_mat[2, 1] = +0.00000f; proj_mat[2, 2] = -1.00020f; proj_mat[2, 3] = -0.02000f;
      // proj_mat[3, 0] = +0.00000f; proj_mat[3, 1] = +0.00000f; proj_mat[3, 2] = -1.00000f; proj_mat[3, 3] = +0.00000f;

      // glm::mat4 proj_mat = glm::perspective(
      //   glm::pi<float>() / 3, (float)WIDTH / HEIGHT, 0.01f, 100.f
      // );
      // proj_mat[0, 0] = 1.299038f; proj_mat[0, 1] = 0.000000f; proj_mat[0, 2] = +0.000000f; proj_mat[0, 3] = +0.000000f;
      // proj_mat[1, 0] = 0.000000f; proj_mat[1, 1] = 1.732051f; proj_mat[1, 2] = +0.000000f; proj_mat[1, 3] = +0.000000f;
      // proj_mat[2, 0] = 0.000000f; proj_mat[2, 1] = 0.000000f; proj_mat[2, 2] = -1.000200f; proj_mat[2, 3] = -1.000000f;
      // proj_mat[3, 0] = 0.000000f; proj_mat[3, 1] = 0.000000f; proj_mat[3, 2] = -0.020002f; proj_mat[3, 3] = +0.000000f;

      // glm::perspective(glm::pi<float>() / 8, (float)16 / 9, 0.01f, 100.f);
      proj_mat[0, 0] = 2.827878f; proj_mat[0, 1] = 0.000000f; proj_mat[0, 2] = +0.000000f; proj_mat[0, 3] = +0.000000f;
      proj_mat[1, 0] = 0.000000f; proj_mat[1, 1] = 5.027339f; proj_mat[1, 2] = +0.000000f; proj_mat[1, 3] = +0.000000f;
      proj_mat[2, 0] = 0.000000f; proj_mat[2, 1] = 0.000000f; proj_mat[2, 2] = -1.000200f; proj_mat[2, 3] = -1.000000f;
      proj_mat[3, 0] = 0.000000f; proj_mat[3, 1] = 0.000000f; proj_mat[3, 2] = -0.020002f; proj_mat[3, 3] = +0.000000f;

      // 平行投影
      // proj_mat[0, 0] = 0.5f;        // 2 / (right - left)
      // proj_mat[0, 1] = 0.0f;
      // proj_mat[0, 2] = 0.0f;
      // proj_mat[0, 3] = 0.0f;

      // proj_mat[1, 0] = 0.0f;
      // proj_mat[1, 1] = 0.6666667f;  // 2 / (top - bottom)
      // proj_mat[1, 2] = 0.0f;
      // proj_mat[1, 3] = 0.0f;

      // proj_mat[2, 0] = 0.0f;
      // proj_mat[2, 1] = 0.0f;
      // proj_mat[2, 2] = -0.020002f;  // -2 / (far - near)
      // proj_mat[2, 3] = -1.0002f;    // -(far + near) / (far - near)

      // proj_mat[3, 0] = 0.0f;
      // proj_mat[3, 1] = 0.0f;
      // proj_mat[3, 2] = 0.0f;
      // proj_mat[3, 3] = 1.0f;
    }
    proj_mat = GL.GetGPUProjectionMatrix(proj_mat, true);
    Debug.Log(proj_mat);
    var view_mat = cam.worldToCameraMatrix;
    drawer.SetVP(texture_id, proj_mat * view_mat);
  }

  void OnRenderImage(RenderTexture _, RenderTexture dst)
  {
    if (this.texture == null)
    {
      return;
    }
    Graphics.Blit(this.texture, dst);
  }

  void OnDestroy()
  {
    if (texture_id != 0)
    {
      drawer.UnregisterTexture(texture_id);
    }
  }
}
