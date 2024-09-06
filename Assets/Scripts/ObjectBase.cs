using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
  [SerializeField] float initial_phi;
  static float theta_threshold = 20f;
  static float radius = 1.5f;
  Angle angle;

  /// convert polar coordinate paramaters
  public void ApplyTransform()
  {
    this.transform.position = Vector3.forward * radius;
    this.transform.rotation = Quaternion.identity;
    // either high or low enough
    if (Mathf.Abs(this.angle.theta) > theta_threshold)
    {
      this.transform.RotateAround(Vector3.zero, Vector3.left, this.angle.theta);
    }
    else
    {
      this.transform.position += Vector3.up * radius * Mathf.Sin(this.angle.theta * Mathf.Deg2Rad);
    }
    this.transform.RotateAround(Vector3.zero, Vector3.up, this.angle.phi);
  }
  public void AddAngle(Angle amount)
  {
    this.angle += amount;
    this.ApplyTransform();
  }

  void Start()
  {
    this.angle = new Angle(0, this.initial_phi);
    this.ApplyTransform();
  }
}
