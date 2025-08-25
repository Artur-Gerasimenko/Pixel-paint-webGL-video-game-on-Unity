using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

public class PanelSettings : MonoBehaviour
{
    [SerializeField] private Slider _volumeForce;

    /*[SerializeField] private UnityEvent InputValueSwipeSpeed;
    [SerializeField] private UnityEvent InputValueVolumeForce;*/
    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }
    private void Initialization()
    {
        _volumeForce.value = ProjectSettings._volumeSoundTracks;
    }
    public void OnEnteredVolumeSorce()
    {
        ProjectSettings.ChangeValueVolumeForce(_volumeForce.value);
    }
    public void SaveVolumeValue()
    {
        PlayerPrefs.SetFloat("VolumeSound", _volumeForce.value);
    }
}
