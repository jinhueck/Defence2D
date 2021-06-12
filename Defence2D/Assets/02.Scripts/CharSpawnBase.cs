using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public class CharSpawnBase : MonoBehaviour
{
    public CharSpawnBase m_SpawnBaseOtherSide;
    public List<CharactorCtrl> m_listCharCtrl_Use = new List<CharactorCtrl>();
    public List<CharactorCtrl> m_listCharCtrl_UnUse = new List<CharactorCtrl>();
    
    [SerializeField]
    public ConvertStorageStageGenData<StageGenData> convertStorage_StageGen; 
    private string sz_pathCSV_Stage = "StageGenData";

    public ConvertStorageMonsterData<MonsterData> convertStorage_Monster;
    private string sz_pathCSV_MonsterData = "MonsterData";

    public Transform spawnPos;

    protected void Start()
    {
        Setup();
    }

    public virtual void Setup()
    {
        if(convertStorage_StageGen == null)
        {
            convertStorage_StageGen = new ConvertStorageStageGenData<StageGenData>();
        }
        convertStorage_StageGen.ConvertCSVToClass(sz_pathCSV_Stage);

        if(convertStorage_Monster == null)
        {
            convertStorage_Monster = new ConvertStorageMonsterData<MonsterData>();
        }
        convertStorage_Monster.ConvertCSVToClass(sz_pathCSV_MonsterData);
    }
    public virtual void SpawnMoster(int nIndex, bool bFlagMyTeam)
    {

    }
    public void RemoveMonster(CharactorCtrl p_RemoveCharacter)
    {
        if (m_listCharCtrl_Use.Contains(p_RemoveCharacter))
        {
            m_listCharCtrl_Use.Remove(p_RemoveCharacter);
            Destroy(p_RemoveCharacter.gameObject);
        }
    }
}

[Serializable]
public class ConvertStorageBase<T>
{
    public virtual void Setup(string stageName) { }

    public virtual void ConvertCSVToClass(string csvPath) { }

    protected int ConvertToInt(object obj)
    {
        string _body = obj.ToString();
        _body = Regex.Replace(_body, @"[^0-9]", "");
        return int.Parse(_body);
    }

    protected string ConvertToString(object obj)
    {
        return obj.ToString();
    }

    protected float ConvertToFloat(object obj)
    {
        string _body = obj.ToString();
        _body = Regex.Replace(_body, @"[^0-9]", "");
        return float.Parse(_body);
    }
}

public class ConvertStorageStageGenData<T> : ConvertStorageBase<T>
{
    private List<StageGenData> m_listStageGenData = new List<StageGenData>();
    private List<StageGenData> m_listStageGenData_Use = new List<StageGenData>();

    public override void Setup(string stageName)
    {
        if (m_listStageGenData_Use.Count > 0)
        {
            m_listStageGenData_Use.Clear();
        }
        List<StageGenData> p_listStageGenData = m_listStageGenData.Where(n => n.stageName == stageName).ToList();
        m_listStageGenData_Use.AddRange(p_listStageGenData);
    }

    public override void ConvertCSVToClass(string csvPath)
    {
        List<Dictionary<string, object>> data = CSVReader.Read(csvPath);

        for (int i = 0; i < data.Count; i++)
        {
            StageGenData nStageGenData = new StageGenData();
            nStageGenData.index = ConvertToInt(data[i]["index"]);
            nStageGenData.code = ConvertToString(data[i]["code"]);
            nStageGenData.stageName = ConvertToString(data[i]["stageName"]);
            nStageGenData.groupCode = ConvertToString(data[i]["groupCode"]);
            nStageGenData.monsterName = ConvertToString(data[i]["monsterName"]);
            nStageGenData.level = ConvertToInt(data[i]["level"]);
            nStageGenData.amount = ConvertToInt(data[i]["amount"]);
            nStageGenData.coolTime = ConvertToFloat(data[i]["coolTime"]);
            nStageGenData.fixSpawn = ConvertToInt(data[i]["fixSpawn"]);
            nStageGenData.randCoolTime = ConvertToFloat(data[i]["randCoolTime"]);
            nStageGenData.randAppear = ConvertToFloat(data[i]["randAppear"]);
            nStageGenData.itemDrop = ConvertToFloat(data[i]["itemDrop"]);
            nStageGenData.item = ConvertToString(data[i]["item"]);
            nStageGenData.itemAmount = ConvertToInt(data[i]["itemAmount"]);
            m_listStageGenData.Add(nStageGenData);
        }
    }
}

public class ConvertStorageMonsterData<T> : ConvertStorageBase<T>
{
    private List<MonsterData> m_listMonsterData = new List<MonsterData>();

    public override void Setup(string stageName)
    {

    }

    public override void ConvertCSVToClass(string csvPath)
    {
        List<Dictionary<string, object>> data = CSVReader.Read(csvPath);

        for (int i = 0; i < data.Count; i++)
        {
            MonsterData nStageGenData = new MonsterData();
            nStageGenData.index = ConvertToInt(data[i]["index"]);
            nStageGenData.monsterName = ConvertToString(data[i]["monsterName"]);
            nStageGenData.type = ConvertToString(data[i]["type"]);
            nStageGenData.level = ConvertToInt(data[i]["level"]);
            nStageGenData.atk = ConvertToInt(data[i]["atk"]);
            nStageGenData.def = ConvertToInt(data[i]["def"]);
            nStageGenData.health = ConvertToInt(data[i]["health"]);
            nStageGenData.atkRange = ConvertToInt(data[i]["atkRange"]);
            nStageGenData.atkSpeed = ConvertToInt(data[i]["atkSpeed"]);
            nStageGenData.moveSpeed = ConvertToInt(data[i]["moveSpeed"]);
            nStageGenData.spawnCool = ConvertToInt(data[i]["spawnCool"]);
            nStageGenData.price = ConvertToInt(data[i]["price"]);
            nStageGenData.resourceName = ConvertToString(data[i]["resourceName"]);
            nStageGenData.charSize = ConvertToFloat(data[i]["charSize"]);
            nStageGenData.reward = ConvertToString(data[i]["reward"]);
            m_listMonsterData.Add(nStageGenData);
        }
    }
}

public class StageGenData
{
    public int index;
    public string code;
    public string stageName;
    public string groupCode;
    public string monsterName;
    public int level;
    public int amount;
    public float coolTime;
    public int fixSpawn;
    public float randCoolTime;
    public float randAppear;
    public float itemDrop;
    public string item;
    public int itemAmount;
}

public class MonsterData
{
    public int index;
    public string monsterName;
    public string type;
    public int level;
    public int atk;
    public int def;
    public int health;
    public int atkRange;
    public int atkSpeed;
    public int moveSpeed;
    public int spawnCool;
    public int price;
    public string resourceName;
    public float charSize;
    public string reward;
}