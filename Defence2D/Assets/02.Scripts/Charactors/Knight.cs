using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : CharactorCtrl
{
    // Start is called before the first frame update
    protected override void Start()
    {
        SetStat();
        base.Start();
    }

    void SetStat()
    {
        MoveSpeed = 2.5f;
        Hp = 100f;
        nowHp = Hp;
        AttackPower = 20f;
        AttackDelay = 2f;
        Protection = 1f;
        AttackRange = 4f;
        //SetAttackRange(4f);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
