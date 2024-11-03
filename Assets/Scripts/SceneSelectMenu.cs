using NRKernal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectMenu : MonoBehaviour
{
  public void LoadCallCpp() { Load("CallCpp"); }
  public void LoadCallDrawLib() { Load("CallDrawLib"); }
  public void LoadGrabMoveObject() { Load("GrabMoveObject"); }
  public void LoadSelectSlideMoveObject() { Load("SelectSlideMoveObject"); }

  [SerializeField] GameObject canvas;
  void Load(string s)
  {
    canvas.SetActive(false);
    SceneManager.LoadScene(s);
  }

  void Start()
  {
    GameObject.DontDestroyOnLoad(this.gameObject);
  }
  void Update()
  {
    if (NRInput.GetButtonDown(ControllerButton.APP))
    {
      canvas.SetActive(!canvas.activeInHierarchy);
    }
  }
}
