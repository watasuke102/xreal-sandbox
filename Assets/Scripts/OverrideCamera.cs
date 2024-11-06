using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://gamedev.stackexchange.com/questions/98632/drawing-a-pixel-to-rendertexture
public class OverrideCamera : MonoBehaviour
{
  void OnRenderImage(RenderTexture _, RenderTexture dst)
  {
    Graphics.Blit(Texture2D.redTexture, dst);
  }
}
