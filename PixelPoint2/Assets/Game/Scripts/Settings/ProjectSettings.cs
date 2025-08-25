using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectSettings : MonoBehaviour
{
    public static float _volumeSoundTracks { get; private set; }

    void Awake()
    {
        Initialization();
    }
    private void Initialization()
    {
        if (!PlayerPrefs.HasKey("VolumeSound")) PlayerPrefs.SetFloat("VolumeSound", 1f);

        _volumeSoundTracks = PlayerPrefs.GetFloat("VolumeSound");
    }
    public static void ChangeValueVolumeForce(float value)
    {
        _volumeSoundTracks = value;
    }
}
