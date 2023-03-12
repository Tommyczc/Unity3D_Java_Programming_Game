using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioMixer audioMixer;    // ���п��Ƶ�Mixer����
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

    public void SetMasterVolume(float volume)    // �����������ĺ���
    {
        audioMixer.SetFloat("Master_Volume", volume-30);
        // MasterVolumeΪ���Ǳ�¶������Master�Ĳ���
    }

    public void SetBGMVolume(float volume)    // ���Ʊ������������ĺ���
    {
        audioMixer.SetFloat("BGM_Volume", volume-30);
        // MusicVolumeΪ���Ǳ�¶������Music�Ĳ���
    }

    public void SetUIVolume(float volume)    // ������Ч�����ĺ���
    {
        audioMixer.SetFloat("UI_Volume", volume-30);
        // SoundEffectVolumeΪ���Ǳ�¶������SoundEffect�Ĳ���
    }

}
