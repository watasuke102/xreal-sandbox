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
  Camera cam;

  RenderTexture texture;
  UInt32 texture_id = 0;

  void Start()
  {
    this.cam = GetComponent<Camera>();
    var width = this.cam.pixelWidth;
    var height = this.cam.pixelHeight;
    Debug.Log("[New Camera] " + this.cam.name + ": " + width + ", " + height);

    texture = new RenderTexture(width, height, 32, RenderTextureFormat.ARGB32);
    texture.filterMode = FilterMode.Point;
    texture.depthStencilFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.D24_UNorm_S8_UInt;
    texture.Create();
  }
  void Update()
  {
    if (texture_id == 0)
    {
      if (texture.IsCreated())
      {
        texture_id = drawer.RegisterTexture(ref texture);
        Debug.Log(">>> registerd : " + texture_id);
      }
      else
      {
        return;
      }
    }

    bool is_succeeded;
    var proj = NRFrame.GetEyeProjectMatrix(out is_succeeded, 0.01f, 100.0f);
    Matrix4x4 proj_mat;
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
      // from real XREAL Air 2 Pro; NRFrame.GetEyeProjectMatrix(_, 0.01f, 100.0f);
      proj_mat[0, 0] = +2.80947f; proj_mat[0, 1] = +0.00000f; proj_mat[0, 2] = +0.03892f; proj_mat[0, 3] = +0.00000f;
      proj_mat[1, 0] = +0.00000f; proj_mat[1, 1] = +5.02696f; proj_mat[1, 2] = +0.04567f; proj_mat[1, 3] = +0.00000f;
      proj_mat[2, 0] = +0.00000f; proj_mat[2, 1] = +0.00000f; proj_mat[2, 2] = -1.00020f; proj_mat[2, 3] = -0.02000f;
      proj_mat[3, 0] = +0.00000f; proj_mat[3, 1] = +0.00000f; proj_mat[3, 2] = -1.00000f; proj_mat[3, 3] = +0.00000f;
    }
    proj_mat = GL.GetGPUProjectionMatrix(proj_mat, true);

    var cam = GetComponent<Camera>();
    var view_mat = cam.worldToCameraMatrix;
    var vp_mat = proj_mat * view_mat;
    drawer.SetVP(texture_id, vp_mat);
  }

  void OnRenderImage(RenderTexture _, RenderTexture dst)
  {
    if (this.texture == null || !this.texture.IsCreated())
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
