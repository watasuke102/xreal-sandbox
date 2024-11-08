using UnityEngine;
using System.Runtime.InteropServices;
using System;
using TMPro;

public class CallCpp : MonoBehaviour
{
  [SerializeField] GameObject lhsObj;
  [SerializeField] GameObject rhsObj;
  [SerializeField] GameObject labelObj;

#if UNITY_EDITOR
  [DllImport("cppadd-x86_64")]
#else
  [DllImport("cppadd-aarch64")]
#endif
  private static extern Int32 cpp_add(Int32 x, Int32 y);

  public void Calc()
  {
    var label = this.labelObj.GetComponent<TextMeshProUGUI>();
    try
    {
      var lhs = Int32.Parse(this.lhsObj.GetComponent<TMP_InputField>().text);
      var rhs = Int32.Parse(this.rhsObj.GetComponent<TMP_InputField>().text);
      label.text = cpp_add(lhs, rhs).ToString();
    }
    catch (System.Exception e)
    {
      label.text = e.Message;
      throw;
    }
  }
}
