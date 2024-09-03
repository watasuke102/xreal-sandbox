using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
  [SerializeField] float initial_phi;
  static float theta_threshold = 20f;
  static float radius = 1.5f;
  static float theta_max = 80f;
  /// <summary>
  /// polar angle within [-theta_max, theta_max] deg
  /// </summary>
  public float theta;
  /// <summary>
  /// azimuthal angle (in xz-plane)
  /// </summary>
  public float phi;

  /// convert polar coordinate paramaters
  public void ApplyTransform()
  {
    this.transform.position = Vector3.forward * radius;
    this.transform.rotation = Quaternion.identity;
    // either high or low enough
    if (Mathf.Abs(this.theta) > theta_threshold)
    {
      this.transform.RotateAround(Vector3.zero, Vector3.left, this.theta);
    }
    else
    {
      this.transform.position += Vector3.up * radius * Mathf.Sin(theta * Mathf.Deg2Rad);
    }
    this.transform.RotateAround(Vector3.zero, Vector3.up, this.phi);
  }
  public void AddTheta(float amount)
  {
    this.theta = Mathf.Clamp(this.theta + amount, -theta_max, theta_max);
    this.ApplyTransform();
  }
  public void AddPhi(float amount)
  {
    this.phi += amount;
    this.ApplyTransform();
  }

  void Start()
  {
    this.phi = this.initial_phi;
    this.ApplyTransform();
  }
}
