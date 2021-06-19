using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOFileStorage : MonoBehaviour
{
    public Dictionary<string, SOFile_CharacterData> map_CharacterData = new Dictionary<string, SOFile_CharacterData>();

    public Dictionary<string, SOFile_SkillData> map_SkillData = new Dictionary<string, SOFile_SkillData>();

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        var p_listCharacterData = Resources.LoadAll<SOFile_CharacterData>("SOFile/CharacterData");
        lock(p_listCharacterData)
        {
            for (int i = 0; i < p_listCharacterData.Length; i++)
            {
                SOFile_CharacterData p_characterData = p_listCharacterData[i];
                map_CharacterData.Add(p_characterData.szCode, p_characterData);
            }
        }

        var p_listSkillData = Resources.LoadAll<SOFile_SkillData>("SOFile/SkillData");
        lock (p_listSkillData)
        {
            for (int i = 0; i < p_listSkillData.Length; i++)
            {
                SOFile_SkillData p_skillData = p_listSkillData[i];
                map_SkillData.Add(p_skillData.szCode, p_skillData);
            }
        }
    }
}
