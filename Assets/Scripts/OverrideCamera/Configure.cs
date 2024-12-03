using NRKernal;
using UnityEngine;

public class Configure : MonoBehaviour
{
  void Start()
  {
    NRDebugger.logLevel = LogLevel.Warning;
  }

  int app_clicked = 0;
  void Update()
  {
    if (NRInput.GetButtonDown(ControllerButton.APP))
    {
      ++app_clicked;
      if (app_clicked >= 2)
      {
        Application.Quit();
      }
    }
  }
}
