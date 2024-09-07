using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;
using Unity.VisualScripting;
using TMPro;

public class ObjectGrabManipulator : MonoBehaviour
{
  [SerializeField] GameObject ui_text;
  const float radius = 1.5f;

  TextMeshProUGUI text;
  ObjectBase selected = null;
  Vector3 diff;
  Transform tmp_transform;

  void Start()
  {
    this.text = ui_text.GetComponent<TextMeshProUGUI>();
    this.tmp_transform = new GameObject().transform;
    this.tmp_transform.position = Vector3.zero;
    Debug.Log(this.text);
    NRInput.AddDownListener(ControllerHandEnum.Right, ControllerButton.TRIGGER, HandleInputDown);
    NRInput.AddPressingListener(ControllerHandEnum.Right, ControllerButton.TRIGGER, HandleInputPressing);
    NRInput.AddUpListener(ControllerHandEnum.Right, ControllerButton.TRIGGER, HandleInputUp);
  }
  void Update()
  {
    var anchor = NRInput.AnchorsHelper.GetAnchor(ControllerAnchorEnum.RightLaserAnchor);
    this.text.text = $"{anchor.transform.forward,5:F3}";
  }

  void RemoveSelection()
  {
    SetSelectedColor(Color.white);
    this.selected = null;
  }
  void SetSelectedColor(Color color)
  {
    if (!this.selected) { return; }
    try
    {
      foreach (var child in this.selected.GetComponentsInChildren<MeshRenderer>())
      {
        child.material.color = color;
      }
      this.selected.transform.GetComponent<MeshRenderer>().material.color = color;
    }
    catch (System.Exception)
    { }
  }

  void HandleInputDown()
  {
    RaycastHit hit;
    var anchor = NRInput.AnchorsHelper.GetAnchor(ControllerAnchorEnum.RightLaserAnchor);
    if (Physics.Raycast(anchor.transform.position, anchor.transform.forward, out hit))
    {
      this.selected = hit.transform.GetComponent<ObjectBase>();
    }
    if (!this.selected) { return; }
    // this.diff = hit.point - this.selected.transform.position;
    this.diff = anchor.transform.forward * radius - this.selected.transform.position;
    this.SetSelectedColor(Color.green);
  }
  void HandleInputPressing()
  {
    if (!this.selected) { return; }
    var anchor = NRInput.AnchorsHelper.GetAnchor(ControllerAnchorEnum.RightLaserAnchor);
    this.tmp_transform.rotation = anchor.transform.rotation;
    this.selected.transform.position = this.tmp_transform.forward * radius - this.diff;
    this.selected.transform.LookAt(Vector3.zero);
    this.selected.transform.Rotate(0, 180, 0);

    Debug.DrawRay(this.tmp_transform.position, this.tmp_transform.forward * 2f, Color.yellow);
  }
  void HandleInputUp()
  {
    this.RemoveSelection();
  }
}
