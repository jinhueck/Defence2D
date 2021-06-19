using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCtrl : MonoBehaviour
{
    int Type;
    GameObject AttackTarget;
    float Damage;

    float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 5f;
    }

    public void Setup(int _type, GameObject _atkTarget, float _damage, string _camp)
    {
        Type = _type;
        AttackTarget = _atkTarget;
        Damage = _damage;
        this.transform.tag = _camp;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.tag == "Player")
            this.transform.Translate(new Vector3(speed * Time.deltaTime, 0f, 0f));
        else if (this.transform.tag == "Enemy")
            this.transform.Translate(new Vector3(-speed * Time.deltaTime, 0f, 0f));

        switch (Type)
        {
            case 1:
                float damageDist = Mathf.Abs(AttackTarget.transform.position.x - this.transform.position.x);
                if (damageDist <= 0.1f)
                {
                    if (AttackTarget != null)
                    {
                        AttackTarget.GetComponent<CharactorCtrl>().Hit(Damage);
                        Destroy(this.gameObject);
                    }
                }
                break;

            case 2:
                //float closetDist = Mathf.Infinity;
                //if (m_SpawnBase_Enemy != null)
                //{

                //}
                break;
        }
    }
}
