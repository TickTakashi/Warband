using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UEL;
using System;

public class Warrior : Entity {
  public static float EPSILLON = 0.001f;
  public Animator anim;

  public string anim_bool_move = "Running";
  public string anim_trigger_die = "Die";
  public string anim_trigger_hurt = "HurtForward";

  public int speed;

  public bool active { get { return ready && !(moved && casted); } }

  public bool ready = false;
  public bool moved = false;
  public bool casted = false;

  private float rot_speed_scale = 100f;

  public void Start() {
    StartCoroutine(Spawn());
  }

  public override void OnTurnBegin(object o, BeginTurnEventArgs e) {
    if (e.player == owner) {
      moved = false;
      casted = false;
    }
  }

  public IEnumerator Spawn() {
    yield return StartCoroutine(WaitForAnimation());
    ready = true;
  }

  public int GetSpeed() {
    return active ? speed : 0;
  }

  public void FollowPath(Path p) {
    if (!moved && active) {
      moved = true;
      List<Location> steps = p.path;
      Grid.board.MoveEntity(this, p.End());
      StartCoroutine(FollowPathRoutine(steps));
    } else {
      Debug.Log("This unit has already acted!");
    }
  }

  public override void Die() {
    ready = false;
    Grid.board.RemoveEntity(this);
    StartCoroutine(AnimateDeath());
  }

  public IEnumerator AnimateDeath() {
    anim.SetTrigger(anim_trigger_die);
    yield return StartCoroutine(WaitForAnimation());
    gameObject.SetActive(false);
  }

  public IEnumerator FollowPathRoutine(List<Location> steps) {
    ready = false;
    Transform trans = transform;
    NotifyPositionChange(true);
    anim.SetBool(anim_bool_move, true);
    for (int i = 0; i < steps.Count; i++) {
      Vector3 next_pos = Grid.board.CalculatePosition(steps[i]);
      Vector3 cur_pos = trans.position;
      float timer = 0;
      Quaternion target = trans.LookAtRotation(next_pos);
      float rot_speed = speed * rot_speed_scale;

      while (Vector3.Distance(trans.position, next_pos) > EPSILLON) {
        trans.position = Vector3.Lerp(cur_pos, next_pos, timer * speed * 0.5f);
        timer += Time.deltaTime;
        trans.rotation = Quaternion.RotateTowards(trans.rotation, target,
          Time.deltaTime * rot_speed);
        //NotifyPositionChange(true);
        yield return null;
      }
    }
    anim.SetBool(anim_bool_move, false);
    ready = true;
    NotifyPositionChange(false);
  }


  public IEnumerator LookAtTile(Location loc) {
    NotifyPositionChange(true);
    Transform trans = transform;
    Vector3 target = Grid.board.CalculatePosition(loc);
    Quaternion target_rotation = trans.LookAtRotation(target);
    float rot_speed = speed * rot_speed_scale;
    anim.SetBool(anim_bool_move, true);
    while (Quaternion.Angle(trans.rotation, target_rotation) > EPSILLON) {
      trans.rotation = Quaternion.RotateTowards(trans.rotation, target_rotation,
        Time.deltaTime * rot_speed);
      //NotifyPositionChange(true);
      yield return null;
    }
    anim.SetBool(anim_bool_move, false);
    
    yield return StartCoroutine(WaitForTransition());

    NotifyPositionChange(false);
  }

  public IEnumerator WaitForTransition() {
    yield return null;

    while (anim.IsInTransition(0))
      yield return null;
  }

  public IEnumerator WaitForAnimation(float percentage = 0.9f) {
    yield return StartCoroutine(WaitForTransition());

    while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < percentage)
      yield return null;
  }

  internal void DisplayUI(bool p) {
    ui.Visible(active && p);
  }

  public void NotifyPositionChange(bool moving) {
    PositionChangeEventArgs pcea = new PositionChangeEventArgs();
    pcea.moving = moving;
    PositionChange(pcea);
  }
}
