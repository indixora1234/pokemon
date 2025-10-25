using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class BattleHud
{
    [field: SerializeField] public Text nameText {get; private set; }
    [field: SerializeField] public Text levelText {get; private set; }
    [field: SerializeField] public HPBar hpBar {get; private set; }

    public void SetData(Pokemon pokemon){
        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl" + pokemon.Level;
        hpBar.SetHP((float) pokemon.HP / pokemon.MaxHp);
    }

}
