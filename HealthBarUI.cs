using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UEL;
using System;

public class HealthBarUI : UELBehaviour {

  public static Color[] player_colors = new Color[] {
    Color.green,
    Color.red,
    Color.cyan,
    Color.yellow,
    Color.blue
  };

  public Image bar_mask;
  public Image underlay;
  public Image bar;

  private Entity e;
  private Rect initial_dimensions;
  private float offset = 0.75f;

  public void Awake() {
    initial_dimensions = bar_mask.rectTransform.rect;
  }

  public void SetEntity(Entity e) {
    this.e = e;
    e.HealthChangeEvent += new EventHandler<HealthChangeEventArgs>(OnHealthChange);
    e.PositionChangeEvent += new EventHandler<PositionChangeEventArgs>(OnPositionChange);
    bar.color = player_colors[e.owner];
    UpdateBar();
  }

  private void OnHealthChange(object sender, HealthChangeEventArgs e) {
    UpdateBar();
  }

  public void OnPositionChange(object sender, PositionChangeEventArgs ea) {
    UpdateBar();
  }

  public void UpdateBar() {
    Vector3 position = e.transform.position + new Vector3(
      Grid.board.tile_size * offset / 2f,
      0,
      Grid.board.tile_size * offset / 2f);

    Vector2 viewpoint_point = Camera.main.WorldToViewportPoint(position);


    float percentage = (float)e.health / (float)e.max_health;
    SetValue(percentage);

    RectTransform rt = GetComponent<RectTransform>();
    rt.anchorMin = viewpoint_point;
    rt.anchorMax = viewpoint_point; 
  }

  public void SetValue(float percentage) {
    if (percentage > 1 || percentage < 0) {
      throw new UnityException("HealthBar - SetValue - Health overflow!");
    }

    bar_mask.rectTransform.sizeDelta = new Vector2(
      initial_dimensions.width * percentage,
      initial_dimensions.width);

    bar_mask.rectTransform.anchoredPosition = new Vector2(
      initial_dimensions.width * percentage / 2f, 0f);
  }
}
