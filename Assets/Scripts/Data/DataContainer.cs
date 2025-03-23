using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class DataContainer
{
    [JsonProperty("story_name")] 
    public string storyName { get; set; }

    [JsonProperty("paragraphs")] 
    public string[] paragraphs { get; set; }
    
    [JsonIgnore]
    public Texture2D[] images360 => Resources.LoadAll<Texture2D>($"{storyName}/Image360");
    public List<AudioClip> storyAudios { get; set; }
    public List<AudioClip> characterSounds { get; set; }
}
