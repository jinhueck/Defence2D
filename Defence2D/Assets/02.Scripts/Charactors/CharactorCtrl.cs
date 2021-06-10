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

    float nowHealth;
    public Image HpImg;
    private bool isDie = false;

    GameObject attackTarget = null;
    //CharSpawn charSpawn;

    //임시 타겟리스트, 스폰스크립트에 생성
    public List<GameObject> TargetList;

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
        //기본 셋팅
        nowHealth = health;
        HpImg.fillAmount = nowHealth / health;

        ChangeState(State.Move);

        //임시 공격 타겟 리스트
        TargetList = new List<GameObject>();
    }

    public void ChangeState(State newState)
    {
        StopCoroutine(state.ToString());
        state = newState;
        StartCoroutine(state.ToString());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            //캐릭터 이동
            if (this.transform.tag == "Player")
                this.transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0f, 0f));
            else if (this.transform.tag == "Enemy")
                this.transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f));

            //Search Target
            float closestDist = Mathf.Infinity;

            for (int i = 0; i < TargetList.Count; i++)
            {
                float distance = Mathf.Abs(TargetList[i].transform.position.x - this.transform.position.x);
                if (distance <= atkRange && distance < closestDist)
                {
                    closestDist = distance;
                    attackTarget = TargetList[i];
                }
            }

            if (attackTarget != null)
                ChangeState(State.Attack);

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
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

            //공격 쿨타임
            yield return new WaitForSeconds(atkSpeed);

            //공격
            attackTarget.GetComponent<CharactorCtrl>().Hit(atk);
        }
    }

    public void Hit(float _damage)
    {
        if (isDie)
            return;

        nowHealth -= _damage;
        HpImg.fillAmount = nowHealth / health;

        //애니메이션
        StopCoroutine("HitAnim");
        StartCoroutine("HitAnim");

        if (nowHealth <= 0f)
        {
            isDie = true;
            Die();
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

    public void Die()
    {
        //CharSpawn에 List삭제, Destroy함수 추가
        //charSpawn.Destroy(this);
    }
}
