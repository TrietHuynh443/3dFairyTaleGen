using System;
using System.IO;
using UnityEngine;

public static class WavUtility
{
    /// <summary>
    /// Converts an AudioClip to a WAV file in byte array format.
    /// </summary>
    public static byte[] ToWav(AudioClip clip)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            WriteWav(clip, stream);
            return stream.ToArray();
        }
    }

    /// <summary>
    /// Saves an AudioClip as a WAV file to the specified path.
    /// </summary>
    public static void SaveWav(AudioClip clip, string filePath)
    {
        byte[] wavData = ToWav(clip);
        File.WriteAllBytes(filePath, wavData);
        Debug.Log($"WAV file saved at: {filePath}");
    }

    /// <summary>
    /// Writes WAV file header and data.
    /// </summary>
    private static void WriteWav(AudioClip clip, Stream stream)
    {
        int sampleRate = clip.frequency;
        int channels = clip.channels;
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            // WAV Header
            writer.Write(new char[] { 'R', 'I', 'F', 'F' });
            writer.Write(36 + samples.Length * 2);
            writer.Write(new char[] { 'W', 'A', 'V', 'E' });
            writer.Write(new char[] { 'f', 'm', 't', ' ' });
            writer.Write(16);
            writer.Write((short)1);
            writer.Write((short)channels);
            writer.Write(sampleRate);
            writer.Write(sampleRate * channels * 2);
            writer.Write((short)(channels * 2));
            writer.Write((short)16);
            writer.Write(new char[] { 'd', 'a', 't', 'a' });
            writer.Write(samples.Length * 2);

            // WAV Data (PCM 16-bit)
            foreach (float sample in samples)
            {
                short value = (short)(Mathf.Clamp(sample, -1f, 1f) * short.MaxValue);
                writer.Write(value);
            }
        }
    }
}
