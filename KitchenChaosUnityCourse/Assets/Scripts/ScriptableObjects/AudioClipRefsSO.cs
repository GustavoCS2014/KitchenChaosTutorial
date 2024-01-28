using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{
    public AudioClip[] Chop;
    public AudioClip[] DeliveryFail;
    public AudioClip[] DeliverySuccess;
    public AudioClip[] FootStep;
    public AudioClip[] ObjectDrop;
    public AudioClip[] ObjectPickup;
    public AudioClip[] Trash;
    public AudioClip[] Warning;
    public AudioClip StoveSizzle;
}
