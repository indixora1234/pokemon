using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit
{
    [field: SerializeField] public Pokemon _base {get; private set; }
    [field: SerializeField] public int Level {get; private set; }
    [field: SerializeField] public bool isPlayerUnit {get; private set; }

    public Pokemon pokemon {get; set;}
    
    public void SetUp(){
        new Pokemon(_base, level);
        if (isPlayerUnit){
            GetComponent<Image>().sprite = pokemon.Base.BackSprite;
        }
        else{
            GetComponent<Image>().sprite = pokemon.Base.FrontSprite;

        }
    }
}
