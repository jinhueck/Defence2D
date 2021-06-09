using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorCtrl : MonoBehaviour
{
    public enum State
    {
        MOVE,
        ATTACK,
        DIE
    };
    public State state = State.MOVE;

    public int key;

    protected float MoveSpeed;
    protected float Hp;
    public float nowHp;
    protected float AttackPower;
    protected float AttackDelay;
    protected float Protection;
    protected float AttackRange;

    float tempAttackDelay = 0f;
    public Image HpImg;

    public List<GameObject> TargetList;
    public GameObject Target = null;


    protected virtual void Start()
    {
        HpImg.fillAmount = nowHp / Hp;
        TargetList = new List<GameObject>();
        this.transform.GetChild(0).GetComponent<BoxCollider2D>().size = new Vector2(AttackRange, 10f);
        this.GetComponent<BoxCollider2D>().enabled = true;
    }

    //protected void SetAttackRange(float _AttackRange)
    //{
    //    HpImg.fillAmount = nowHp / Hp;
    //    TargetList = new List<GameObject>();
    //    this.transform.GetChild(0).GetComponent<BoxCollider2D>().size = new Vector2(_AttackRange, 10f);
    //}

    void StateCheck()
    {
        if (nowHp <= 0f)
        {
            state = State.DIE;
            return;
        }

        if (TargetList.Count == 0)
            state = State.MOVE;
        else
            state = State.ATTACK;
    }

    void UpdateTarget()
    {
        foreach(GameObject target in TargetList)
        {
            if (Vector2.Distance(this.transform.position, target.transform.position)
                < Vector2.Distance(this.transform.position, Target.transform.position))
            {
                Target = target;
            }
        }

    }

    public virtual void Move()
    {
        this.GetComponent<SpriteRenderer>().color = Color.white;

        if (this.transform.tag == "Player")
            this.transform.Translate(new Vector3(MoveSpeed * Time.deltaTime, 0f, 0f));
        else if (this.transform.tag == "Enemy")
            this.transform.Translate(new Vector3(-MoveSpeed * Time.deltaTime, 0f, 0f));
    }

    void Attack()
    {
        tempAttackDelay -= Time.deltaTime;
        if (tempAttackDelay < 0f)
        {
            Target.GetComponent<CharactorCtrl>().GetDamage(AttackPower);
            this.GetComponent<SpriteRenderer>().color = Color.red;
            tempAttackDelay = AttackDelay;
        }

        if (Target.GetComponent<CharactorCtrl>().nowHp <= 0f)
        {
            this.TargetList.Remove(Target);
            UpdateTarget();
        }
    }

    public void GetDamage(float _damage)
    {
        //print(_damage);
        nowHp -= _damage;
        HpImg.fillAmount = nowHp / Hp;
    }

    void Die()
    {
        this.GetComponent<SpriteRenderer>().color = Color.black;
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.TargetList.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        StateCheck();

        switch (state)
        {
            case State.MOVE:
                Move();
                break;

            case State.ATTACK:
                Attack();
                break;

            case State.DIE:
                Die();
                break;

        }
    }
}
