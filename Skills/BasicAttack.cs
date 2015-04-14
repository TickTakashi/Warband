using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicAttack : Skill {

  // TODO: Investigate if there is a better way to set this up.
  public Warrior owner;

  public string trigger_name;

  public int power = 3;
  public int min_range = 1;
  public int max_range = 1;
  public int cost = 1;
  public Location[] shape;

  public float hit_time = 0.75f;
  public ParticleSystem hit_effect;

  public override bool CanUse(Warrior w, Location l) {
    return Location.Distance(w.location, l) <= max_range &&
           Location.Distance(w.location, l) >= min_range;
  }

  public override IEnumerator Use(Location l) {
    owner.casted = true;
    yield return StartCoroutine(owner.LookAtTile(l));
    Animator anim = owner.anim; 
    anim.SetTrigger(trigger_name);
    yield return StartCoroutine(owner.WaitForAnimation(hit_time)); 
    ApplySkillToLocation(l);
    yield return StartCoroutine(owner.WaitForAnimation()); 
  }


  public virtual void ApplySkillToLocation(Location l) {
    foreach (Location loc in GetShape()) {
      Hit(l + loc);
    }
  }

  // TODO: This should be overriden by subclasses to add additional effects
  public virtual void Hit(Location l) {
    Vector3 spawn_point = Grid.board.CalculatePosition(l) + Vector3.up;
    Tile t = Grid.board.GetTile(l);

    if (t.occupant != null) {
      t.occupant.Damage(power);
      if (hit_effect != null) {
        // TODO: Consider the case where the effect has a direction.
        Instantiate(hit_effect, spawn_point, Quaternion.identity);
      }
    }
  }

  // TODO: Override this to deal with more shapes based on conditions.
  public virtual Location[] GetShape() {
    return shape;
  }

  public virtual IEnumerable<Location> GetRange() {
    return owner.location.Range(min_range, max_range);
  }
}
