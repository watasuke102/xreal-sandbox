using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
  public int GetTexture(bool is_left_side_camera)
  {
    if (is_left_side_camera)
    {
      return 111;
    }
    else
    {
      return 8;
    }
  }
  public void SetMVP(bool is_left_side_camera, Matrix4x4 mvp)
  {
    // TODO
  }
}
