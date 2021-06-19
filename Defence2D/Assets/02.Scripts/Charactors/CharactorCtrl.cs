using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorCtrl : MonoBehaviour
{
    public enum State
    {
        Move,
        Attack,
        Hit,
        Die
    };
    private State state = State.Move;

    public int key;

    //캐릭터 정보
    private int c_index;
    private string c_szCode;
    private int unitType;
    private int unitGrade;
    private int jobType;
    private float damage;
    private float growth_Damage;
    private float defense;
    private float growth_Defense;
    private float health;
    private float growth_Health;
    private float attackRange;
    private float moveSpeed;
    public float spawnCoolTime;
    public int price;
    private string szResourceName;
    private float characterScale;
    private float skillCoolTime;
    private string szSkillCode;
    private int gold;

    //스킬정보
    private int sIndex;
    private string szCode;
    private int atkTarget;
    private int atkType;
    private int atkRange;
    private int projectileType;
    private string projectile;
    private bool penetration;
    private int deleteRange;
    private int projectileTime;
    private int skillEffType;
    private int skillEff;
    private int add;
    private int multi;
    private int function;
    private int functionCode;

    float atkCoolTime;
    float nowHealth;
    public Image HpImg;
    private bool isDie = false;
    float DeadAnimTime;

    public GameObject projectileObj;

    public GameObject attackTarget = null;
    public List<GameObject> AttackedList;

    Animator animator;

    InGameMgr gameMgr;

    public void SetStat(string _key)
    {
        SOFileStorage soStorage = GameObject.Find("SOFileStorage").GetComponent<SOFileStorage>();

        SOFile_CharacterData cData;
        soStorage.map_CharacterData.TryGetValue(_key, out cData);

        //기본 정보
        c_index = cData.nIndex;
        szCode = cData.szCode;
        unitType = cData.nUnitType;
        unitGrade = cData.nUnitGrade;
        jobType = cData.nJob;
        damage = cData.fDamage;
        growth_Damage = cData.fGrowth_Damage;
        defense = cData.fDefense;
        growth_Defense = cData.fGrowth_Defense;
        health = cData.fHealth;
        growth_Health = cData.fGrowth_Health;
        attackRange = cData.fAttackRange;
        moveSpeed = cData.fMoveSpeed;
        spawnCoolTime = cData.fSpawnCoolTime;
        price = cData.fPrice;
        szResourceName = cData.szResourceName;
        characterScale = cData.fCharacterScale;
        skillCoolTime = cData.fSkillCoolTime;
        szSkillCode = cData.szSkillCode;
        gold = cData.nGold;

        SOFile_SkillData sData;
        soStorage.map_SkillData.TryGetValue(szSkillCode, out sData);
        sIndex = sData.nIndex;
        szCode = sData.szCode;
        atkTarget = sData.atkTarget;
        atkType = sData.atkType;
        atkRange = sData.atkRange;
        projectileType = sData.projectileType;
        projectile = sData.projectile;
        penetration = sData.penetration;
        deleteRange = sData.deleteRange;
        projectileTime = sData.projectileTime;
        skillEffType = sData.skillEffType;
        skillEff = sData.skillEff;
        add = sData.add;
        multi = sData.multi;
        function = sData.function;
        functionCode = sData.functionCode;
    }

    private void Start()
    {
        print(c_index);
        //기본 셋팅
        damage += growth_Damage * (unitGrade - 1);
        defense += growth_Defense * (unitGrade - 1);
        health += growth_Health * (unitGrade - 1);

        atkCoolTime = skillCoolTime;
        DeadAnimTime = 3f;
        nowHealth = health;
        //HpImg.fillAmount = nowHealth / health;

        this.gameObject.transform.localScale = new Vector2(characterScale, characterScale);

        AttackedList = new List<GameObject>();
        animator = this.GetComponent<Animator>();

        projectileObj = Resources.Load("Arrow") as GameObject;

        gameMgr = GameObject.Find("InGameMgr").GetComponent<InGameMgr>();

        ChangeState(State.Move);
    }

    public void ChangeState(State newState)
    {
        StopCoroutine(state.ToString());
        state = newState;
        StartCoroutine(state.ToString());
    }

    private IEnumerator Move()
    {
        animator.SetBool("isAttack", false);

        while (true)
        {
            //캐릭터 이동
            if (this.transform.tag == "Player")
                this.transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0f, 0f));
            else if (this.transform.tag == "Enemy")
                this.transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f));

            //Search Target
            float closestDist = Mathf.Infinity;
            if(m_SpawnBase_Enemy != null)
            {
                for (int i = 0; i < m_SpawnBase_Enemy.m_listCharCtrl_Use.Count; i++)
                {
                    float distance = Mathf.Abs(m_SpawnBase_Enemy.m_listCharCtrl_Use[i].transform.position.x - this.transform.position.x);
                    if (distance <= attackRange && distance < closestDist)
                    {
                        closestDist = distance;
                        attackTarget = m_SpawnBase_Enemy.m_listCharCtrl_Use[i].gameObject;
                    }
                }

                if (attackTarget != null)
                    ChangeState(State.Attack);
            }
            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        atkCoolTime = skillCoolTime;

        animator.SetBool("isAttack", true);

        CharactorCtrl attackedCharac = attackTarget.GetComponent<CharactorCtrl>();

        if (!attackedCharac.AttackedList.Contains(this.gameObject))
            attackedCharac.AttackedList.Add(this.gameObject);

        while (true)
        {
            //다른 캐릭터에 의해 제거
            if (attackTarget == null)
            {
                ChangeState(State.Move);
                break;
            }

            //공격범위 벗어남
            float distance = Mathf.Abs(attackTarget.transform.position.x - this.transform.position.x);
            if (distance > attackRange)
            {
                attackTarget = null;
                ChangeState(State.Move);
                break;
            }

            ////공격
            //if(attackTarget != null)
            //    attackTarget.GetComponent<CharactorCtrl>().Hit(atk);

            ////공격 쿨타임
            //yield return new WaitForSeconds(atkSpeed);

            if (attackTarget != null)
            {
                atkCoolTime -= Time.deltaTime;
                if (atkCoolTime <= 0f)
                {
                    AttackAct();

                    atkCoolTime = skillCoolTime;
                }

                //애니메이션 속도 조절(추후 레벨당 공속 증가 있을 시)
                //https://wergia.tistory.com/41

                //공격범위 벗어났을 때 하고있던 공격 마무리? or 캔슬
                //마무리 시 해당 함수로 "공격범위 벗어남" 이동
            }

            yield return null;
        }
    }

    void AttackAct()
    {
        //Attack_Normal
        //attackTarget.GetComponent<CharactorCtrl>().Hit(damage);

        switch (atkType)
        {
            case 1:
                Attack_Normal();
                break;

            case 2:
                Attack_Range();
                break;

            case 3:
                Attack_Shot();
                break;
        }
    }

    void Attack_Normal()
    {
        attackTarget.GetComponent<CharactorCtrl>().Hit(damage);
    }

    void Attack_Range()
    {
        attackTarget.GetComponent<CharactorCtrl>().Hit(damage);

        float attackDist = Mathf.Infinity;
        for (int i = 0; i < m_SpawnBase_Enemy.m_listCharCtrl_Use.Count; i++)
        {
            attackDist = Mathf.Abs(attackTarget.transform.position.x - m_SpawnBase_Enemy.m_listCharCtrl_Use[i].transform.position.x);
            if (attackDist <= atkRange)
            {
                m_SpawnBase_Enemy.m_listCharCtrl_Use[i].GetComponent<CharactorCtrl>().Hit(damage);
            }
        }
    }

    void Attack_Shot()
    {
        GameObject p_projectileObj = Instantiate(projectileObj, this.transform);
        p_projectileObj.GetComponent<ProjectileCtrl>().Setup(projectileType, attackTarget, damage, this.transform.tag);

        switch (projectileType)
        {
            case 1:
                break;

            case 2:
                break;
        }
    }

    public void Hit(float _damage)
    {
        if (isDie)
            return;

        nowHealth -= _damage;
        //HpImg.fillAmount = nowHealth / health;

        //애니메이션
        StopCoroutine("HitAnim");
        StartCoroutine("HitAnim");

        if (nowHealth <= 0f)
        {
            isDie = true;
            //Die();
            ChangeState(State.Die);
        }
    }

    private IEnumerator HitAnim()
    {
        Color color = this.GetComponent<SpriteRenderer>().color;

        color.a = 0.4f;
        this.GetComponent<SpriteRenderer>().color = color;

        yield return new WaitForSeconds(0.07f);

        color.a = 1f;
        this.GetComponent<SpriteRenderer>().color = color;
    }


    //public void Die()
    //{
    //    for (int i = 0; i < AttackedList.Count; i++)
    //    {
    //        CharactorCtrl attackedCharac = AttackedList[i].GetComponent<CharactorCtrl>();

    //        attackedCharac.attackTarget = null;
    //    }

    //    m_SpawnBase_Team.RemoveMonster(this);
    //    animator.SetTrigger("isDie");
    //    Destroy(this.gameObject, DeadAnimTime);
    //}

    public IEnumerator Die()
    {
        //1
        //attackTarget.GetComponent<CharactorCtrl>().AttackedList.Remove(this.gameObject);

        for (int i = 0; i < AttackedList.Count; i++)
        {
            CharactorCtrl attackedCharac = AttackedList[i].GetComponent<CharactorCtrl>();

            attackedCharac.attackTarget = null;

            //2
            if (attackedCharac.AttackedList.Contains(this.gameObject))
                attackedCharac.AttackedList.Remove(this.gameObject);
        }

        m_SpawnBase_Team.RemoveMonster(this);
        animator.SetTrigger("isDie");

        yield return new WaitForSeconds(DeadAnimTime);

        if (this.gameObject.tag == "Enemy")
            gameMgr.Gold += gold;

        Destroy(this.gameObject);
    }

    protected CharSpawnBase m_SpawnBase_Team;
    protected CharSpawnBase m_SpawnBase_Enemy;
    public void SetupSpawnBase(CharSpawnBase p_SpawnBase_Team, CharSpawnBase p_SpawnBase_Enemy)
    {
        this.m_SpawnBase_Team = p_SpawnBase_Team;
        this.m_SpawnBase_Enemy = p_SpawnBase_Enemy;
    }
}
