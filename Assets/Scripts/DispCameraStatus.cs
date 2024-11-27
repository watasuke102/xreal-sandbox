using System.Collections;
using System.Collections.Generic;
using NRKernal;
using TMPro;
using UnityEngine;

public class DispCameraStatus : MonoBehaviour
{
  [SerializeField] GameObject cameraRig;
  [SerializeField] bool is_left;
  TextMeshProUGUI label;

  void Start()
  {
    this.label = GetComponent<TextMeshProUGUI>();
  }
  void Update()
  {
    var cam = this.cameraRig.GetComponent<Camera>();
    // var m = transform.localToWorldMatrix;
    var v = cam.worldToCameraMatrix;

    bool is_succeeded;
    var proj_list = NRFrame.GetEyeProjectMatrix(out is_succeeded, 0.01f, 100.0f);
    var p = Matrix4x4.zero;
    if (is_succeeded)
    {
      if (this.is_left)
      {
        p = proj_list.LEyeMatrix;
      }
      {
        p = proj_list.REyeMatrix;
      }
      p = GL.GetGPUProjectionMatrix(p, true);
    }

    var vp = p * v;
    var text = "";
    text += "[vp]\n" + vp.ToString() + "\n";
    text += "[v]\n" + v.ToString();
    text += "[p]\n" + p.ToString();
    text += "\n" + cam.transform.eulerAngles.ToString();
    this.label.text = text;
    Debug.Log(text);
  }
}
