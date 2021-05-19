using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupEnemy : EnemyCtrl
{
    private bool isRight = true;

    // Start is called before the first frame update
    void Start()
    {
        SetInfo();

        StartCoroutine(Beat());
    }

    void SetInfo()
    {
        beat = 1.0f;
        speed = 10.0f;
        distance = 0.2f;
        MovePos = new Vector2(0f, 4f);
    }


    void MoveCheck()
    {
        if (isRight)
        {
            if (MovePos.x >= 0.9f)
            {
                MovePos.y -= distance;
                isRight = !isRight;
            }
            else
                MovePos.x += distance;
        }
        else
        {
            if (MovePos.x <= -0.9f)
            {
                MovePos.y -= distance;
                isRight = !isRight;
            }
            else
                MovePos.x -= distance;
        }
        //Debug.Log(MovePos.x);
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
