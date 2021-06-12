using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSpawnBase_Team : CharSpawnBase
{
    public List<Button> m_listButton = new List<Button>();
    public List<CharactorCtrl> m_listCharacterPrefabList = new List<CharactorCtrl>();

    public void Awake()
    {
        for (int i = 0; i < m_listButton.Count; i++)
        {
            int p_nIndex = i;
            m_listButton[i].onClick.AddListener(() =>
            {
                OnButtonPress(p_nIndex);
            });
        }
    }
    public override void SpawnMoster(int nIndex, bool bFlagMyTeam)
    {
        GameObject p_SpawnMonster = Instantiate(m_listCharacterPrefabList[nIndex].gameObject);
        Vector3 p_VecCharScale = p_SpawnMonster.transform.localScale;
        if(bFlagMyTeam == false)
        {
            p_VecCharScale.x *= -1;
            p_SpawnMonster.transform.localScale = p_VecCharScale;
        }
        p_SpawnMonster.tag = bFlagMyTeam ? "Player" : "Enemy";
        p_SpawnMonster.transform.position = spawnPos.position;

        CharactorCtrl nCharacterCtrl = p_SpawnMonster.GetComponent<CharactorCtrl>();
        if (nCharacterCtrl != null)
        {
            nCharacterCtrl.SetupSpawnBase(this, m_SpawnBaseOtherSide);
            m_listCharCtrl_Use.Add(nCharacterCtrl);
        }
    }

    public void OnButtonPress(int nIndex)
    {
        SpawnMoster(nIndex, true);
    }
}
