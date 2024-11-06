using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverrideCamera : MonoBehaviour
{
  void OnRenderImage(RenderTexture _, RenderTexture dst)
  {
    Graphics.Blit(Texture2D.redTexture, dst);
  }
}
