using System;
using System.Collections;
using System.Runtime.InteropServices;
using NRKernal;
using UnityEngine;

public class Drawer : MonoBehaviour
{
#if UNITY_EDITOR
  [DllImport("drawer")]
#else
  [DllImport("drawer_android")]
#endif
  private static extern UInt32 register_texture(System.IntPtr texture, Int32 width, Int32 height);

#if UNITY_EDITOR
  [DllImport("drawer")]
#else
  [DllImport("drawer_android")]
#endif
  private static extern void set_vp(UInt32 id,//
           float x0, float y0, float z0, float w0, //
           float x1, float y1, float z1, float w1, //
           float x2, float y2, float z2, float w2, //
           float x3, float y3, float z3, float w3  //
  );

#if UNITY_EDITOR
  [DllImport("drawer")]
#else
  [DllImport("drawer_android")]
#endif
  private static extern IntPtr get_render_handler_ptr();

#if UNITY_EDITOR
  [DllImport("drawer")]
#else
  [DllImport("drawer_android")]
#endif
  private static extern void unregister_texture(UInt32 id);

  public UInt32 RegisterTexture(ref Texture2D texture)
  {
    return register_texture(texture.GetNativeTexturePtr(), texture.width, texture.height);
  }
  public void UnregisterTexture(UInt32 id)
  {
    unregister_texture(id);
  }
  public void SetVP(UInt32 texture_id, Matrix4x4 vp)
  {
    set_vp(
      texture_id, //
      vp[0, 0], vp[0, 1], vp[0, 2], vp[0, 3], //
      vp[1, 0], vp[1, 1], vp[1, 2], vp[1, 3], //
      vp[2, 0], vp[2, 1], vp[2, 2], vp[2, 3], //
      vp[3, 0], vp[3, 1], vp[3, 2], vp[3, 3]  //
    );
  }

  IEnumerator Start()
  {
    NRDebugger.logLevel = LogLevel.Warning;
    yield return StartCoroutine(draw_coroutine());
  }
  int app_clicked = 0;
  void Update()
  {
    if (NRInput.GetButtonDown(ControllerButton.APP))
    {
      ++app_clicked;
      if (app_clicked >= 2)
      {
        Application.Quit();
      }
    }
  }
  private IEnumerator draw_coroutine()
  {
    while (true)
    {
      yield return new WaitForEndOfFrame();
      GL.IssuePluginEvent(get_render_handler_ptr(), 1);
    }
  }
}
