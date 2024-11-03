using UnityEngine;
using System.Runtime.InteropServices;
using System;
using TMPro;
using System.Collections;

public class CallDrawLib : MonoBehaviour
{
#if UNITY_EDITOR
  [DllImport("glesdraw")]
  // [DllImport("__Internal")]
#else
    [DllImport("glesdraw_android")]
    // [DllImport("__Internal")]
#endif
  private static extern void set_mvp(
  float m0, float m1, float m2, float m3,
  float m4, float m5, float m6, float m7,
  float m8, float m9, float m10, float m11,
  float m12, float m13, float m14, float m15
);
#if UNITY_EDITOR
  [DllImport("glesdraw")]
  // [DllImport("__Internal")]
#else
    [DllImport("glesdraw_android")]
    // [DllImport("__Internal")]
#endif
  private static extern IntPtr get_draw_fn_ptr();

#if UNITY_EDITOR
  [DllImport("glesdraw")]
  // [DllImport("__Internal")]
#else
    [DllImport("glesdraw_android")]
    // [DllImport("__Internal")]
#endif
  private static extern int get_cnt();

  [SerializeField] GameObject cameraRig;
  [SerializeField] GameObject labelObj;

  TextMeshProUGUI label;
  int stat = 1;

  public void Run()
  {
    try
    {
      var cam = cameraRig.GetComponent<Camera>();
      var m = transform.localToWorldMatrix;
      var v = cam.worldToCameraMatrix;
      var p =
       GL.GetGPUProjectionMatrix(cam.projectionMatrix, true);
      //  cam.projectionMatrix;
      Debug.Log($"[M]\n{m}");
      Debug.Log($"[V]\n{v}");
      Debug.Log($"[P]\n{p}");
      if (this.stat == 1)
      {
        this.stat = 2;
        this.label.text = "2";
      }
    }
    catch (System.Exception e)
    {
      this.label.text = e.Message;
      throw;
    }
  }

  IEnumerator Start()
  {
    this.label = this.labelObj.GetComponent<TextMeshProUGUI>();
    yield return StartCoroutine(draw_coroutine());
  }
  private IEnumerator draw_coroutine()
  {
    while (true)
    {
      // if (this.stat != 2) { continue; }
      yield return new WaitForEndOfFrame();
      try
      {
        var cam = cameraRig.GetComponent<Camera>();
        var m = transform.localToWorldMatrix;
        var v = cam.worldToCameraMatrix;
        var p =
         GL.GetGPUProjectionMatrix(cam.projectionMatrix, true);
        //  cam.projectionMatrix;
        // Debug.Log($"[M]\n{m}");
        // Debug.Log($"[V]\n{v}");
        // Debug.Log($"[P]\n{p}");
        var mvp_mat = p * v * m;
        // Debug.Log($"[MVP]\n{mvp_mat}");
        set_mvp(
          mvp_mat[0, 0], mvp_mat[0, 1], mvp_mat[0, 2], mvp_mat[0, 3],
          mvp_mat[1, 0], mvp_mat[1, 1], mvp_mat[1, 2], mvp_mat[1, 3],
          mvp_mat[2, 0], mvp_mat[2, 1], mvp_mat[2, 2], mvp_mat[2, 3],
          mvp_mat[3, 0], mvp_mat[3, 1], mvp_mat[3, 2], mvp_mat[3, 3]
        );
        GL.IssuePluginEvent(get_draw_fn_ptr(), 1);
        this.label.text = $"{get_cnt()}";
      }
      catch (System.Exception e)
      {
        this.label.text = e.Message;
        throw;
      }
    }
  }
}
