using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]

public class PokemonBase : ScriptableObject
{
    [field: SerializeField] public string Name {get; private set; }

    [TextArea]
    [field: SerializeField] public string Description {get ; private set; }

    [field: SerializeField] public Sprite FrontSprite {get ; private set; }
    [field: SerializeField] public Sprite BackSprite {get ; private set; }

    [field: SerializeField] public PokemonType Type1 {get ; private set; }
    [field: SerializeField] public PokemonType Type2 {get ; private set; }

    //Base Stats
    [field: SerializeField] public int MaxHp {get ; private set; }
    [field: SerializeField] public int Attack {get ; private set; }
    [field: SerializeField] public int Defense {get ; private set; }
    [field: SerializeField] public int SpAttack {get ; private set; }
    [field: SerializeField] public int SpDefense {get ; private set; }
    [field: SerializeField] public int Speed {get ; private set; }

    [field: SerializeField] public List<LearnableMove> LearnableMoves {get ; private set; }
}
[System.Serializable]
public class LearnableMove
{
    [field: SerializeField] public MoveBase Base {get ; private set; }
    [field: SerializeField] public int Level {get ; private set; }
}

public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
    Griffin,
}