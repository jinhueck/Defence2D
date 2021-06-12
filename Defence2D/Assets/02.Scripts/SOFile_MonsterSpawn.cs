using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOFile_MonsterSpawn", menuName = "Scriptable Object/SOFile_MonsterSpawn", order = int.MaxValue)]
public class SOFile_MonsterSpawn : ScriptableObject
{
    public CharactorCtrl characterCtrl;
    public float f_SpawnTime;
}
