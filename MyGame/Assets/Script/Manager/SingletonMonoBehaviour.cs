using UnityEngine;
using System;

public abstract class SingletonMonoBehaviour<T>:MonoBehaviour where T: MonoBehaviour
{

    private static T instance;
    public static T Instance
    {
        get
        {
            //指定したインスタンスがnullか？
            if (instance == null)
            {
                //typeof = インスタンスを作成したクラスの判別
                Type t = typeof(T);
                //最初に見つけたアクティブオブジェクトを返し格納
                instance = (T)FindObjectOfType(t);
                //指定したインスタンスがnullか？
                if (instance == null)
                {
                    Debug.LogError(t + "をアタッチしているGameObjectはありません");
                }
            }
            return instance;
        }
    }

    virtual protected void Awake()
    {
        //他のゲームオブジェクトにアタッチされているかを調べる
        //アタッチされている場合は破棄する
        CheckInstance();
    }
    protected bool CheckInstance()
    {
        if (instance == null)
        {
            //自分をセット
            instance = this as T;
            return true;
        }
        else if(Instance == this)
        {
            return true;
        }

        Destroy(this);
        return false;
    }
}