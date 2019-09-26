using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField,Range(0, 1),Header("マスタ音量")]
    float volume = 1;
    [SerializeField, Range(0, 1), Header("BGMの音量")]
    float bgmVolume = 0.5f;
    [SerializeField, Range(0, 1), Header("SEの音量")]
    float seVolume = 0.5f;


    AudioClip[] bgm;    
    AudioClip[] se;     

    //soundの管理
    Dictionary<string, int> bgmIndex = new Dictionary<string, int>();
    Dictionary<string, int> seIndex = new Dictionary<string, int>();

    AudioSource bgmAudioSource;
    AudioSource seAudioSource;

    public float BGMVolume
    {
        get { return bgmVolume; }
        set
        {
            bgmVolume = Mathf.Clamp01(value);
            bgmAudioSource.volume = bgmVolume * volume;
        }
    }
    public float SEVolume
    {
        get { return seVolume; }
        set { seVolume = Mathf.Clamp01(value);
            seAudioSource.volume = seVolume * volume;
        }
    }

    public void Awake()
    {
        //他のSoundManagerはないか？
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        //シーンが切り替わっても削除されない
        DontDestroyOnLoad(gameObject);
        //このゲームオブジェクトにAudioSourceを追加
        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        seAudioSource = gameObject.AddComponent<AudioSource>();

        //音源すべての読み込み（）内はパス 
        /*TODO:これでロードする場合ResourcesフォルダーをAssets内に作成しその中にSoundなどのファイルを作成する必要がある
               Resourcesと名の付くフォルダ全てが対象となってしまうため重くなる可能性大
               色々と直す必要性が出てきそう*/
        bgm = Resources.LoadAll<AudioClip>("Sound/BGM");
        se = Resources.LoadAll<AudioClip>("Sound/SE");
        //全ての音源をインデックス管理をできるように
        {
            for (int i = 0; i < bgm.Length; i++)
            {
                bgmIndex.Add(bgm[i].name, i);
            }
            for (int i = 0; i < se.Length; i++)
            {
                seIndex.Add(se[i].name, i);
            }
        }
    }
    /// <summary>
    /// 名前からBGM番号の検索
    /// </summary>
    /// <param name="name">BGMのファイル名</param>
    /// <returns></returns>
    public int GetBGMIndex(string name)
    {
        if (bgmIndex.ContainsKey(name))
        {
            return bgmIndex[name];
        }
        else
        {
            Debug.LogError("検索された名前のBGMは存在しません。確認してください。");
            return 0;
        }
    }
    /// <summary>
    /// 名前からSE番号の検索
    /// </summary>
    /// <param name="name">SEのファイル名</param>
    /// <returns></returns>
    public int GetSEIndex(string name)
    {
        if (seIndex.ContainsKey(name))
        {
            return seIndex[name];
        }
        else
        {
            Debug.LogError("検索された名前のSEは存在しません。確認してください。");
            return 0;
        }
    }
    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="index">BGM番号</param>
    public void PlayBGM(int index)
    {
        index = Mathf.Clamp(index, 0, bgm.Length);

        bgmAudioSource.clip = bgm[index];
        bgmAudioSource.loop = true;
        bgmAudioSource.volume = bgmVolume * volume;
        bgmAudioSource.Play();
    }
    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="name">BGMのファイル名</param>
    public void PlayBGMByName(string name)
    {
        PlayBGM(GetBGMIndex(name));
    }
    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="index">SE番号</param>
    public void PlaySE(int index)
    {
        index = Mathf.Clamp(index, 0, se.Length);
        seAudioSource.volume = seVolume * volume;
        seAudioSource.PlayOneShot(se[index]);
    }
    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="name">SEのファイル名</param>
    public void PlaySEByName(string name)
    {
        PlaySE(GetSEIndex(name));
    }
    public void StopSE()
    {
        seAudioSource.Stop();
        seAudioSource.clip = null;
    }
    public void StopBGM()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = null;
    }
}
