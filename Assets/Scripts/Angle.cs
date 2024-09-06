using UnityEngine;


public struct Angle
{
  /// <summary>
  /// polar angle within [-theta_max, theta_max] deg
  /// </summary>
  public float theta { get; set; }
  const float theta_max = 80f;
  /// <summary>
  /// azimuthal angle (in xz-plane)
  /// </summary>
  public float phi { get; set; }

  public Angle(float theta, float phi) { this.theta = theta; this.phi = phi; }
  public Angle from_vec3(Vector3 from)
  {
    // [x, y, z] = [-cos(theta)sin(phi), sin(theta), cos(theta)cos(phi)]
    return new Angle(Mathf.Asin(from.y), Mathf.Atan2(from.x, from.z));
  }
  public static Angle operator +(Angle a, Angle b)
  {
    return new Angle(
      Mathf.Clamp(a.theta + b.theta, -theta_max, theta_max),
      a.phi + b.phi
    );
  }
}
