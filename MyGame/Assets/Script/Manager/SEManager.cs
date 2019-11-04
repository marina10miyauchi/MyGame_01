using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : SingletonMonoBehaviour<SEManager>
{
    AudioClip[] m_se;

    Dictionary<string, int> m_seIndex = new Dictionary<string, int>();

    GameObject m_seObj;

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        //SE全ロード
        m_se = Resources.LoadAll<AudioClip>("Sound");
        for (int num = 0; num < m_se.Length; num++)
        {
            m_seIndex.Add(m_se[num].name, num);
        }

        //オブジェクト指定がなかった時用のSE源　生成と設定
        {
            m_seObj = new GameObject("SEObj");
            m_seObj.transform.parent = transform;
            m_seObj.gameObject.AddComponent<AudioSource>();
            m_seObj.GetComponent<AudioSource>().spatialize = true;
            m_seObj.GetComponent<AudioSource>().reverbZoneMix = 1.1f;
        }
    }
    
    public void PlaySE(GameObject sourceObj,string name)
    {
        PlaySE(sourceObj, GetSEIndex(name));
    }
    public void PlaySE(string name)
    {
        Vector3 cameraPos = Camera.main.transform.position;

        m_seObj.transform.position = new Vector3(cameraPos.x,cameraPos.y,cameraPos.z+2);
        PlaySE(m_seObj, GetSEIndex(name));

    }
    void PlaySE(GameObject sourceObj,int index)
    {
        index = Mathf.Clamp(index, 0, m_se.Length);
        var audio = sourceObj.GetComponent<AudioSource>();
        audio.PlayOneShot(m_se[index]);
        
    }
    int GetSEIndex(string name)
    {
        if (m_seIndex.ContainsKey(name))
        {
            return m_seIndex[name];
        }
        else
        {
            Debug.LogError("検索された名前のSEは存在しません。確認してください。");
            return 0;
        }
    }
    public void StopSE(GameObject sourceObj)
    {
        var audio= sourceObj.GetComponent<AudioSource>();
        audio.Stop();
        audio.clip = null;
    }

}
