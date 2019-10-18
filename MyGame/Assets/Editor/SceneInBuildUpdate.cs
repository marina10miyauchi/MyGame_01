using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class SceneInBuildUpdate : AssetPostprocessor
{
    private const string scene_build_path = "Assets/Scenes";

    //ファイルの変更が行われたかのチェック
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        //チェックするアッセットのパスリスト
        List<string[]> assetsList = new List<string[]>()
        {
            importedAssets,deletedAssets,movedAssets,movedFromAssetPaths
        };
        //チェックするディレクトリのリスト
        List<string> targetDirectoryNameList = new List<string>()
        {
            scene_build_path,
        };
        //変更されたファイルに指定のディレクトリ内の物が含まれている時はビルドするシーンを更新
        if (ExistsDirectoryInAssets(assetsList, targetDirectoryNameList))
        {
            UpdateSceneInBuild();
        }
        EditorBuildSettingsScene scene = new EditorBuildSettingsScene("", true);
    }
   
    //入力されたassetsの中に、ディレクトリのパスがdirectoryNameの物はあるか
    protected static bool ExistsDirectoryInAssets(List<string[]> assetsList, List<string> targetDirectoryNameList)
    {
        return assetsList
            .Any(assets => assets                                           //
            .Select(asset => System.IO.Path.GetDirectoryName(asset))        //
            .Intersect(targetDirectoryNameList)                             //
            .Count() > 0);                                                  //

    }
    [MenuItem("Tools/Update/Scenes In Build")]
    private static void UpdateSceneInBuild()
    {
        //Sceneファイルのパスを格納するリスト
        List<string> pathList = new List<string>();
        //最初のシーンに設定するディレクトリパス
        string firstScenePath = "";

        //Assetsの検索をし、検索したアセット全てに処理を行う　(t: はラベル)
        foreach (var guid in AssetDatabase.FindAssets("t:Scene"))
        {
            //guidのGUIDをアセットパスに変換
            string path = AssetDatabase.GUIDToAssetPath(guid);
            //pathをファイル名の拡張子を付けずない形に変換
            string sceneNama = System.IO.Path.GetFileNameWithoutExtension(path);

            //指定した文字列のパス以外に入っているものはスルー
            if (!path.Contains(scene_build_path))
            {
                continue;
            }
            //シーン名がかぶっているか？
            if (pathList.Contains(sceneNama))
            {
                //エラー
                Debug.LogError(sceneNama + "というシーン名が重複しています。");
            }
            //Firstという名のディレクトに入っているならば最初のシーンに設定
            else if (System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(path)) == "First")
            {
                //firstScenePathがnullもしくは""の場合
                if (!string.IsNullOrEmpty(firstScenePath))
                {
                    Debug.LogError("Firstディレクトリに複数のシーンが入っています。");
                }
                firstScenePath = path;
            }
            else
            {
                pathList.Add(path);
            }
        }
        List<EditorBuildSettingsScene> sceneList = new List<EditorBuildSettingsScene>();
        //最初のシーンを一番最初に追加
        if (!string.IsNullOrEmpty(firstScenePath))
        {
            sceneList.Add(new EditorBuildSettingsScene(firstScenePath, true));
        }

        foreach (string path in pathList)
        {
            sceneList.Add(new EditorBuildSettingsScene(path, true));
        }
        //Sceneを追加
        EditorBuildSettings.scenes = sceneList.ToArray();
    }
}
