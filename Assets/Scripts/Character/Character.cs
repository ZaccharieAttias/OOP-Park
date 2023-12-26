using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;
using System;


[System.Serializable]
public class Character : MonoBehaviour
{
    public string name;
    public List<Character> ancestors;
    public List<Character> childrens;
    public List<CharacterAttribute> attributes;
    public List<CharacterMethod> methods;
    public string description;
    public int depth = 0;


    public Character()
    {
        ancestors = new List<Character>();
        childrens = new List<Character>();
        attributes = new List<CharacterAttribute>();
        methods = new List<CharacterMethod>();
    }

    public Character(string name, string description, List<Character> ancestors)
    {
        this.name = name;
        this.description = description;
        this.attributes = new List<CharacterAttribute>();
        this.methods = new List<CharacterMethod>();
        this.childrens = new List<Character>();




        Debug.Log("need help here motherfisher");
        this.ancestors = ancestors;
        PreDetails();
        SetChildrens();
    }

    public void InitializeCharacter(Character character)
    {
        this.name = character.name;
        this.description = character.description;
        this.attributes = character.attributes;
        this.methods = character.methods;
        this.childrens = character.childrens;
        this.ancestors = character.ancestors;
        this.depth = character.depth;
    }


    private void PreDetails()
    {
        foreach (Character character in this.ancestors)
        {
            if (character.attributes != null)
            {
                foreach (CharacterAttribute attribute in character.attributes)
                {
                    CharacterAttribute newAttribute = new CharacterAttribute(attribute.name, attribute.description, attribute.accessModifier);
                    if (!(attribute.accessModifier == AccessModifier.Private))
                    {
                        if (!this.attributes.Any(a => a.name == attribute.name))
                        {
                            this.attributes.Add(newAttribute);
                        }
                    }
                }
            }

            if (character.methods != null)
            {
                foreach (CharacterMethod method in character.methods)
                {
                    CharacterMethod newMethod = new CharacterMethod(method.name, method.description, method.accessModifier);
                    if (!(method.accessModifier == AccessModifier.Private))
                    {
                        if (!this.methods.Any(m => m.name == method.name))
                        {
                            this.methods.Add(newMethod);
                        }
                    }
                }
            }
            this.depth = Math.Max(this.depth, character.depth + 1);
        }
    }

    private void SetChildrens()
    {
        foreach (Character character in this.ancestors)
        {
            Debug.Log("zaaaaaaaaaa");
            character.childrens.Add(this);
        }
    }
}