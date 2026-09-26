using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[System.Serializable]
public class Animation
{
    public List<AnimationKeyframe> keyframes;
    public bool loop = false;
}
