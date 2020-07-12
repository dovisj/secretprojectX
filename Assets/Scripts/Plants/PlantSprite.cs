using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game", menuName = "Game/New Plant Sprite")]
public class PlantSprite : ScriptableObject
{
    public Sprite[] frames;
    public Sprite initialFrame;


    public Sprite GetNextFrame(int neededFrame)
    {
        if (neededFrame < frames.Length)
        {
            Sprite frame = frames[neededFrame];
            return frame;
        }
        else
        {
            neededFrame = 0;
            return frames[neededFrame];
        }
    }


}
