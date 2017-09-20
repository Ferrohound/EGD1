using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AudioTrigger : MonoBehaviour {

    public AudioCommand[] commands;

    bool triggered = false;
    MusicManager manager;

    private void Start()
    {
        manager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            foreach (AudioCommand command in commands)
            {
                switch (command.action)
                {
                    case AudioAction.Play:
                        manager.play(command.stem);
                        break;
                    case AudioAction.Stop:
                        manager.stop(command.stem);
                        break;
                    case AudioAction.FadeIn:
                        manager.fadeIn(command.stem, command.duration);
                        break;
                    case AudioAction.FadeOut:
                        manager.fadeOut(command.stem, command.duration);
                        break;
                    default:
                        manager.stop(command.stem);
                        break;
                }
            }
            triggered = true;
        }
    }
}

public enum AudioAction { Play, Stop, FadeIn, FadeOut };

[System.Serializable]
public class AudioCommand
{
    public int stem;
    public AudioAction action;
    public float duration;
}