using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance{ get; private set; }
    private AudioSource _audio;
    [SerializeField] private AudioClip _upClip;
    [SerializeField] private AudioClip _downClip;
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

    public void RingUpSound() 
    {
        _audio.clip = _upClip;
        _audio.Play();
    }
    public void RingDownSound()
    {
        _audio.clip = _downClip;
        _audio.Play();
    }
}
