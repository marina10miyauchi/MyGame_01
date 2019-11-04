using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : SingletonMonoBehaviour<BGMManager>
{
    AudioClip[] m_bgm;

    Dictionary<string, int> m_bgmIndex = new Dictionary<string, int>();
    AudioSource m_bgmSource;

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        m_bgmSource = gameObject.AddComponent<AudioSource>();

        m_bgm = Resources.LoadAll<AudioClip>("Sound");
        for (int num = 0; num < m_bgm.Length; num++)
        {
            m_bgmIndex.Add(m_bgm[num].name, num);
        }
    }

    //BGM再生
    public void PlayBGM(string name)
    {
        PlayBGM(GetBGMIndex(name));
    }
    void PlayBGM(int index)
    {
        index = Mathf.Clamp(index, 0, m_bgm.Length);

        m_bgmSource.clip = m_bgm[index];
        m_bgmSource.loop = true;

        m_bgmSource.Play();
    }
    int GetBGMIndex(string name)
    {
        if (m_bgmIndex.ContainsKey(name))
        {
            return m_bgmIndex[name];
        }
        else
        {
            Debug.LogError("検索された名前のBGMは存在しません。確認してください。");
            return 0;
        }
    }
    //BGM停止
    public void StopBGM()
    {
        m_bgmSource.Stop();
        m_bgmSource.clip = null;
    }
}
