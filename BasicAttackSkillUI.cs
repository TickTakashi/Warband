using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BasicAttackSkillUI : FollowEntity {

  public BasicAttack attack;

  public ShapeIcon shape_icon;
  public Text skill_name;
  public Text range;
  public Text damage;
  public Text cost;

  public Button btn;

  public Button range_indicator;

  public void Start() {
    this.shape_icon.hits = attack.GetShape();
    this.skill_name.text = attack.skill_name;
    this.range.text = "" + attack.range;
    this.damage.text = "" + attack.power;
    this.cost.text = "" + attack.cost;

  }

  public void DisplayRange() {
    // Instantiate range indicators
      // Bind Attack peramters to range indicators.
  }

  // TODO: Create a bunch of range buttons based on the range of this skill
  // these should FollowEntity (the same as the attack) but only get unhidden
  // if the location in question is a legit target (not a hole, must be a tile.
  
  public override void SetVisibility(bool is_on) {
    base.SetVisibility(is_on);
  }
}
