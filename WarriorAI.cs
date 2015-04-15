using UnityEngine;
using System.Collections;

public abstract class WarriorAI : MonoBehaviour {
  protected Warrior w;

  void Start() {
    w = GetComponent<Warrior>();
  }
	
  public abstract IEnumerator Act(); 
}
