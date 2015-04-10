using UnityEngine;
using System.Collections;

public abstract class Skill : MonoBehaviour {
  public string skill_name;
  public Transform UIButton;

  private FollowEntity my_button;

  public abstract bool CanUse(Warrior w, Location l);
  public abstract IEnumerator Use(Location l);

  public virtual FollowEntity GetUI(Entity e) {
    if (my_button == null) {
      my_button = (Instantiate(UIButton) as Transform).GetComponent<FollowEntity>();
      my_button.rtrans.SetParent(Grid.canvas.transform, false);
      my_button.SetEntity(e);
    }

    return my_button;
  }
}
