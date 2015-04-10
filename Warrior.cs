using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UEL;
using System;

public class Warrior : Entity {
  public static float EPSILLON = 0.001f;
  public int speed;
  public bool active = true;
  public Skill[] skills; 

  public override void OnTurnBegin(object o, BeginTurnEventArgs e) {
    if (e.player == owner) {
      this.active = true;
    }
  }

  public int GetSpeed() {
    return active ? speed : 0;
  }

  public void FollowPath(Path p) {
    if (active) {
      active = false;
      List<Location> steps = p.path;
      Grid.board.MoveEntity(this, p.End());
      StartCoroutine(FollowPathRoutine(steps));
    } else {
      Debug.Log("This unit has already acted!");
    }
  }

  public IEnumerator FollowPathRoutine(List<Location> steps) {
    Transform trans = transform;
    PositionChangeEventArgs pcea = new PositionChangeEventArgs();
    pcea.moving = true;
    PositionChange(pcea); 
    Animator anim = GetComponent<Animator>();
    anim.SetBool("Running", true);
    for (int i = 0; i < steps.Count; i++) {
      Vector3 next_pos = Grid.board.CalculatePosition(steps[i]);
      Vector3 cur_pos = trans.position;
      float timer = 0;
      Quaternion target = trans.LookAtRotation(next_pos);
      float rot_speed = speed * 100f;

      while (Vector3.Distance(trans.position, next_pos) > EPSILLON) {
        trans.position = Vector3.Lerp(cur_pos, next_pos, timer * speed * 0.5f);
        timer += Time.deltaTime;
        trans.rotation = Quaternion.RotateTowards(trans.rotation, target,
          Time.deltaTime * rot_speed);
        yield return null;
      }
    }
    anim.SetBool("Running", false);
    pcea = new PositionChangeEventArgs();
    pcea.moving = false;
    PositionChange(pcea);
  }

}
