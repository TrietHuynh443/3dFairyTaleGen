using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class InitialLoader : MonoBehaviour
{
    [SerializeField] private GameObject _image360HolderPrefab;
    private DataContainer _initialData = new DataContainer()
    {
        storyName = "Alibaba"
    };
    
    private List<GameObject> _image360Holders = new List<GameObject>();
    
    private void Awake()
    {
        var textures = _initialData.images360;
        for (int i = 0; i < textures.Length; i++)
        {
            var texture2D = textures[i];
            var newPos = new Vector3(_image360HolderPrefab.transform.localScale.x * i, 0, 0);
            var holder = Instantiate(_image360HolderPrefab, newPos, Quaternion.identity);
            var mesh = holder.GetComponent<MeshRenderer>();
            mesh.material = Resources.Load<Material>($"Materials/Inverted {i}");
            mesh.material.mainTexture = texture2D;
            
            var cam = holder.GetComponentInChildren<CinemachineVirtualCamera>();
            cam.Priority = textures.Length - i - 1;
            _image360Holders.Add(holder);
            LoadAllModelGen(holder, _initialData.storyName, i);
        }
    }

    private void LoadAllModelGen(GameObject parent, string initialDataStoryName, int i)
    {
        GameObject[] objects = Resources.LoadAll<GameObject>($"{initialDataStoryName}/Mesh/{initialDataStoryName}_{i}")
            .Where(obj => obj.name == "mesh")
            .ToArray();

        foreach (var obj in objects)
        {
            var instance = Instantiate(obj, parent.transform, true);
            SetCharacterPosition(instance);
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = 0.3f * Vector3.one;
            Debug.Log($"Spawned: {obj.name}");
        }
    }

    private void SetCharacterPosition(GameObject instance)
    {
        float randomX = Mathf.Round(Random.Range(-0.3f, 0.3f) / 0.05f) * 0.05f;
        float randomZ = Mathf.Round(Random.Range(-0.3f, 0.3f) / 0.05f) * 0.05f;
    
        instance.transform.localPosition = new Vector3(randomX, 0f, randomZ);
    }
}
