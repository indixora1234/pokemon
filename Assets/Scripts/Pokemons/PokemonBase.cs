using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]

public class PokemonBase : ScriptableObject
{
    [field: SerializeField] public string name {get; private set; }

    [TextArea]
    [field: SerializeField] public string description {get ; private set; }

    [field: SerializeField] public Sprite frontSprite {get ; private set; }
    [field: SerializeField] public Sprite backSprite {get ; private set; }

    [field: SerializeField] public PokemonType type1 {get ; private set; }
    [field: SerializeField] public PokemonType type2 {get ; private set; }

    //Base Stats
    [field: SerializeField] public int maxHp {get ; private set; }
    [field: SerializeField] public int attack {get ; private set; }
    [field: SerializeField] public int defense {get ; private set; }
    [field: SerializeField] public int spAttack {get ; private set; }
    [field: SerializeField] public int spDefense {get ; private set; }
    [field: SerializeField] public int speed {get ; private set; }

    [field: SerializeField] public List<LearnableMove> learnableMoves {get ; private set; }
}
[System.Serializable]
public class LearnableMove
{
    [field: SerializeField] public MoveBase moveBase {get ; private set; }
    [field: SerializeField] public int level {get ; private set; }
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