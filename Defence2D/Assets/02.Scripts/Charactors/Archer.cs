using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : CharactorCtrl
{
    // Start is called before the first frame update
    protected override void Start()
    {
        SetStat();
        base.Start();
    }

    void SetStat()
    {
        MoveSpeed = 1.0f;
        Hp = 100f;
        nowHp = Hp;
        AttackPower = 10f;
        AttackDelay = 4f;
        Protection = 1f;
        AttackRange = 9f;
        //SetAttackRange(9f);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
