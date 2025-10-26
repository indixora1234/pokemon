using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class BattleHud : MonoBehaviour
{
    [field: SerializeField] public Text NameText {get; private set; }
    [field: SerializeField] public Text LevelText {get; private set; }
    [field: SerializeField] public HPBar HpBar {get; private set; }

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon){
        _pokemon = pokemon;
        NameText.text = pokemon.Base.Name;
        LevelText.text = "Lvl" + pokemon.Level;
        HpBar.SetHP((float) pokemon.HP / pokemon.MaxHp);
    }

    public void UpdateHP(){
        HpBar.SetHP((float) _pokemon.HP / _pokemon.MaxHp);

    }

}
