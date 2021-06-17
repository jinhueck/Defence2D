using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMgr : MonoBehaviour
{
    public int Gold = 0;
    float GoldDelay = 1f;
    float tempGoldDelay = 1f;
    public Text GoldTxt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tempGoldDelay -= Time.deltaTime;
        if (tempGoldDelay < 0f)
        {
            Gold++;
            tempGoldDelay = GoldDelay;
        }

        GoldTxt.text = Gold.ToString();
    }
}
