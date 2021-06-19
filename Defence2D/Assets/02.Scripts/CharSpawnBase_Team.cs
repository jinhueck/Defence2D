using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSpawnBase_Team : CharSpawnBase
{
    public List<Button> m_listButton = new List<Button>();
    public List<CharactorCtrl> m_listCharacterPrefabList = new List<CharactorCtrl>();

    InGameMgr gameMgr;

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

        gameMgr = GameObject.Find("InGameMgr").GetComponent<InGameMgr>();
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
            nCharacterCtrl.SetStat(m_listCharacterPrefabList[nIndex].gameObject.name);
            m_listCharCtrl_Use.Add(nCharacterCtrl);
        }
        StartCoroutine(CheckSpawnCoolTime(nIndex, nCharacterCtrl.spawnCoolTime));
    }

    IEnumerator CheckSpawnCoolTime(int _idx, float _coolTime)
    {
        m_listButton[_idx].interactable = false;

        yield return new WaitForSeconds(_coolTime);

        m_listButton[_idx].interactable = true;
    }

    public void OnButtonPress(int nIndex)
    {
        CharactorCtrl charCtrl = m_listCharacterPrefabList[nIndex];
        charCtrl.SetStat(m_listCharacterPrefabList[nIndex].gameObject.name);

        if (gameMgr.Gold >= charCtrl.price)
        {
            gameMgr.Gold -= charCtrl.price;
            SpawnMoster(nIndex, true);
        }
    }
}
