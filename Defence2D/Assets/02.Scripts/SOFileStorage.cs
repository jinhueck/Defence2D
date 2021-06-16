using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOFileStorage : MonoBehaviour
{
    public Dictionary<string, SOFile_CharacterData> map_CharacterData = new Dictionary<string, SOFile_CharacterData>();

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
    }
}
