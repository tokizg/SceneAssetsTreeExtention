using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Linq;

public partial class SceneManagementHelper : EditorWindow
{
    const string WINDOW_NAME = "Scene List";
    const string UI_ELEMENTS = "UIElements";

    const string LIST_VIEW = "SceneListView";

    const string ASSETS_FOLDER_PATH = "Assets";
    const string SCENE_SEARCH_PATTERN = "t:Scene";

    [MenuItem("Scene Management Helper/Scene List")]
    private static void ShowWindow()
    {
        var window = GetWindow<SceneManagementHelper>(UI_ELEMENTS);
        window.titleContent = new GUIContent(WINDOW_NAME);
        window.Show();
    }

    [SerializeField]
    private VisualTreeAsset _sceneListTreeAsset;

    [SerializeField]
    private StyleSheet _rootStyleSheet;

    private void CreateGUI()
    {
        _sceneListTreeAsset.CloneTree(rootVisualElement);
        rootVisualElement.styleSheets.Add(_rootStyleSheet);
        PopulateSceneList(); // シーンリストの表示更新
    }

    /// <summary>
    /// シーンリストの表示を更新します。
    /// </summary>
    private void PopulateSceneList()
    {
        var listView = rootVisualElement.Q<ListView>(LIST_VIEW);
        if (listView != null)
        {
            var scenes = AssetDatabase
                .FindAssets(SCENE_SEARCH_PATTERN) // プロジェクト内の全てのシーンのGUIDを取得
                .Select(AssetDatabase.GUIDToAssetPath) // GUIDからパスに変換
                .Where(scene => scene.Contains(ASSETS_FOLDER_PATH)) // Assetsフォルダ以外のシーンは除外
                .ToList(); // リストに変換

            listView.itemsSource = scenes;
            listView.makeItem = () => new Button();
            listView.bindItem = (element, i) => // 全てのシーンに対応するボタンを作成
            {
                var button = element as Button;
                string scenePath = scenes[i];
                button.text = System.IO.Path.GetFileNameWithoutExtension(scenePath); // ファイル名のみ表示
                button.clickable.clicked += () =>
                    InEditorSceneNavigator.RequestOpenScene(scenePath); // クリック時にシーンを開くように設定
            };
            listView.Rebuild();
        }
    }
}
