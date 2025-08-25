using System.Collections;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioClip[] Tracks;
    private AudioSource MainAudio { get; set; }
    private AudioClip NowTrack { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        MainAudio = GetComponent<AudioSource>();
        StartCoroutine(PlayTrack());
    }

    private IEnumerator PlayTrack()
    {
        while (true)
        {
            NowTrack = Tracks[Random.Range(0, Tracks.Length)];
            MainAudio.PlayOneShot(NowTrack);
            yield return new WaitForSeconds(NowTrack.length);
        }
    }
}
