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
    private int index;
    private string monsterName;
    private string type;
    private int level;
    private int atk;
    private int def;
    private int health;
    private int atkRange;
    private int atkSpeed;
    private int moveSpeed;
    private int spawnCool;
    private int price;
    private string resourceName;
    private float charSize;
    private string reward;

    float atkCoolTime;
    float nowHealth;
    public Image HpImg;
    private bool isDie = false;
    float DeadAnimTime;

    public GameObject attackTarget = null;
    public List<GameObject> AttackedList;

    Animator animator;

    public void SetStat(MonsterData mData)
    {
        //기본 정보
        index = mData.index;
        monsterName = mData.monsterName;
        type = mData.type;
        level = mData.level;
        atk = mData.atk;
        def = mData.def;
        health = mData.health;
        atkRange = mData.atkRange;
        atkSpeed = mData.atkSpeed;
        moveSpeed = mData.moveSpeed;
        spawnCool = mData.spawnCool;
        price = mData.price;
        resourceName = mData.resourceName;
        charSize = mData.charSize;
        reward = mData.reward;
    }

    private void Start()
    {
        //임시
        moveSpeed = 1;
        atkSpeed = 1;
        atkRange = 3;
        health = 100;
        atk = 10;

        //기본 셋팅
        atkCoolTime = atkSpeed;
        DeadAnimTime = 3f;
        nowHealth = health;
        //HpImg.fillAmount = nowHealth / health;

        AttackedList = new List<GameObject>();
        animator = this.GetComponent<Animator>();

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
                    if (distance <= atkRange && distance < closestDist)
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
            if (distance > atkRange)
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
                AttackAct();

            yield return null;
        }
    }

    void AttackAct()
    {
        atkCoolTime -= Time.deltaTime;
        if (atkCoolTime <= 0f)
        {
            attackTarget.GetComponent<CharactorCtrl>().Hit(atk);

            atkCoolTime = atkSpeed;
        }

        //애니메이션 속도 조절(추후 레벨당 공속 증가 있을 시)
        //https://wergia.tistory.com/41

        //공격범위 벗어났을 때 하고있던 공격 마무리? or 캔슬
        //마무리 시 해당 함수로 "공격범위 벗어남" 이동
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
        for (int i = 0; i < AttackedList.Count; i++)
        {
            CharactorCtrl attackedCharac = AttackedList[i].GetComponent<CharactorCtrl>();

            attackedCharac.attackTarget = null;
            if (attackedCharac.AttackedList.Contains(this.gameObject))
                attackedCharac.AttackedList.Remove(this.gameObject);
        }

        m_SpawnBase_Team.RemoveMonster(this);
        animator.SetTrigger("isDie");

        yield return new WaitForSeconds(DeadAnimTime);

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
