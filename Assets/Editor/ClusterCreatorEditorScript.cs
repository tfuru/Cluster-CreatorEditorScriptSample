using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 1. UnityEditor を追加
using UnityEditor;
using UnityEditor.SceneManagement;

// 11. ClusterCreatorKit を利用する設定の追加
using ClusterVR.CreatorKit.Item;
using ClusterVR.CreatorKit.Item.Implements;
using ClusterVR.CreatorKit.Gimmick;
using ClusterVR.CreatorKit.Gimmick.Implements;
using ClusterVR.CreatorKit.Trigger.Implements;

// 2. namespace  ClusterCreatorEditorScript を追加
namespace ClusterCreatorEditorScript
{
    // 3. class を EditorScript に名変更
    class EditorScript: EditorWindow
    {
        // 8. シーンに必須コンポーネントを配置 する実際のコードで利用するメンバ変数
        // プレハブフォルダ名 Assets/Prefab
        private static string PATH_DIST_REFAB_FOLDER_NAME = "Prefab";

        private static string PATH_SRC_DESPAWN_HEIGHTP_REFAB = "Assets/Editor/Items/DespawnHeight.prefab";
        private static string PATH_DIST_DESPAWN_HEIGHTP_REFAB = "Assets/Prefab/DespawnHeight.prefab";
        // private static Vector3[] PATH_DIST_DESPAWN_HEIGHTP_REFAB_TRANSFORM = {new Vector3(0,-10,0), Vector3.zero, new Vector3(1,1,1)};

        private static string PATH_SRC_SPAWNPOINT_REFAB = "Assets/Editor/Items/SpawnPoint.prefab";
        private static string PATH_DIST_SPAWNPOINT_REFAB = "Assets/Prefab/SpawnPoint.prefab";
        // private static Vector3[] PATH_DIST_SPAWNPOINT_TRANSFORM = {new Vector3(0,2,-4), Vector3.zero, new Vector3(1,1,1)};

        private static string PATH_SRC_FLOOR_REFAB = "Assets/Editor/Items/Floor.prefab";
        private static string PATH_DIST_FLOOR_REFAB = "Assets/Prefab/Ground.prefab";
        // private static Vector3[] PATH_DIST_FLOOR_TRANSFORM = {new Vector3(0,0,0), Vector3.zero, new Vector3(1,1,1)};

	    // 12. エディターの GUI で使う メンバ変数を定義する
	    private static GameObject selectGrabbableObject = null;

        // 4. Start(), Update() は不要なので削除する
        // 5. メニューに "ClusterCreatorEditor/ワールド初期設定" を追加するコードを追加
        [MenuItem ("ClusterCreatorEditor/ワールド初期設定")]
        private static void InitWorld()
        {
            // メニューで選択されたら シーンに必須コンポーネントを配置 するメソッドを実行する
            InitScene();
        }

        // 6. シーンに必須コンポーネントを配置 するコードを追加する
        private static void InitScene() {
            Debug.Log("InitScene()");

            // 9. シーンに必須コンポーネントを配置 する実際のコード
            // Prefab ディレクトリを作成
            Debug.Log(PATH_DIST_REFAB_FOLDER_NAME);
            if (AssetDatabase.IsValidFolder("Assets/" + PATH_DIST_REFAB_FOLDER_NAME) == false){
                Debug.Log("Create REFAB_FOLDER");
                AssetDatabase.CreateFolder("Assets", PATH_DIST_REFAB_FOLDER_NAME);
            }

            // Cluster 必須コンポーネントを Prefabから生成 する
            CreatePrefab(PATH_SRC_FLOOR_REFAB, PATH_DIST_FLOOR_REFAB /*, PATH_DIST_FLOOR_TRANSFORM */);
            CreatePrefab(PATH_SRC_SPAWNPOINT_REFAB, PATH_DIST_SPAWNPOINT_REFAB /*, PATH_DIST_SPAWNPOINT_TRANSFORM */);
            CreatePrefab(PATH_SRC_DESPAWN_HEIGHTP_REFAB, PATH_DIST_DESPAWN_HEIGHTP_REFAB /*, PATH_DIST_DESPAWN_HEIGHTP_REFAB_TRANSFORM */);

            // シーンを保存する
            EditorSceneManager.SaveOpenScenes();            
        }

        // 10. Cluster 必須コンポーネントを Prefab から生成 する
        private static void CreatePrefab(string pathSrc, string pashDist/*, Vector3[] transform*/) {
            // すでにあった場合は処理をしない
            GameObject dist = AssetDatabase.LoadAssetAtPath<GameObject>(pashDist);
            if (dist == null) {
                // Prefabから生成 する
                var src = PrefabUtility.LoadPrefabContents(pathSrc);
                dist = PrefabUtility.SaveAsPrefabAsset(src, pashDist);
                PrefabUtility.UnloadPrefabContents(src);
            }

            // シーンに Prefab を配置する
            // dist.transform.position = transform[0];
            // dist.transform.rotation = Quaternion.Euler(transform[1]);
            // dist.transform.localScale = transform[2];
            PrefabUtility.InstantiatePrefab(dist);
        }

	    // 13. メニューの追加　ClusterCreatorEditor/ユーティリティ
        [MenuItem ("ClusterCreatorEditor/ユーティリティ")]
        private static void OpenUtility()
        {
            // メニューで選択されたら ウインドウを開く
            EditorWindow.GetWindow<EditorScript>("ClusterCreatorEditor");
        }

        // 14. エディターの GUI を実装
        void OnGUI () {
            using (new GUILayout.VerticalScope())
            {
                EditorGUILayout.LabelField("Grabbable Item", EditorStyles.boldLabel);
                using (new GUILayout.HorizontalScope())
                {

                EditorGUILayout.LabelField("GameObject",  GUILayout.Width (100));                
                selectGrabbableObject = EditorGUILayout.ObjectField(selectGrabbableObject, typeof(Object), true) as GameObject;
                }

                if (GUILayout.Button("addGrabbable"))
                {
                    // selectGrabbableObject に Grabbable Item を設定する
                    addGrabbableObject();
                }
            }
        }
        
        // 15. selectGrabbableObject に Grabbable Item を設定する
        private static void addGrabbableObject(){
            if (selectGrabbableObject == null) {
                Debug.Log(string.Format("<color=#ff0000>{0}</color>", "GameObject を指定してください"));
                return;
            }
            // CreatorKit で定義されているコンポーネントを AddComponent する
            selectGrabbableObject.AddComponent<GrabbableItem>();
        }               
    }
}
