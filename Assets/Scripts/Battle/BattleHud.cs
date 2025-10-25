using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class BattleHud : MonoBehaviour
{
    [field: SerializeField] public Text NameText {get; private set; }
    [field: SerializeField] public Text LevelText {get; private set; }
    [field: SerializeField] public HPBar HpBar {get; private set; }

    public void SetData(Pokemon pokemon){
        NameText.text = pokemon.Base.Name;
        LevelText.text = "Lvl" + pokemon.Level;
        HpBar.SetHP((float) pokemon.HP / pokemon.MaxHp);
    }

}
