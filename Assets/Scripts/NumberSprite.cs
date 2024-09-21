using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NumberSprite", menuName = "ScriptableObjects/NumberSprite", order = 1)]
public class NumberSprite : ScriptableObject
{
    public Sprite[] numbers;

    public Sprite this[int index]
    {
        get { return numbers[index]; }
    }

    public Sprite Get(int index) {
        return numbers[index];
    } 
}
