using UnityEngine;
using System.Collections;

public abstract class Skill : MonoBehaviour {
  public string skill_name;
  public Transform UIButton;
  public abstract bool CanUse(Warrior w, Location l);
  public abstract IEnumerator Use(Location l);
}
