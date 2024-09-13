using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NRKernal;
using TMPro;
using UnityEngine;

enum CurrentState
{
  Init,
  AcceptedUpHand,
  AcceptedHorizontalHand,
  // AcceptedDownHand,
}

public class HandTrackMenu : MonoBehaviour
{
  [SerializeField] GameObject menuCanvas;
  [SerializeField] GameObject cameraRig;
  [SerializeField] GameObject logObj;

  CurrentState current_state = CurrentState.Init;
  Vector3 gesture_beginning_pos;

  void Update()
  {
    var hand = NRInput.Hands.GetHandState(HandEnum.RightHand);
    var index_pose = hand.GetJointPose(HandJointID.IndexTip);
    var middle_pose = hand.GetJointPose(HandJointID.MiddleTip);

    var angle_up_index = Vector3.Angle(Vector3.up, index_pose.up);
    var angle_up_middle = Vector3.Angle(Vector3.up, middle_pose.up);
    var angle_horizontal_index = Vector3.Angle(this.cameraRig.transform.forward, index_pose.up);
    var angle_horizontal_middle = Vector3.Angle(this.cameraRig.transform.forward, middle_pose.up);
    var angle_down_index = Vector3.Angle(Vector3.down, index_pose.up);
    var angle_down_middle = Vector3.Angle(Vector3.down, middle_pose.up);

    {
      var label = this.logObj.GetComponent<TextMeshProUGUI>();
      label.text = "";
      Pose[] poses = new Pose[5];
      poses[0] = (hand.GetJointPose(HandJointID.ThumbTip));
      poses[1] = (hand.GetJointPose(HandJointID.IndexTip));
      poses[2] = (hand.GetJointPose(HandJointID.MiddleTip));
      poses[3] = (hand.GetJointPose(HandJointID.RingTip));
      poses[4] = (hand.GetJointPose(HandJointID.PinkyTip));
      foreach (var pose in poses)
      {
        label.text += $"({pose.position.x,8:F2}, {pose.position.y,8:F2}, {pose.position.z,8:F2}) / ";
        label.text += $"({pose.rotation.eulerAngles.x,8:F2}, {pose.rotation.eulerAngles.y,8:F2}, {pose.rotation.eulerAngles.z,8:F2})\n";
      }
      label.text += "\n";
      label.text += $"up   = {angle_up_index,6:F2}, {angle_up_middle,6:F2}\n";
      label.text += $"hor  = {angle_horizontal_index,6:F2}, {angle_horizontal_middle,6:F2}\n";
      label.text += $"down = {angle_down_index,6:F2}, {angle_down_middle,6:F2}\n";
      label.text += $"stat = {this.current_state}\n";
      label.text += $"diff = {this.gesture_beginning_pos - index_pose.position,7:F2}\n";
      label.text += $"ge   = {hand.currentGesture}\n";

      Debug.DrawRay(index_pose.position, index_pose.up, Color.green);
      Debug.DrawRay(index_pose.position, Vector3.up, Color.red);
      Debug.DrawRay(index_pose.position, this.cameraRig.transform.forward, Color.yellow);
      Debug.DrawRay(index_pose.position, Vector3.down, Color.blue);
      var p = hand.GetJointPose(HandJointID.Palm);
      Debug.DrawRay(p.position, p.forward, Color.cyan);
    }

    switch (this.current_state)
    {
      // case CurrentState.AcceptedDownHand:
      //   goto case CurrentState.Init;
      case CurrentState.AcceptedHorizontalHand:
        //   // down gesture tend to go out of the hand tracking detection area
        //   // so make the threshold larger
        //   if (angle_down_index < 40f && angle_down_middle < 40f && )
        //   {
        //     this.current_state = CurrentState.AcceptedDownHand;
        //     this.menuCanvas.SetActive(true);
        //   }
        goto case CurrentState.Init;
      case CurrentState.AcceptedUpHand:
        if (angle_horizontal_index < 35f && angle_horizontal_middle < 35f && (this.gesture_beginning_pos - index_pose.position).y > 0.12f)
        {
          this.current_state = CurrentState.AcceptedHorizontalHand;
          this.menuCanvas.transform.position = index_pose.position;
          this.menuCanvas.transform.position += -index_pose.right * 0.07f;
          this.menuCanvas.transform.LookAt(cameraRig.transform);
          this.menuCanvas.transform.Rotate(0, 180, 0);
          this.menuCanvas.SetActive(true);
        }
        goto case CurrentState.Init;
      case CurrentState.Init:
        if (angle_up_index < 55f && angle_up_middle < 55f)
        {
          this.current_state = CurrentState.AcceptedUpHand;
          this.gesture_beginning_pos = index_pose.position;
        }
        break;
    }
    if (this.current_state != CurrentState.AcceptedHorizontalHand)
    {
      this.menuCanvas.SetActive(false);
    }
  }
}
