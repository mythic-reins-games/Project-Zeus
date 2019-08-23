using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected string _name;
    [SerializeField]
    protected Image _characterImage;
    public string Name { get { return _name; } }
    public Image CharacterImage { get { return _characterImage; } }
}
