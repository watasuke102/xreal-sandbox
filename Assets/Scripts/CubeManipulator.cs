using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;
using Unity.VisualScripting;

public class CubeManipulator : MonoBehaviour
{
  [SerializeField] float unselect_delta = 0.3f;

  bool is_first_click = true;
  GameObject selected = null;

  void Start()
  {
    NRInput.AddDownListener(ControllerHandEnum.Right, ControllerButton.TRIGGER, HandleInputDown);
    NRInput.AddClickListener(ControllerHandEnum.Right, ControllerButton.TRIGGER, HandleInputClick);
  }
  void Update()
  {
    if (this.selected)
    {
      // Debug.Log(NRInput.GetDeltaTouch().x);
    }
  }

  void RemoveSelection()
  {
    this.selected.transform.GetComponent<MeshRenderer>().material.color = Color.white;
    this.selected = null;
  }

  void HandleInputDown()
  {
    RaycastHit hit;
    var anchor = NRInput.AnchorsHelper.GetAnchor(ControllerAnchorEnum.RightLaserAnchor);
    if (Physics.Raycast(anchor.transform.position, anchor.transform.forward, out hit))
    {
      if (this.selected)
      {
        if (this.selected == hit.transform.gameObject)
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
      this.selected = hit.transform.gameObject;
      this.selected.transform.GetComponent<MeshRenderer>().material.color = Color.green;
    }
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
