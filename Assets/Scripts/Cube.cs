using System.Collections;
using System.Collections.Generic;
using NRKernal;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cube : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
  private MeshRenderer mesh_renderer;
  // Start is called before the first frame update
  void Awake()
  {
    mesh_renderer = transform.GetComponent<MeshRenderer>();
  }

  //when pointer click, set the cube color to random color
  public void OnPointerClick(PointerEventData eventData)
  {
    mesh_renderer.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
  }

  //when pointer hover, set the cube color to green
  public void OnPointerEnter(PointerEventData eventData)
  {
    mesh_renderer.material.color = Color.green;
  }

  //when pointer exit hover, set the cube color to white
  public void OnPointerExit(PointerEventData eventData)
  {
    mesh_renderer.material.color = Color.white;
  }
}
