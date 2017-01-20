using UnityEditor;
using UnityEngine;

public class PrefabManagerAutoPopulate : AssetPostprocessor
{
	private const string PrefabManagerPath = "Assets/PrefabManager/PrefabManager.prefab";

	public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		bool isSavingPrefabManager = importedAssets.Length == 1 && importedAssets[0] == PrefabManagerPath && deletedAssets.Length == 0 && movedAssets.Length == 0 && movedFromAssetPaths.Length == 0;
		if (isSavingPrefabManager)
		{
			return;
		}

		var prefabManager = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabManagerPath);
		var prefabManagerComponent = prefabManager.GetComponent<PrefabManager>();
		prefabManagerComponent.Prefabs.Clear();

		var prefabs = AssetDatabase.FindAssets("t:prefab");
		foreach (string prefab in prefabs)
		{
			var path = AssetDatabase.GUIDToAssetPath(prefab);
			var prefabObj = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			prefabManagerComponent.Prefabs.Add(prefabObj);
		}

		EditorUtility.SetDirty(prefabManager);
		AssetDatabase.SaveAssets();
	}
}