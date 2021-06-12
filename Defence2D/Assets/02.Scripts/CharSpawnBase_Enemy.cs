using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSpawnBase_Enemy : CharSpawnBase
{
    public List<SOFile_MonsterSpawn> m_listSOFile_MonsterSpawn = new List<SOFile_MonsterSpawn>();
    public List<float> m_listPresentTimeCheck = new List<float>();

    public override void Setup()
    {
        for (int i = 0; i < m_listSOFile_MonsterSpawn.Count; i++)
        {
            m_listPresentTimeCheck.Add(0f);
        }
        //base.Setup();
    }
    public override void SpawnMoster(int nIndex, bool bFlagMyTeam)
    {
        SOFile_MonsterSpawn p_SOFile_MonsterSpawn = m_listSOFile_MonsterSpawn[nIndex];

        GameObject p_SpawnMonster = Instantiate(p_SOFile_MonsterSpawn.characterCtrl.gameObject);
        Vector3 p_VecCharScale = p_SpawnMonster.transform.localScale;
        p_VecCharScale.x *= -1;
        p_SpawnMonster.transform.localScale = p_VecCharScale;
        p_SpawnMonster.tag = bFlagMyTeam ? "Player" : "Enemy";
        p_SpawnMonster.transform.position = spawnPos.position;

        CharactorCtrl nCharacterCtrl = p_SpawnMonster.GetComponent<CharactorCtrl>();
        if(nCharacterCtrl != null)
        {
            nCharacterCtrl.SetupSpawnBase(this, m_SpawnBaseOtherSide);
            m_listCharCtrl_Use.Add(nCharacterCtrl);
        }
    }

    private void Update()
    {
        for (int i = 0; i < m_listSOFile_MonsterSpawn.Count; i++)
        {
            SOFile_MonsterSpawn p_SOFile_MonsterSpawn = m_listSOFile_MonsterSpawn[i];
            if (p_SOFile_MonsterSpawn.f_SpawnTime <= m_listPresentTimeCheck[i])
            {
                m_listPresentTimeCheck[i] = 0f;
                SpawnMoster(i, false);
                continue;
            }
            else
            {
                m_listPresentTimeCheck[i] += Time.deltaTime;
            }
        }
    }

}
