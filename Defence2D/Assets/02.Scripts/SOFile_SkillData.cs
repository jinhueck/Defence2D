using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOFile_CharacterSkill", menuName = "Scriptable Object/SOFile_CharacterSkill", order = int.MaxValue)]
public class SOFile_SkillData : ScriptableObject
{
    public int nIndex;
    public string szCode;
    public int atkTarget;
    public int atkType;
    public int atkRange;
    public int projectileType;
    public string projectile;
    public bool penetration;
    public int deleteRange;
    public int projectileTime;
    public int skillEffType;
    public int skillEff;
    public int add;
    public int multi;
    public int function;
    public int functionCode;
}
