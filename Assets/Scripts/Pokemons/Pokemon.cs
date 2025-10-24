using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pokemon
{
    PokemonBase _base;
    int level;

    public int HP { get; set; }

    public List<Move> Moves {get; set; }

    public Pokemon(PokemonBase pBase, int pLevel){
        _base = pBase;
        level = pLevel;
        HP = _base.maxHp;

        //Generate Moves
        Moves = new List<Move>();
        foreach(var move in _base.learnableMoves){
            if (move.level <= level){
                Moves.add(new Move(move.Base));
            }

            if (Moves.Count >= 4){
                break;
            }
        }
    }

    public int Attack {
        get { return Mathf.FloorToInt((_base.attack * level)/100f) + 5;}
    }

    public int Defense {
        get { return Mathf.FloorToInt((_base.defense * level)/100f) + 5;}
    }

    public int SpAttack {
        get { return Mathf.FloorToInt((_base.spAttack * level)/100f) + 5;}
    }

    public int Speed {
        get { return Mathf.FloorToInt((_base.speed * level)/100f) + 5;}
    }

    public int MaxHp {
        get { return Mathf.FloorToInt((_base.maxHp * level)/100f) + 5;}
    }

    public int SpDefense {
        get { return Mathf.FloorToInt((_base.spDefense * level)/100f) + 5;}
    }
}
