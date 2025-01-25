using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance{ get; private set; }
    private AudioSource _audio;
    [SerializeField] private AudioClip _upClip;
    [SerializeField] private AudioClip _downClip;
    [SerializeField] private AudioClip _winClip;
    [SerializeField] private AudioClip _loseClip;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _audio = gameObject.AddComponent<AudioSource>();
    }
    public void SoundPlay(int i) 
    {
        switch (i) 
        {
            case 1:
                _audio.clip = _upClip;
                _audio.Play();
                break;
            case 2:
                _audio.clip = _downClip;
                _audio.Play();
                break;
            case 3:
                _audio.clip = _winClip;
                _audio.Play();
                break;
            case 4:
                _audio.clip = _loseClip;
                _audio.Play();
                break;
        }
    }
}
