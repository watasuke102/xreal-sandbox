using System;
using System.Runtime.InteropServices;
using NRKernal;
using UnityEngine;

public enum EyeType
{
  Left, Center, Right,
}

class Constant
{
#if UNITY_EDITOR
  public const string DrawerLibName = "drawer";
#else
  public const string DrawerLibName = "drawer_android";
#endif
}

public class OverrideCamera : MonoBehaviour
{
  [SerializeField] EyeType eye_type;
  Camera cam;

  Int32 texture_id = 0;

  [DllImport(Constant.DrawerLibName)]
  private static extern IntPtr get_render_handler_ptr();
  [DllImport(Constant.DrawerLibName)]
  private static extern Int32 register_camera(Int32 width, Int32 height);
  [DllImport(Constant.DrawerLibName)]
  private static extern void unregister_camera(Int32 id);
  [DllImport(Constant.DrawerLibName)]
  private static extern void set_vp(Int32 id,//
           float x0, float y0, float z0, float w0, //
           float x1, float y1, float z1, float w1, //
           float x2, float y2, float z2, float w2, //
           float x3, float y3, float z3, float w3  //
  );

  void Start()
  {
    this.cam = GetComponent<Camera>();
    var width = this.cam.pixelWidth;
    var height = this.cam.pixelHeight;
    this.texture_id = register_camera(width, height);
    Debug.Log($"[New Camera] {this.cam.name}: pixel={width}x{height}");
  }
  void Update()
  {
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

    var view_mat = this.cam.worldToCameraMatrix;
    var vp = proj_mat * view_mat;
    set_vp(this.texture_id, //
      vp[0, 0], vp[0, 1], vp[0, 2], vp[0, 3], //
      vp[1, 0], vp[1, 1], vp[1, 2], vp[1, 3], //
      vp[2, 0], vp[2, 1], vp[2, 2], vp[2, 3], //
      vp[3, 0], vp[3, 1], vp[3, 2], vp[3, 3]  //
    );
  }

  void OnPostRender()
  {
    GL.Clear(true, true, Color.clear);
    GL.IssuePluginEvent(get_render_handler_ptr(), this.texture_id);
  }

  void OnDestroy()
  {
    if (texture_id != 0)
    {
      unregister_camera(texture_id);
    }
  }
}
