using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class HealthBarWorld : MonoBehaviour {

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
  public Entity e;

  private Rect initial_dimensions;


  public void Awake() {
    initial_dimensions = bar_mask.rectTransform.rect;
  }

  public void Start() {
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
    float percentage = (float)e.health / (float)e.max_health;
    SetValue(percentage);
  }

  public void SetValue(float percentage) {
    if (percentage > 1 || percentage < 0) {
      throw new UnityException("HealthBar - SetValue - Health overflow!");
    }

    bar_mask.rectTransform.sizeDelta = new Vector2(
      initial_dimensions.width * percentage,
      initial_dimensions.height);

    bar_mask.rectTransform.anchoredPosition = new Vector2(
      -(initial_dimensions.width * (1 - percentage)) / 2f, 0f);
  }
}
