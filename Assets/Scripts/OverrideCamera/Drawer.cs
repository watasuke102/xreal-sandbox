using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class Drawer : MonoBehaviour
{
  [DllImport("drawer_android")]
  private static extern UInt32 register_texture(System.IntPtr texture, Int32 width, Int32 height);
  [DllImport("drawer_android")]
  private static extern void set_mvp(UInt32 id,//
           float x0, float y0, float z0, float w0, //
           float x1, float y1, float z1, float w1, //
           float x2, float y2, float z2, float w2, //
           float x3, float y3, float z3, float w3  //
  );
  [DllImport("drawer_android")]
  private static extern IntPtr get_render_handler_ptr();

  public UInt32 RegisterTexture(Texture2D texture)
  {
    return register_texture(texture.GetNativeTexturePtr(), texture.width, texture.height);
  }
  public void SetMVP(UInt32 texture_id, Matrix4x4 mvp)
  {
    set_mvp(
      texture_id, //
      mvp[0, 0], mvp[0, 1], mvp[0, 2], mvp[0, 3],
      mvp[1, 0], mvp[1, 1], mvp[1, 2], mvp[1, 3],
      mvp[2, 0], mvp[2, 1], mvp[2, 2], mvp[2, 3],
      mvp[3, 0], mvp[3, 1], mvp[3, 2], mvp[3, 3]
    );
  }

  IEnumerator Start()
  {
    yield return StartCoroutine(draw_coroutine());
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
