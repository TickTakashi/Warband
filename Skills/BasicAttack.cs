using UnityEngine;
using System.Collections;

public class BasicAttack : Skill {

  // TODO: Investigate if there is a better way to set this up.
  public string trigger_name;

  public int power = 3;
  public int range = 1;
  public int cost  = 1;

  public float hit_time = 0.75f;
  public float completion_time = 0.95f;
  public ParticleSystem hit_effect;

  public override bool CanUse(Warrior w, Location l) {
    return Location.Distance(w.location, l) <= range;
  }

  public override IEnumerator Use(Location l) {
    Animator anim = GetComponent<Animator>();
    anim.SetTrigger(trigger_name);
    yield return null;

    while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < hit_time) {
      yield return null;
    }

    ApplySkillToLocation(l);

    while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < completion_time) {
      yield return null;
    }
  }

  public override FollowEntity GetUI(Entity e) {
    FollowEntity rt = base.GetUI(e);
    rt.GetComponent<BasicAttackSkillUI>().attack = this;
    return rt;
  }

  public virtual void ApplySkillToLocation(Location l) {
    foreach (Location loc in GetShape()) {
      Hit(l + loc);
    }
  }

  // TODO: This should be overriden by subclasses to add additional effects
  public virtual void Hit(Location l) {
    Vector3 spawn_point = Grid.board.CalculatePosition(l);
    if (hit_effect != null) {
      // TODO: Consider the case where the effect has a direction.
      Instantiate(hit_effect, spawn_point, Quaternion.identity);
    }

    Tile t = Grid.board.GetTile(l);
    t.occupant.Damage(power);
  }

  // TODO: Override this to deal with more interesting shapes.
  public virtual Location[] GetShape() {
    return new Location[] { new Location(0, 0) };
  }
}
