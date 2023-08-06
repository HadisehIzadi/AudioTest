using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E7.Native;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	public AudioClip clip;
	public AudioSource unitySource;
	private NativeAudioPointer pointer;
	
	void Awake()
	{
		
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void simpleSoundPlay()
    {
    	unitySource.PlayOneShot(clip);
    }
    
    public void nativeSoundPlay()
    {
    	NativeAudio.Initialize();

        pointer = NativeAudio.Load(clip);
    	NativeAudio.GetNativeSourceAuto().Play(pointer);
    }
}
