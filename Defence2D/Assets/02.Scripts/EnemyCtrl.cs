using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public enum State
    {
        MOVE,
        ATTACK
    };
    protected State state = State.MOVE;

    protected float beat;
    protected float speed;
    protected float distance;

    protected Vector2 MovePos;

    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        if (state == State.MOVE)
            this.transform.position = Vector2.Lerp(this.transform.position, MovePos, speed * Time.deltaTime);
    }
}
