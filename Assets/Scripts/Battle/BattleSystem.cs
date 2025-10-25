using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [field: SerializeField] public BattleUnit playerUnit {get; private set; }
    [field: SerializeField] public BattleUnit enemyUnit {get; private set; }

    [field: SerializeField] public BattleHud playerHud {get; private set; }
    [field: SerializeField] public BattleHud enemyHud {get; private set; }


    private void Start(){
        SetUpBattle();
    }

    public void SetUpBattle(){
        playerUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon);
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);
    }
}
