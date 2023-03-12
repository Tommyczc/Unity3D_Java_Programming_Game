using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioMixer audioMixer;    // 进行控制的Mixer变量
    private Transform[] father;
    void Start()
    {
        father = GetComponentsInChildren<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
     public void playAudio(string name,float targetVolume) {
        foreach (Transform child in father) {
            if (!child.gameObject.Equals(this.gameObject) && child.gameObject.name==name)
            {
                child.gameObject.GetComponent<AudioSource>().Play();
                StartCoroutine(FadeMusic(child.gameObject.GetComponent<AudioSource>(), 4.0f, targetVolume));
                
            }
        }
     }

    public void turnOff()
    {
        foreach (Transform child in father)
        {
            if (!child.gameObject.Equals(this.gameObject) && child.gameObject.GetComponent<AudioSource>().isPlaying)
            {
                
                StartCoroutine(FadeMusic(child.gameObject.GetComponent<AudioSource>(),2f, 0));
                child.gameObject.GetComponent<AudioSource>().Pause();
            }
        }
    }

    private IEnumerator FadeMusic(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        //float volumeTotal = targetVolume = start;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            //Debug.Log(audioSource.volume);
            yield return null;
        }

        yield break;
    }

    public void SetMasterVolume(float volume)    // 控制主音量的函数
    {
        audioMixer.SetFloat("Master_Volume", volume-30);
        // MasterVolume为我们暴露出来的Master的参数
    }

    public void SetBGMVolume(float volume)    // 控制背景音乐音量的函数
    {
        audioMixer.SetFloat("BGM_Volume", volume-30);
        // MusicVolume为我们暴露出来的Music的参数
    }

    public void SetUIVolume(float volume)    // 控制音效音量的函数
    {
        audioMixer.SetFloat("UI_Volume", volume-30);
        // SoundEffectVolume为我们暴露出来的SoundEffect的参数
    }

}
