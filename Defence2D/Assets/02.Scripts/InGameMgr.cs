using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMgr : MonoBehaviour
{
    int Gold = 0;
    float GoldDelay = 1f;
    float tempGoldDelay = 1f;
    public Text GoldTxt;

    public GameObject TempCharac;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Spawn(int _idx)
    {
        TempCharac.gameObject.tag = "Player";
        Instantiate(TempCharac, new Vector3(-16f, -0.8f, 0f), TempCharac.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        tempGoldDelay -= Time.deltaTime;
        if (tempGoldDelay < 0f)
        {
            Gold++;
            GoldTxt.text = Gold.ToString();
            tempGoldDelay = GoldDelay;
        }
    }
}
