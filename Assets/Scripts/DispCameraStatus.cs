using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DispCameraStatus : MonoBehaviour
{
  [SerializeField] GameObject cameraRig;
  TextMeshProUGUI label;
  void Start()
  {
    this.label = GetComponent<TextMeshProUGUI>();
  }
  void Update()
  {
    var cam = this.cameraRig.GetComponent<Camera>();
    var m = transform.localToWorldMatrix;
    var v = cam.worldToCameraMatrix;
    var p =
     GL.GetGPUProjectionMatrix(cam.projectionMatrix, true);
    var mvp = p * v * m;
    this.label.text = mvp.ToString() + "\n";
    this.label.text += "\n" + m.ToString();
    this.label.text += "\n" + v.ToString();
    this.label.text += "\n" + p.ToString();
  }
}
