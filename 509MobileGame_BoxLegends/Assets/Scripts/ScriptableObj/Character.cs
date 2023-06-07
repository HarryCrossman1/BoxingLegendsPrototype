using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="NewCharacter",menuName ="Character")]
public class Character : ScriptableObject

{
    public Texture FlagImage, CharacterImage;
    public string CharacterName;
    public string CharacterBio;

    public float Strength;
    public float Dodge;
    public float accuracy;
    public float defense;
    public float stamina;

}