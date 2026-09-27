using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationsManager : MonoBehaviour
{
    public List<Animation> animations;
    public int currentAnimation = -1;
    public int currentKeyframe = 0;

    float timer = 0f;

    void Update()
    {
        timer -= Time.deltaTime;
        if (currentAnimation > -1 && currentAnimation < animations.Count && timer <= 0)
        {
            if (currentKeyframe < animations[currentAnimation].keyframes.Count)
            {
                foreach (UnityEvent event_ in animations[currentAnimation].keyframes[currentKeyframe].events)
                {
                    event_.Invoke();
                }
                timer = animations[currentAnimation].keyframes[currentKeyframe].duration;
            }
            else if (animations[currentAnimation].loop)
            {
                currentKeyframe = 0;
            }
        }
    }
}
