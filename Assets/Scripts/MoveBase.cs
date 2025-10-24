using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Move", menuName = "Pokemon/Create new move")]
public class MoveBase : ScriptableObject
{
    [field: SerializeField] public string name {get; private set; }

    [TextArea]
    [field: SerializeField] public string description {get ; private set; }

    [field: SerializeField] public PokemonType type {get ; private set; }
    [field: SerializeField] public int power {get ; private set; }
    [field: SerializeField] public int accuracy {get ; private set; }
    [field: SerializeField] public int pp {get ; private set; }
}
