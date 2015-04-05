using UnityEngine;
using System.Collections;
using UEL;

public class PortraitCamera : MonoBehaviour, IObserver {

  //public Shader unlitShader;

  void Start() {
    //GetComponent<Camera>().SetReplacementShader(unlitShader, "");
    Grid.selector.Attach(this);
  }

  public void Notify() {
    Warrior selected = Grid.selector.selected;
    transform.SetParent(selected.portrait_cam);
    transform.localPosition = Vector3.zero;
    transform.localRotation = Quaternion.identity;
  }
}