using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimationKeyframe
{
    public List<UnityEvent> events;
    public float duration;
}
