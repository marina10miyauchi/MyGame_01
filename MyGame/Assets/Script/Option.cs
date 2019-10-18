using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Select
{
    Null,
    BGM,
    SE,
    Return,
}

public class Option : MonoBehaviour
{
    Slider bgmSlider_;
    Slider seSlider_;

    Select thisSelectNum;

    // Start is called before the first frame update
    void Start()
    {
        thisSelectNum = Select.Null;
        bgmSlider_ = GameObject.Find("BGMSlider").GetComponent<Slider>();
        seSlider_ = GameObject.Find("SESlider").GetComponent<Slider>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (thisSelectNum)
        {
            case Select.Null:
                break;
            case Select.BGM:
                break;
            case Select.SE:
                break;
            case Select.Return:
                if(Input.GetKeyDown("joystic button 1"))
                {
                    //Scene_Manager.Instance.ChangeScene()
                }
                break;

        }
    }
    void SelectMove()
    {

    }
}
