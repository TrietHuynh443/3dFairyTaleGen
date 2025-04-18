using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using EventProcessing;
using Network;
using SceneEnity;
using SO;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class InitialLoader : UnitySingleton<InitialLoader>
{
    [SerializeField] private GameObject _image360HolderPrefab;
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private EnvironmentSO _environmentSO;
    private AudioClip[] _audioClips;
    private UnityEngine.AudioSource[] _audioSources;

    private DataContainer _initialData = new DataContainer()
    {
        storyName = "Alibaba"
    };
    
    public bool IsCurrentAudioPlaying => _audioSources[CurrentScene].isPlaying;

    public static bool IsInitialized { get; private set; } = false;
    public static int CurrentScene;

    private List<GameObject> _image360Holders = new List<GameObject>();
    private OVRPlayerController _playerController;
    private CharacterController _characterController;
    private CinemachineBrain _cinemachineBrain;
    private string[] _paragraphs;
    private OVRVignette _vignette;
    public static int MaxScene = 0;

    protected override void SingletonAwake()
    {
        _playerController = GetComponent<OVRPlayerController>();
        _characterController = GetComponent<CharacterController>();
        Transform canvasTransform = GameObject.Find("CenterEyeAnchor").transform;
        _vignette = canvasTransform.GetComponent<OVRVignette>();
    }

    private async void Start()
    {
        var story =
            "In a town in Persia there dwelt two brothers, one named Cassim, the other Ali Baba. Cassim was married to a rich wife and lived in plenty, while Ali Baba had to maintain his wife and children by cutting wood in a neighboring forest and selling it in the town.\n\nOne day, when Ali Baba was in the forest, he saw a troop of men on horseback, coming toward him in a cloud of dust. He was afraid they were robbers, and climbed into a tree for safety. When they came up to him and dismounted, he counted forty of them. They unbridled their horses and tied them to trees.\n\nThe finest man among them, whom Ali Baba took to be their captain, went a little way among some bushes, and said, \"Open, Sesame!\" so plainly that Ali Baba heard him.\n\nA door opened in the rocks, and having made the troop go in, he followed them, and the door shut again of itself. They stayed some time inside, and Ali Baba, fearing they might come out and catch him, was forced to sit patiently in the tree. At last the door opened again, and the Forty Thieves came out. As the Captain went in last he came out first, and made them all pass by him; he then closed the door, saying, \"Shut, Sesame!\"\n\nEvery man bridled his horse and mounted, the Captain put himself at their head, and they returned as they came.\n\nThen Ali Baba climbed down and went to the door concealed among the bushes, and said, \"Open, Sesame!\" and it flew open.\n\nAli Baba, who expected a dull, dismal place, was greatly surprised to find it large and well lighted, hollowed by the hand of man in the form of a vault, which received the light from an opening in the ceiling. He saw rich bales of merchandise -- silk, stuff-brocades, all piled together, and gold and silver in heaps, and money in leather purses. He went in and the door shut behind him. He did not look at the silver, but brought out as many bags of gold as he thought his asses, which were browsing outside, could carry, loaded them with the bags, and hid it all with fagots.\n\nUsing the words, \"Shut, Sesame!\" he closed the door and went home.\n\nThen he drove his asses into the yard, shut the gates, carried the money-bags to his wife, and emptied them out before her. He bade her keep the secret, and he would go and bury the gold.\n\n\"Let me first measure it,\" said his wife. \"I will go borrow a measure of someone, while you dig the hole.\"\n\nSo she ran to the wife of Cassim and borrowed a measure. Knowing Ali Baba's poverty, the sister was curious to find out what sort of grain his wife wished to measure, and artfully put some suet at the bottom of the measure. Ali Baba's wife went home and set the measure on the heap of gold, and filled it and emptied it often, to her great content. She then carried it back to her sister, without noticing that a piece of gold was sticking to it, which Cassim's wife perceived directly her back was turned.\n\nShe grew very curious, and said to Cassim when he came home, \"Cassim, your brother is richer than you. He does not count his money, he measures it.\"\n\nHe begged her to explain this riddle, which she did by showing him the piece of money and telling him where she found it. Then Cassim grew so envious that he could not sleep, and went to his brother in the morning before sunrise. \"Ali Baba,\" he said, showing him the gold piece, \"you pretend to be poor and yet you measure gold.\"\n\nBy this Ali Baba perceived that through his wife's folly Cassim and his wife knew their secret, so he confessed all and offered Cassim a share.\n\n\"That I expect,\" said Cassim; \"but I must know where to find the treasure, otherwise I will discover all, and you will lose all.\"\n\nAli Baba, more out of kindness than fear, told him of the cave, and the very words to use. Cassim left Ali Baba, meaning to be beforehand with him and get the treasure for himself. He rose early next morning, and set out with ten mules loaded with great chests. He soon found the place, and the door in the rock.\n\nHe said, \"Open, Sesame!\" and the door opened and shut behind him. He could have feasted his eyes all day on the treasures, but he now hastened to gather together as much of it as possible; but when he was ready to go he could not remember what to say for thinking of his great riches. Instead of \"Sesame,\" he said, \"Open, Barley!\" and the door remained fast. He named several different sorts of grain, all but the right one, and the door still stuck fast. He was so frightened at the danger he was in that he had as much forgotten the word as if he had never heard it.\n\nAbout noon the robbers returned to their cave, and saw Cassim's mules roving about with great chests on their backs. This gave them the alarm; they drew their sabers, and went to the door, which opened on their Captain's saying, \"Open, Sesame!\"\n\nCassim, who had heard the trampling of their horses' feet, resolved to sell his life dearly, so when the door opened he leaped out and threw the Captain down. In vain, however, for the robbers with their sabers soon killed him. On entering the cave they saw all the bags laid ready, and could not imagine how anyone had got in without knowing their secret. They cut Cassim's body into four quarters, and nailed them up inside the cave, in order to frighten anyone who should venture in, and went away in search of more treasure.\n\nAs night drew on Cassim's wife grew very uneasy, and ran to her brother-in-law, and told him where her husband had gone. Ali Baba did his best to comfort her, and set out to the forest in search of Cassim. The first thing he saw on entering the cave was his dead brother. Full of horror, he put the body on one of his asses, and bags of gold on the other two, and, covering all with some fagots, returned home. He drove the two asses laden with gold into his own yard, and led the other to Cassim's house.\n\nThe door was opened by the slave Morgiana, whom he knew to be both brave and cunning. Unloading the ass, he said to her, \"This is the body of your master, who has been murdered, but whom we must bury as though he had died in his bed. I will speak with you again, but now tell your mistress I am come.\"\n\nThe wife of Cassim, on learning the fate of her husband, broke out into cries and tears, but Ali Baba offered to take her to live with him and his wife if she would promise to keep his counsel and leave everything to Morgiana; whereupon she agreed, and dried her eyes.\n\nMorgiana, meanwhile, sought an apothecary and asked him for some lozenges. \"My poor master,\" she said, \"can neither eat nor speak, and no one knows what his distemper is.\" She carried home the lozenges and returned next day weeping, and asked for an essence only given to those just about to die.\n\nThus, in the evening, no one was surprised to hear the wretched shrieks and cries of Cassim's wife and Morgiana, telling everyone that Cassim was dead.\n\nThe day after Morgiana went to an old cobbler near the gates of the town who opened his stall early, put a piece of gold in his hand, and bade him follow her with his needle and thread. Having bound his eyes with a handkerchief, she took him to the room where the body lay, pulled off the bandage, and bade him sew the quarters together, after which she covered his eyes again and led him home. Then they buried Cassim, and Morgiana his slave followed him to the grave, weeping and tearing her hair, while Cassim's wife stayed at home uttering lamentable cries. Next day she went to live with Ali Baba, who gave Cassim's shop to his eldest son.\n\nThe Forty Thieves, on their return to the cave, were much astonished to find Cassim's body gone and some of their money-bags.\n\n\"We are certainly discovered,\" said the Captain, \"and shall be undone if we cannot find out who it is that knows our secret. Two men must have known it; we have killed one, we must now find the other. To this end one of you who is bold and artful must go into the city dressed as a traveler, and discover whom we have killed, and whether men talk of the strange manner of his death. If the messenger fails he must lose his life, lest we be betrayed.\"\n\nOne of the thieves started up and offered to do this, and after the rest had highly commended him for his bravery he disguised himself, and happened to enter the town at daybreak, just by Baba Mustapha's stall. The thief bade him good-day, saying, \"Honest man, how can you possibly see to stitch at your age?\"\n\n\"Old as I am,\" replied the cobbler, \"I have very good eyes, and will you believe me when I tell you that I sewed a dead body together in a place where I had less light than I have now.\"\n\nThe robber was overjoyed at his good fortune, and, giving him a piece of gold, desired to be shown the house where he stitched up the dead body. At first Mustapha refused, saying that he had been blindfolded; but when the robber gave him another piece of gold he began to think he might remember the turnings if blindfolded as before. This means succeeded; the robber partly led him, and was partly guided by him, right in front of Cassim's house, the door of which the robber marked with a piece of chalk. Then, well pleased, he bade farewell to Baba Mustapha and returned to the forest. By and by Morgiana, going out, saw the mark the robber had made, quickly guessed that some mischief was brewing, and fetching a piece of chalk marked two or three doors on each side, without saying anything to her master or mistress.\n\nThe thief, meantime, told his comrades of his discovery. The Captain thanked him, and bade him show him the house he had marked. But when they came to it they saw that five or six of the houses were chalked in the same manner. The guide was so confounded that he knew not what answer to make, and when they returned he was at once beheaded for having failed.\n\nAnother robber was dispatched, and, having won over Baba Mustapha, marked the house in red chalk; but Morgiana being again too clever for them, the second messenger was put to death also.\n\nThe Captain now resolved to go himself, but, wiser than the others, he did not mark the house, but looked at it so closely that he could not fail to remember it. He returned, and ordered his men to go into the neighboring villages and buy nineteen mules, and thirty-eight leather jars, all empty except one, which was full of oil. The Captain put one of his men, fully armed, into each, rubbing the outside of the jars with oil from the full vessel. Then the nineteen mules were loaded with thirty-seven robbers in jars, and the jar of oil, and reached the town by dusk.\n\nThe Captain stopped his mules in front of Ali Baba's house, and said to Ali Baba, who was sitting outside for coolness, \"I have brought some oil from a distance to sell at tomorrow's market, but it is now so late that I know not where to pass the night, unless you will do me the favor to take me in.\"\n\nThough Ali Baba had seen the Captain of the robbers in the forest, he did not recognize him in the disguise of an oil merchant. He bade him welcome, opened his gates for the mules to enter, and went to Morgiana to bid her prepare a bed and supper for his guest. He brought the stranger into his hall, and after they had supped went again to speak to Morgiana in the kitchen, while the Captain went into the yard under pretense of seeing after his mules, but really to tell his men what to do.\n\nBeginning at the first jar and ending at the last, he said to each man, \"As soon as I throw some stones from the window of the chamber where I lie, cut the jars open with your knives and come out, and I will be with you in a trice.\"\n\nHe returned to the house, and Morgiana led him to his chamber. She then told Abdallah, her fellow slave, to set on the pot to make some broth for her master, who had gone to bed. Meanwhile her lamp went out, and she had no more oil in the house.\n\n\"Do not be uneasy,\" said Abdallah; \"go into the yard and take some out of one of those jars.\"\n\nMorgiana thanked him for his advice, took the oil pot, and went into the yard. When she came to the first jar the robber inside said softly, \"Is it time?\"\n\nAny other slave but Morgiana, on finding a man in the jar instead of the oil she wanted, would have screamed and made a noise; but she, knowing the danger her master was in, bethought herself of a plan, and answered quietly, \"Not yet, but presently.\"\n\nShe went to all the jars, giving the same answer, till she came to the jar of oil. She now saw that her master, thinking to entertain an oil merchant, had let thirty-eight robbers into his house. She filled her oil pot, went back to the kitchen, and, having lit her lamp, went again to the oil jar and filled a large kettle full of oil. When it boiled she went and poured enough oil into every jar to stifle and kill the robber inside. When this brave deed was done she went back to the kitchen, put out the fire and the lamp, and waited to see what would happen.\n\nIn a quarter of an hour the Captain of the robbers awoke, got up, and opened the window. As all seemed quiet, he threw down some little pebbles which hit the jars. He listened, and as none of his men seemed to stir he grew uneasy, and went down into the yard. On going to the first jar and saying, \"Are you asleep?\" he smelt the hot boiled oil, and knew at once that his plot to murder Ali Baba and his household had been discovered. He found all the gang was dead, and, missing the oil out of the last jar, became aware of the manner of their death. He then forced the lock of a door leading into a garden, and climbing over several walls made his escape. Morgiana heard and saw all this, and, rejoicing at her success, went to bed and fell asleep.\n\nAt daybreak Ali Baba arose, and, seeing the oil jars still there, asked why the merchant had not gone with his mules. Morgiana bade him look in the first jar and see if there was any oil. Seeing a man, he started back in terror. \"Have no fear,\" said Morgiana; \"the man cannot harm you; he is dead.\"\n\nAli Baba, when he had recovered somewhat from his astonishment, asked what had become of the merchant.\n\n\"Merchant!\" said she, \"he is no more a merchant than I am!\" and she told him the whole story, assuring him that it was a plot of the robbers of the forest, of whom only three were left, and that the white and red chalk marks had something to do with it. Ali Baba at once gave Morgiana her freedom, saying that he owed her his life. They then buried the bodies in Ali Baba's garden, while the mules were sold in the market by his slaves.\n\nThe Captain returned to his lonely cave, which seemed frightful to him without his lost companions, and firmly resolved to avenge them by killing Ali Baba. He dressed himself carefully, and went into the town, where he took lodgings in an inn. In the course of a great many journeys to the forest he carried away many rich stuffs and much fine linen, and set up a shop opposite that of Ali Baba's son. He called himself Cogia Hassan, and as he was both civil and well dressed he soon made friends with Ali Baba's son, and through him with Ali Baba, whom he was continually asking to sup with him.\n\nAli Baba, wishing to return his kindness, invited him into his house and received him smiling, thanking him for his kindness to his son.\n\nWhen the merchant was about to take his leave Ali Baba stopped him, saying, \"Where are you going, sir, in such haste? Will you not stay and sup with me?\"\n\nThe merchant refused, saying that he had a reason; and, on Ali Baba's asking him what that was, he replied, \"It is, sir, that I can eat no victuals that have any salt in them.\"\n\n\"If that is all,\" said Ali Baba, \"let me tell you that there shall be no salt in either the meat or the bread that we eat to-night.\"\n\nHe went to give this order to Morgiana, who was much surprised.\n\n\"Who is this man,\" she said, \"who eats no salt with his meat?\"\n\n\"He is an honest man, Morgiana,\" returned her master; \"therefore do as I bid you.\"\n\nBut she could not withstand a desire to see this strange man, so she helped Abdallah to carry up the dishes, and saw in a moment that Cogia Hassan was the robber Captain, and carried a dagger under his garment.\n\n\"I am not surprised,\" she said to herself, \"that this wicked man, who intends to kill my master, will eat no salt with him; but I will hinder his plans.\"\n\nShe sent up the supper by Abdallah, while she made ready for one of the boldest acts that could be thought on. When the dessert had been served, Cogia Hassan was left alone with Ali Baba and his son, whom he thought to make drunk and then to murder them. Morgiana, meanwhile, put on a headdress like a dancing-girl's, and clasped a girdle round her waist, from which hung a dagger with a silver hilt, and said to Abdallah, \"Take your tabor, and let us go and divert our master and his guest.\"\n\nAbdallah took his tabor and played before Morgiana until they came to the door, where Abdallah stopped playing and Morgiana made a low courtesy.\n\n\"Come in, Morgiana,\" said Ali Baba, \"and let Cogia Hassan see what you can do\"; and, turning to Cogia Hassan, he said, \"She's my slave and my housekeeper.\"\n\nCogia Hassan was by no means pleased, for he feared that his chance of killing Ali Baba was gone for the present; but he pretended great eagerness to see Morgiana, and Abdallah began to play and Morgiana to dance. After she had performed several dances she drew her dagger and made passes with it, sometimes pointing it at her own breast, sometimes at her master's, as if it were part of the dance. Suddenly, out of breath, she snatched the tabor from Abdallah with her left hand, and, holding the dagger in her right hand, held out the tabor to her master. Ali Baba and his son put a piece of gold into it, and Cogia Hassan, seeing that she was coming to him, pulled out his purse to make her a present, but while he was putting his hand into it Morgiana plunged the dagger into his heart.\n\n\"Unhappy girl!\" cried Ali Baba and his son, \"what have you done to ruin us?\"\n\n\"It was to preserve you, master, not to ruin you,\" answered Morgiana. \"See here,\" opening the false merchant's garment and showing the dagger; \"see what an enemy you have entertained! Remember, he would eat no salt with you, and what more would you have? Look at him! he is both the false oil merchant and the Captain of the Forty Thieves.\"\n\nAli Baba was so grateful to Morgiana for thus saving his life that he offered her to his son in marriage, who readily consented, and a few days after the wedding was celebrated with greatest splendor.\n\nAt the end of a year Ali Baba, hearing nothing of the two remaining robbers, judged they were dead, and set out to the cave. The door opened on his saying, \"Open Sesame!\" He went in, and saw that nobody had been there since the Captain left it. He brought away as much gold as he could carry, and returned to town. He told his son the secret of the cave, which his son handed down in his turn, so the children and grandchildren of Ali Baba were rich to the end of their lives.";
        _audioClips = await _networkManager.GetAudioClips(story, "Alibaba");

        var textures = _initialData.images360;
        _audioSources = new UnityEngine.AudioSource[textures.Length];
        for (int i = 0; i < textures.Length; i++)
        {
            var texture2D = textures[i];
            var newPos = new Vector3(_image360HolderPrefab.transform.localScale.x * i, 0, 0);
            var holder = Instantiate(_image360HolderPrefab, newPos, Quaternion.identity);
            var mesh = holder.GetComponent<MeshRenderer>();
            mesh.material = Resources.Load<Material>($"Materials/Inverted {i}");
            mesh.material.mainTexture = texture2D;

            var audioSource = holder.GetComponentInChildren<UnityEngine.AudioSource>();
            if (_audioClips != null && i <= _audioClips.Length - 1)
            {
                audioSource.clip = _audioClips[i];
            }

            _audioSources[i] = audioSource;

            var cam = holder.GetComponentInChildren<CinemachineVirtualCamera>();
            cam.Priority = textures.Length - i - 1;
            _image360Holders.Add(holder);
            LoadAllModelGen(holder, _initialData.storyName, i);
        }

        MaxScene = textures.Length - 1;

        EventAggregator.Instance.RaiseEvent(new InitialLoadingFinish());
        IsInitialized = true;
        var audioPlayer = _image360Holders[CurrentScene].GetComponentInChildren<SceneEnity.AudioPlayer>();
        audioPlayer.ForcePlay();
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
        SetEnableController(true);
        CinemachineVirtualCamera cam;
        cam = _image360Holders[CurrentScene].GetComponentInChildren<CinemachineVirtualCamera>();
        var audioPlayer = _image360Holders[CurrentScene].GetComponentInChildren<SceneEnity.AudioPlayer>();
        audioPlayer.ForcedStop();
        if (obj.IsNext && CurrentScene < MaxScene)
        {
            ++CurrentScene;
        }
        else if (!obj.IsNext && CurrentScene > 0)
        {
            --CurrentScene;
        }
        else
        {
            return;
        }
        cam = _image360Holders[CurrentScene].GetComponentInChildren<CinemachineVirtualCamera>();
        audioPlayer = _image360Holders[CurrentScene].GetComponentInChildren<SceneEnity.AudioPlayer>();
        audioPlayer.ForcePlay();
        SetVignetteFov();
        StartCoroutine(DelayMove(cam));
    }

    // Change the Vignette Field of View to 0 in 1 second
    private void SetVignetteFov()
    {
        ;
        DOTween.To(() => _vignette.VignetteFieldOfView, x => _vignette.VignetteFieldOfView = x, -10f, 1f);
    }

    private IEnumerator DelayMove(CinemachineVirtualCamera cam)
    {
        yield return new WaitForSeconds(1.5f);
        transform.DOMove(cam.transform.position, 2f).OnComplete(() =>
        {
            DOTween.To(() => _vignette.VignetteFieldOfView, x => _vignette.VignetteFieldOfView = x, 180f, 1f);
            SetEnableController(false); // Re-enable player control after blend
        });
    }


    // Delay 1 second and then set the Vignette Field of View to 0
    private void SetEnableController(bool isActive)
    {
        _characterController.enabled = !isActive;
        _playerController.enabled = !isActive;
    }


    private void LoadAllModelGen(GameObject parent, string initialDataStoryName, int i)
    {
        if (_environmentSO.environmentLevel != EnvironmentLevel.VRModel3d)
        {
            return;
        }

        GameObject[] objects = Resources.LoadAll<GameObject>($"{initialDataStoryName}/Mesh/{initialDataStoryName}_{i}");

        foreach (var obj in objects)
        {
            var instance = Instantiate(obj, parent.transform, true);
            instance.name = $"{obj.name}";
            InitMesh(instance, i);
            SetCharacterPosition(instance);
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = 0.3f * Vector3.one;
            Debug.Log($"Spawned: {obj.name}");
        }
    }

    private void InitMesh(GameObject instance, int paragraphIndex = 0)
    {
        var realMesh = instance.transform.GetChild(0).gameObject;
        realMesh.tag = "character";
        var collider = realMesh.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size *= 0.7f;
        var audioSource = realMesh.AddComponent<UnityEngine.AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.maxDistance = 2.5f;
        Debug.Log($"{_initialData.storyName}/{paragraphIndex}/{instance.name}");
        var clips = Resources.LoadAll<AudioClip>(
            $"{_initialData.storyName}/{paragraphIndex}/{instance.name}");
        audioSource.clip = clips.FirstOrDefault();
        realMesh.AddComponent<AudioPlayer>();
    }

    private void SetCharacterPosition(GameObject instance)
    {
        float randomX = Mathf.Round(Random.Range(-0.3f, 0.3f) / 0.05f) * 0.05f;
        float randomZ = Mathf.Round(Random.Range(-0.3f, 0.3f) / 0.05f) * 0.05f;

        instance.transform.localPosition = new Vector3(randomX, 0f, randomZ);
    }
}
