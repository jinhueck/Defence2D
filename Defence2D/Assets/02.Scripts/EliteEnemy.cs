using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy : EnemyCtrl
{
    // Start is called before the first frame update
    void Start()
    {
        SetInfo();

        StartCoroutine(Beat());
    }

    void SetInfo()
    {
        beat = 0.5f;
        speed = 10.0f;
        distance = 0.4f;
        MovePos = new Vector2(-1.8f, 5.4f);
    }

    void MoveCheck()
    {
        MovePos.y -= distance;
    }

    IEnumerator Beat()
    {
        while (state == State.MOVE)
        {
            MoveCheck();
            yield return new WaitForSeconds(beat);
        }
    }
}
