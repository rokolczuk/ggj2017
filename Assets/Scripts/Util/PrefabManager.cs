using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class PrefabManager : MonoBehaviour
{
	public List<GameObject> Prefabs;

	private Dictionary<Type, List<GameObject>> prefabsByScript = new Dictionary<Type, List<GameObject>>();

	NetworkManager networkManager;

	public T Instantiate<T>() where T : MonoBehaviour
	{
		GameObject original = FindPrefabByScript<T>();
		var instance = GameObject.Instantiate(original);
		return instance.GetComponent<T>();
	}

	public T Instantiate<T>(Vector3 position, Quaternion rotation) where T : MonoBehaviour
	{
		GameObject original = FindPrefabByScript<T>();
		var instance = (GameObject)GameObject.Instantiate(original, position, rotation);
		return instance.GetComponent<T>();
	}

	// Use this for initialization
	private void Start()
	{
		networkManager = GameObject.FindObjectOfType<NetworkManager>();

		foreach (GameObject prefab in Prefabs)
		{
			foreach (MonoBehaviour monoBehaviour in prefab.GetComponents<MonoBehaviour>())
			{
				Type type = monoBehaviour.GetType();
				if (!prefabsByScript.ContainsKey(type))
				{
					prefabsByScript[type] = new List<GameObject>();
				}

				prefabsByScript[type].Add(prefab);
			}

			if (prefab.GetComponent<NetworkIdentity>() != null)
			{
				ClientScene.RegisterPrefab(prefab);
			}
		}
	}

	bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
	{
		return potentialDescendant.IsSubclassOf(potentialBase)
			|| potentialDescendant == potentialBase;
	}

	private GameObject FindPrefabByScript<T>() where T : MonoBehaviour
	{
		List<GameObject> gameObjectsWithScript;

		bool found = prefabsByScript.TryGetValue(typeof(T), out gameObjectsWithScript);

		if (!found)
		{
			throw new Exception("No prefab found with script " + typeof(T));
		}

		if (gameObjectsWithScript.Count > 1)
		{
			string[] strings = gameObjectsWithScript.Select(o => o.name).ToArray();
			Debug.LogError(string.Format("Script {0} does not uniquely identify a prefab. Candidates are: {1}", typeof(T), string.Join(",", strings)));
			throw new Exception("Multiple prefabs for script " + typeof(T));
		}

		GameObject original = gameObjectsWithScript[0];
		return original;
	}
}