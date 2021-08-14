using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlaylist : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource _audioSource;

    [SerializeField] private AudioClip[] playlist;
    private int _index;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        SetMusic();
    }

    private void SetMusic()
    {
        ChooseNextMusic();
        _audioSource.clip = playlist[_index];
        _audioSource.Play();
        StartCoroutine(WaitForEndOfMusic(_audioSource.clip));
    }

    private void ChooseNextMusic()
    {
        int i;
        do i = Random.Range(0, playlist.Length);
        while (i == _index);
        _index = i;
    }

    private IEnumerator WaitForEndOfMusic(AudioClip audioSourceClip)
    {
        
        yield return new WaitForSeconds(audioSourceClip.length);
        SetMusic();
    }
}
