using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    CharactorCtrl cCtrl;

    // Start is called before the first frame update
    void Start()
    {
        cCtrl = this.transform.parent.GetComponent<CharactorCtrl>();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Range" && this.transform.parent.tag != collision.tag)
        {
            if (cCtrl.TargetList.Count == 0)
            {
                cCtrl.Target = collision.gameObject;
            }
            cCtrl.TargetList.Add(collision.gameObject);
        }
    }
}
