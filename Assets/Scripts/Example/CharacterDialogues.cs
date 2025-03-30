using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Network;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CharacterDialogues : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string paragraph = @"Ali Baba was in the forest getting firewood.\n\nHe heard a clatter of hooves on the path. He was afraid. He climbed the nearest tree and hid.\n\nHe peeked out and saw forty men on horseback.\n\nAli Baba in the forest\n\nThe forty riders reached a cliff. In the bright sunlight Ali Baba could see that their saddlebags were full of glistening and sparkling gold.\n\n“Those bags are full of stolen treasure!” he thought.\n\nThen the leader cried, “Open Sesame!”\n\nSuddenly, a great big secret door in the rock opened. The forty riders rode inside.\n\nWhen they were all inside, the leader shouted, “Close Sesame!”\n\nThe secret door closed.\n\nAli Baba waited, still hiding in his treetop. Then the riders all came out of the cave.\n\nThe leader shouted, “Close Sesame!”\n\nThe secret door closed.\n\nAli Baba peered through the leaves.\n\nThe thieves put empty saddlebags over their horses and set off.\n\nAli Baba listened until they were gone";
    [SerializeField] private NetworkManager _networkManager;
    
    private AudioSource audioSource;
    private Stopwatch _timer;
    private async void Start()
    {
        audioSource = GetComponent<AudioSource>();
        string[] dialogues = await _networkManager.GetCharacterDialogues(paragraph, "Alibaba");
        Debug.Log("dialogues get: " + string.Join(',', dialogues));
        foreach (var dialogue in dialogues)
        {
            _timer = Stopwatch.StartNew();
            var clip = await _networkManager.GetAudioClip(dialogue, "en");
            Debug.Log($"Loaded {dialogue} audio success in : {_timer.Elapsed.TotalSeconds} seconds.}}");
            _timer.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
    
}
