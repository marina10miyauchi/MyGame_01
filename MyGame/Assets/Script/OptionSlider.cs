using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSlider : MonoBehaviour
{
    Slider bgmSlider_;
    Slider seSlider_;

    Text bgmText_;
    Text seText_;
    
    // Start is called before the first frame update
    void Start()
    {
        bgmSlider_ = GameObject.Find("BGMSlider").GetComponent<Slider>();
        seSlider_ = GameObject.Find("SESlider").GetComponent<Slider>();

        bgmSlider_.value = SoundManager.Instance.BGMVolume;
        seSlider_.value = SoundManager.Instance.SEVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BGMOption()
    {
        SoundManager.Instance.BGMVolume =(float)bgmSlider_.value;
        float bgmVolume = SoundManager.Instance.BGMVolume * 100;
        Debug.Log("BGM:" + (int)bgmVolume);
    }
    public void SEOption()
    {
        SoundManager.Instance.SEVolume = (float)seSlider_.value;
        float seVolume = SoundManager.Instance.SEVolume * 100;
        Debug.Log("SE:" + (int)seVolume);
    }


}
