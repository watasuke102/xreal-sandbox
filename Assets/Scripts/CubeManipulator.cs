using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;
using Unity.VisualScripting;

enum CurrentMoveMode
{
  None, Theta, Phi
}

public class CubeManipulator : MonoBehaviour
{
  [SerializeField] float speed = 50.0f;
  [SerializeField] float move_start_threshold = 0.005f;
  [SerializeField] float max_click_duration = 0.2f;

  bool is_first_click = true;
  float click_start_time;
  ObjectBase selected = null;
  CurrentMoveMode currentMoveMode = CurrentMoveMode.None;

  void Start()
  {
    NRInput.AddDownListener(ControllerHandEnum.Right, ControllerButton.TRIGGER, HandleInputDown);
    NRInput.AddPressingListener(ControllerHandEnum.Right, ControllerButton.TRIGGER, HandleInputPressing);
    NRInput.AddUpListener(ControllerHandEnum.Right, ControllerButton.TRIGGER, HandleInputUp);
  }
  void Update()
  {
    if (!this.selected) { return; }
    switch (this.currentMoveMode)
    {
      case CurrentMoveMode.Phi:
        this.selected.AddPhi(NRInput.GetDeltaTouch().x * this.speed);
        break;
      case CurrentMoveMode.Theta:
        this.selected.AddTheta(NRInput.GetDeltaTouch().y * this.speed);
        break;
    }
    this.selected.ApplyTransform();
  }

  void RemoveSelection()
  {
    this.currentMoveMode = CurrentMoveMode.None;
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
    this.click_start_time = Time.time;
    RaycastHit hit;
    var anchor = NRInput.AnchorsHelper.GetAnchor(ControllerAnchorEnum.RightLaserAnchor);
    if (Physics.Raycast(anchor.transform.position, anchor.transform.forward, out hit))
    {
      if (this.selected)
      {
        if (this.selected.gameObject == hit.transform.gameObject)
        {
          // exit handler for now, removing process will be executed in HandleInputClick when tapping is completed
          return;
        }
        else
        {
          // start re-sellecting
          this.RemoveSelection();
        }
      }
      Debug.DrawRay(anchor.transform.position, anchor.transform.forward * 1000, Color.yellow);
      this.is_first_click = true;
      try
      {
        this.selected = hit.transform.GetComponent<ObjectBase>();
      }
      catch (System.Exception)
      {
        return;
      }
      this.SetSelectedColor(Color.green);
    }
  }
  void HandleInputPressing()
  {
    var delta_x = Mathf.Abs(NRInput.GetDeltaTouch().x);
    var delta_y = Mathf.Abs(NRInput.GetDeltaTouch().y);
    if (this.currentMoveMode != CurrentMoveMode.None || (delta_x + delta_y) < move_start_threshold)
    {
      return;
    }

    if (delta_x > delta_y)
    {
      this.currentMoveMode = CurrentMoveMode.Phi;
    }
    else
    {
      this.currentMoveMode = CurrentMoveMode.Theta;
    }
    Debug.Log($"({delta_x}, {delta_y}) -> Mode Initialized : {this.currentMoveMode}");
  }
  void HandleInputUp()
  {
    if (this.is_first_click)
    {
      this.is_first_click = false;
      return;
    }
    if (this.selected && this.currentMoveMode == CurrentMoveMode.None &&
      (Time.time - this.click_start_time) < this.max_click_duration)
    {
      this.RemoveSelection();
    }
    this.currentMoveMode = CurrentMoveMode.None;
  }
  void HandleInputClick()
  {
    if (this.is_first_click)
    {
      this.is_first_click = false;
      return;
    }
    if (this.selected)
    {
      this.RemoveSelection();
    }
  }
}
