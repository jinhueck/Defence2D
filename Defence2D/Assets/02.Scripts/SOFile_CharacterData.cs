using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOFile_CharacterStat", menuName = "Scriptable Object/SOFile_CharacterStat", order = int.MaxValue)]
public class SOFile_CharacterData : ScriptableObject
{
    public int nIndex;
    public string szCode;
    public int nUnitType;
    public int nUnitGrade;
    public int nJob;
    public float fDamage;
    public float fGrowth_Damage;
    public float fDefense;
    public float fGrowth_Defense;
    public float fHealth;
    public float fGrowth_Health;
    public float fAttackRange;
    public float fMoveSpeed;
    public float fSpawnCoolTime;
    public int fPrice;
    public string szResourceName;
    public float fCharacterScale;
    public float fSkillCoolTime;
    public string szSkillCode;
    public int nGold;
}
