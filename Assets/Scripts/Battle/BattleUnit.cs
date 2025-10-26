using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase _base ;
    [SerializeField] int level ; 
    [SerializeField] bool isPlayerUnit ; 
    [field: SerializeField] public BattleHud Hud {get; private set;}

    public Pokemon Pokemon {get; set;}
    
    public void Setup(){
        Pokemon = new Pokemon(_base, level);

        if (isPlayerUnit)
            GetComponent<Image>().sprite = Pokemon.Base.BackSprite;
        
        else
            GetComponent<Image>().sprite = Pokemon.Base.FrontSprite;

        Hud.SetData(Pokemon);
        
    }
}
