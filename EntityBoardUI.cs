using UnityEngine;
using System.Collections;
using UEL;
using System.Collections.Generic;
using System;

public class EntityBoardUI : MonoBehaviour {

  private Dictionary<Entity, List<FollowEntity>> togglable_entity_ui;
  private Dictionary<Entity, List<FollowEntity>> perm_entity_ui;
  public Transform health_bar_prefab;
  public Transform move_button_prefab;

  public float global_offset = 100f;

  public void Start() {
    togglable_entity_ui = new Dictionary<Entity, List<FollowEntity>>();
    perm_entity_ui = new Dictionary<Entity, List<FollowEntity>>();
    foreach (Warrior e in Grid.board.warriors.Keys) {
      togglable_entity_ui[e] = new List<FollowEntity>();
      perm_entity_ui[e] = new List<FollowEntity>();
      InitWarriorUI(e);
    }
  }

  public void InitWarriorUI(Warrior e) {
    // TODO: This is similar to the skill stuff. potential refactor.
    Transform hbt = Instantiate(health_bar_prefab) as Transform;
    hbt.SetParent(transform, false);
    HealthBarUI hb = hbt.GetComponent<HealthBarUI>();
    hb.SetEntity(e);
    perm_entity_ui[e].Add(hb);
    hb.SetVisibility(true);

    Transform mb = Instantiate(move_button_prefab) as Transform;
    mb.SetParent(transform, false);
    FollowEntity mb_fe = mb.GetComponent<FollowEntity>();
    mb_fe.SetEntity(e);
    togglable_entity_ui[e].Add(mb_fe);
    mb_fe.SetVisibility(false);
    mb_fe.screen_space_offset = new Vector2(0, global_offset);
    // TODO: Add Movement Button (It's Togglable UI)
    // TODO: Convert Movement into a skill - forces correct skill coroutine architecture (Queueing actions and waiting for them to finish before ending turn etc).
    // TODO: Add Spacing between Skills
    Skill[] skills = e.skills;
    float spacing = 90f;
    for (int i = 0; i < skills.Length; i++) {
      FollowEntity skill_button = skills[i].GetUI(e);
      togglable_entity_ui[e].Add(skill_button);
      skill_button.SetVisibility(false);
      skill_button.screen_space_offset = new Vector2(0, global_offset + (i + 1) * spacing);
    } 
  }

  public void DisplayUIFor(Entity e, bool visibility) {
    List<FollowEntity> ui = togglable_entity_ui[e];
    foreach (FollowEntity fe in ui) {
      fe.SetVisibility(visibility);
    }
  }
}
