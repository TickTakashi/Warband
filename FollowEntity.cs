using UnityEngine;
using System.Collections;
using System;

public class FollowEntity : FollowUI<Entity> {
  public virtual void SetEntity(Entity entity) {
    entity.PositionChangeEvent += new EventHandler<PositionChangeEventArgs>(OnPositionChange);
    SetTracked(entity);
    this.e = entity;
  }

  private void OnPositionChange(object sender, PositionChangeEventArgs ea) {
    entity_moving = ea.moving;
    MaintainPosition();
  }
}