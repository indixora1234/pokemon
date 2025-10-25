using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HPBar : MonoBehaviour
{
    [field: SerializeField] public GameObject health {get; private set; }

    private void Start(){
        health.transform.localScale = new Vector3(0.5f, 1f);
    }

    public void SetHP(float hpNormalized){
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }
}
