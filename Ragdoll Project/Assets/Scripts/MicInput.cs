using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicInput : MonoBehaviour
{
    #region SingleTon

    public static MicInput Inctance { set; get; }

    #endregion

    public enum SoundStates {NoState = 0, Wishper, Loud, Scream };

    public static float MicLoudness;
    public static float MicLoudnessinDecibels;
    public SoundStates SoundState;
    public Light[] Lights = null;  //0 active, 1 Wishper, 2 Loud, 3 Scream.


    private string _device;

    public string[] devices;

    public string device_requested;
    //mic initialization
    public void InitMic()
    {
        if (_device == null)
        {
            //foreach(string s in Microphone.devices)
            //{
            //    if (s == device_requested)
            //        _device = s;
            //}
            //if(_device.Length ==0)
                _device = Microphone.devices[0];

            devices = Microphone.devices;
            Debug.Log(_device);
        }
        _clipRecord = Microphone.Start(_device, true, 999, 44100);
        Active = true;
        if (Lights != null)
            Lights[0].color = new Color(0.0f, 255.0f, 0.0f); //Green

    }

    public void StopMicrophone()
    {
        Microphone.End(_device);
        Active = false;
        //if (Lights[0] != null)
        //    Lights[0].color = new Color(255.0f, 0.0f, 0.0f); //Red

    }


    AudioClip _clipRecord;
    AudioClip _recordedClip;
    int _sampleWindow = 128;

    //get data from microphone into audioclip
    float MicrophoneLevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        _clipRecord.GetData(waveData, micPosition);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }

    //get data from microphone into audioclip
    float MicrophoneLevelMaxDecibels()
    {

        float db = 20 * Mathf.Log10(Mathf.Abs(MicLoudness));

        return db;
    }

    public float FloatLinearOfClip(AudioClip clip)
    {
        StopMicrophone();

        _recordedClip = clip;

        float levelMax = 0;
        float[] waveData = new float[_recordedClip.samples];

        _recordedClip.GetData(waveData, 0);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _recordedClip.samples; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }

    public float DecibelsOfClip(AudioClip clip)
    {
        StopMicrophone();

        _recordedClip = clip;

        float levelMax = 0;
        float[] waveData = new float[_recordedClip.samples];

        _recordedClip.GetData(waveData, 0);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _recordedClip.samples; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }

        float db = 20 * Mathf.Log10(Mathf.Abs(levelMax));

        return db;
    }



    void Update()
    {
        // levelMax equals to the highest normalized value power 2, a small number because < 1
        // pass the value to a static var so we can access it from anywhere
        MicLoudness = MicrophoneLevelMax();
        MicLoudnessinDecibels = MicrophoneLevelMaxDecibels();
        if (MicLoudnessinDecibels > -90 && MicLoudnessinDecibels < -70)
        {
            SoundState = SoundStates.Wishper;
            Lights[1].gameObject.SetActive(true);
            Lights[2].gameObject.SetActive(false);
            Lights[3].gameObject.SetActive(false);

        }
        else if (MicLoudnessinDecibels >= -70 && MicLoudnessinDecibels < -40)
        {
            SoundState = SoundStates.Loud;
            Lights[1].gameObject.SetActive(false);
            Lights[2].gameObject.SetActive(true);
            Lights[3].gameObject.SetActive(false);
        }
        else if (MicLoudnessinDecibels >= -40 && MicLoudnessinDecibels < 0)
        {
            SoundState = SoundStates.Scream;
            Lights[1].gameObject.SetActive(false);
            Lights[2].gameObject.SetActive(false);
            Lights[3].gameObject.SetActive(true);
        }
        else
        {
            SoundState = SoundStates.NoState;
            Lights[1].gameObject.SetActive(false);
            Lights[2].gameObject.SetActive(false);
            Lights[3].gameObject.SetActive(false);

        }


        //Debug.Log(MicLoudnessinDecibels);

    }

    public bool Active;
    // start mic when scene starts
    void OnEnable()
    {
        InitMic();
        Active = true;
        Inctance = this;
    }

    //stop mic when loading a new level or quit application
    void OnDisable()
    {
        StopMicrophone();
    }

    void OnDestroy()
    {
        StopMicrophone();
    }


    // make sure the mic gets started & stopped when application gets focused
    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            //Debug.Log("Focus");
            if (!Active)
            {
                InitMic();
            }
        }
        if (!focus)
        {
            //Debug.Log("Pause");
            StopMicrophone();
            //Debug.Log("Stop Mic");

        }
    }
}