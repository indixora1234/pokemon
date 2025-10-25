using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Move", menuName = "Pokemon/Create new move")]
public class MoveBase : ScriptableObject
{
    [field: SerializeField] public string Name {get; private set; }

    [TextArea]
    [field: SerializeField] public string Description {get ; private set; }

    [field: SerializeField] public PokemonType Type {get ; private set; }
    [field: SerializeField] public int Power {get ; private set; }
    [field: SerializeField] public int Accuracy {get ; private set; }
    [field: SerializeField] public int PP {get ; private set; }
}
