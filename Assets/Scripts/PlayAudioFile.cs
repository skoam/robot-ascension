using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioFile : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource targetAudioSource;

    public bool OneShot = true;
    public bool loop = false;
    public bool disableAfter = false;
    public bool OnEnabled;
    public bool OnStart;
    public bool OnRepeat;
    public bool randomRepeats;
    public bool randomRepeatTime;
    public float repeatPauses = 10;
    public float OnMinDistance;
    public float OnMaxDistance;
    public Transform target;

    public bool randomizeCurrent;

    public float distance;

    private bool done;
    private int currentClip = 0;
    private float currentTimer = 0;
    private float random = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (OnStart)
        {
            Play();
        }
    }

    void OnEnable()
    {
        if (OnEnabled)
        {
            Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OnRepeat)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer > repeatPauses * random)
            {
                currentTimer = 0;
                Play();
            }
        }

        if (target != null)
        {
            if (!done)
            {
                distance = Vector3.Distance(this.transform.position, target.transform.position);

                if (OnMinDistance != 0 && distance < OnMinDistance)
                {
                    Play();
                }

                if (OnMaxDistance != 0 && distance > OnMaxDistance)
                {
                    Play();
                }
            }
        }
    }

    void Play()
    {
        if (randomizeCurrent)
        {
            currentClip = Random.Range(0, clips.Length);
        }

        if (OneShot)
        {
            targetAudioSource.PlayOneShot(clips[currentClip]);
        }
        else
        {
            targetAudioSource.clip = clips[currentClip];
            targetAudioSource.Stop();
            targetAudioSource.Play();
        }

        if (disableAfter)
        {
            this.gameObject.SetActive(false);
        }

        if (loop)
        {
            targetAudioSource.loop = true;
        }
        else
        {
            targetAudioSource.loop = false;
        }

        if (!OnRepeat)
        {
            done = true;
            return;
        }

        if (!randomRepeats)
        {
            currentClip++;
        }
        else
        {
            currentClip = Random.Range(0, clips.Length);
        }

        if (randomRepeatTime)
        {
            random = Random.Range(0.2f, 1.5f);
        }

        if (currentClip > clips.Length - 1)
        {
            currentClip = 0;
        }
    }
}
