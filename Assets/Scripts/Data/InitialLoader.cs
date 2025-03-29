using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using EventProcessing;
using Network;
using UnityEngine;
using Random = UnityEngine.Random;

public class InitialLoader : UnitySingleton<InitialLoader>
{
    [SerializeField] private GameObject _image360HolderPrefab;
    [SerializeField] private NetworkManager _networkManager;
    private AudioClip[] _audioClips;
    private DataContainer _initialData = new DataContainer()
    {
        storyName = "Alibaba"
    };
    
    public static bool IsInitialized { get; private set; } = false;
    public static int CurrentScene;
    
    private List<GameObject> _image360Holders = new List<GameObject>();
    public static int MaxScene = 0;

    private async void Start()
    {
        var story =
            "AliBaba was in the forest getting firewood.\\n\\nHe heard a clatter of hooves on the path. He was afraid. He climbed the nearest tree and hid.\\n\\nHe peeked out and saw forty men on horseback.\\n\\nAli Baba in the forest\\n\\nThe forty riders reached a cliff. In the bright sunlight Ali Baba could see that their saddlebags were full of glistening and sparkling gold.\\n\\n“Those bags are full of stolen treasure!” he thought.\\n\\nThen the leader cried, “Open Sesame!”\\n\\nSuddenly, a great big secret door in the rock opened. The forty riders rode inside.\\n\\nWhen they were all inside, the leader shouted, “Close Sesame!”\\n\\nThe secret door closed.\\n\\nAli Baba waited, still hiding in his treetop. Then the riders all came out of the cave.\\n\\nThe leader shouted, “Close Sesame!”\\n\\nThe secret door closed.\\n\\nAli Baba peered through the leaves.\\n\\nThe thieves put empty saddlebags over their horses and set off.\\n\\nAli Baba listened until they were gone. He was safe!\\n\\nHe climbed down the tree. He wondered if the magic door would open for him if he said the special words.\\n\\nAli Baba looked at the rocky cliff and thought about all the treasure inside.\\n\\n“They’ve gone,” he thought, “so I shall say those magic words.”\\n\\n“Open Sesame!” he said. The door opened.\\n\\nAli Baba saw steps leading down. Then he remembered what the leader had said to close the magic door.\\n\\n“Close Sesame!” he cried, and the door slid shut.\\n\\nIn the treasure cave Ali Baba gazed around in amazement. There were heaps of sparkling jewels and piles of gold coins.\\n\\nAli Baba surrounded by jewels and gold\\n\\nHe wasn’t greedy. He didn’t want the jewels. All he wanted was just one little gold coin to buy food for him and his family.\\n\\nSo Ali Baba took one and ran back up the stone steps.\\n\\n“Open Sesame!” he said.\\n\\nImmediately the magic door opened. He hurried outside. Then he called out, “Close Sesame!”\\n\\nThe door slid shut. Ali Baba was safe.\\n\\nBut Ali Baba’s brother, Kassim, had also come to get firewood. He watched Ali Baba come out of the cave and say the magic words.\\n\\n“Oh, Ali Baba! What is this magic?”\\n\\nAli Baba shows Kassim the gold\\n\\nAli Baba told his brother and said that the forty thieves might soon come back. So they both hurried home.\\n\\nKassim told his wife all about his brother’s adventure.\\n\\n“Kassim,” she said, “please go back and bring me a little gold coin.”\\n\\nIn front of the cliff, Kassim said the magic words, “Open Sesame!”\\n\\nWhen the secret door opened, Kassim hurried inside.\\n\\nHe spoke the magic words to close the door, “Close Sesame!”\\n\\nKassim ran down the steps.\\n\\nWhen he saw all the sparkling jewels and gold, his eyes lit up.\\n\\nKassim was so excited that he forgot the magic words! He ran up to the magic door.\\n\\n“Open Porridge!” he cried.\\n\\nNothing happened.\\n\\n“Open Marshmallows!”\\n\\nNothing happened.\\n\\n“I know it was something to eat,” he said to himself.\\n\\n“Open Toast! Open Bananas! Open Fish!”\\n\\nJust then the magic door slid open and there stood the thieves!\\n\\nKassim ran off as fast as he could. He climbed up a tree and hid.\\n\\nKassim’s wife was worried when her husband didn’t come home.\\n\\nShe went to Ali Baba.\\n\\n“I'd better go and find him,” Ali Baba said to her.\\n\\nAli Baba took a piece of gooey chocolate cake to eat and went to look for his brother.\\n\\nHe looked everywhere. He sat outside the cave and ate his cake.\\n\\nAli Baba eats chocolate\\n\\nSome crumbs fell on the ground.\\n\\nThe next day the thieves went back to the cave.\\n\\n“Stop!” the leader cried. He bent down and saw the chocolate crumbs.\\n\\nOne of the thieves said, “I saw Ali Baba at the bakery shop yesterday. He bought a delicious triple chocolate cake with a gold coin.”\\n\\n“That means he knows our hiding place,” said the leader, “now go back to the village and get a cart and a strong donkey. Buy thirty-nine olive oil jars big enough to hide inside. Make a hole in the lids for fresh air.”\\n\\nKassim was still hiding in the tree and heard every word. He rushed back and told Ali Baba.\\n\\nThat night the leader went to Ali Baba’s house and stood outside.\\n\\n“When I come back here tomorrow,” he said to himself, “I shall pretend to be a seller of olive oil. When I shout, ‘Olive oil!’ my thieves will jump out of the jars, ready for a fight.”\\n\\nThe next morning a thief hid in each of the olive oil jars. Their leader put a lid on the top.\\n\\nThe donkey pulled the cart to Ali Baba’s house. Then the leader knocked on the door.\\n\\nA donkey and cart outside Ali Baba’s house\\n\\nAli Baba opened the door and said, “Oh, good. We need olive oil.”\\n\\nWhile they were talking Ali Baba’s wife got a dish of stinky cheese, and she plugged up all the breathing holes in the jars.\\n\\nThe stinky cheese made the thieves feel so sick that they all fell out of the jars onto the ground.\\n\\nThen the guards arrived, they captured all forty thieves and took them to prison.\\n\\nThe thieves became sick and the guards arrived to take them to prison\\n\\nThen they brought all the treasure back to the village. They gave all the jewels and the money back to their rightful owners.\\n\\nEveryone was thrilled and celebrated with a big party.";
        
        _audioClips = await _networkManager.GetAudioClip(story, "Alibaba");

        var textures = _initialData.images360;
        for (int i = 0; i < textures.Length; i++)
        {
            var texture2D = textures[i];
            var newPos = new Vector3(_image360HolderPrefab.transform.localScale.x * i, 0, 0);
            var holder = Instantiate(_image360HolderPrefab, newPos, Quaternion.identity);
            var mesh = holder.GetComponent<MeshRenderer>();
            mesh.material = Resources.Load<Material>($"Materials/Inverted {i}");
            mesh.material.mainTexture = texture2D;
            
            var audioSource = holder.GetComponentInChildren<AudioSource>();
            if (_audioClips != null && i <= _audioClips.Length - 1)
            {
                audioSource.clip = _audioClips[i];
            }
            
            var cam = holder.GetComponentInChildren<CinemachineVirtualCamera>();
            cam.Priority = textures.Length - i - 1;
            _image360Holders.Add(holder);
            LoadAllModelGen(holder, _initialData.storyName, i);
        }
        MaxScene = textures.Length - 1;

        EventAggregator.Instance.RaiseEvent(new InitialLoadingFinish());
        IsInitialized = true;
    }

    private void OnEnable()
    {
        EventAggregator.Instance.AddEventListener<OnChangeParagraphEvent>(LoadParagraph);
    }

    private void OnDisable()
    {
        EventAggregator.Instance?.RemoveEventListener<OnChangeParagraphEvent>(LoadParagraph);
    }

    private void LoadParagraph(OnChangeParagraphEvent obj)
    {
        CinemachineVirtualCamera cam;
        cam = _image360Holders[CurrentScene].GetComponentInChildren<CinemachineVirtualCamera>();
        if (obj.IsNext && CurrentScene < MaxScene)
        {
            ++CurrentScene;
        }
        else if(!obj.IsNext && CurrentScene > 0)
        {
            --CurrentScene;
        }
        else
        {
            return;
        }
        cam.Priority = int.MinValue;
        cam = _image360Holders[CurrentScene].GetComponentInChildren<CinemachineVirtualCamera>();
        var audioPlayer = _image360Holders[CurrentScene].GetComponentInChildren<SceneEnity.AudioPlayer>();
        audioPlayer.ForcePlay();
        cam.Priority = int.MaxValue;
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
