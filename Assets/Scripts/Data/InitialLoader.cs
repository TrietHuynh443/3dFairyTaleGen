using System;
using System.Collections;
using System.Collections.Generic;
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
        }
    }
}
