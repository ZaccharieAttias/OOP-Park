using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.EventSystems;
using System.Collections;


[System.Serializable]
public class Character
{
    public string name;
    public List<Character> parents;
    public List<Character> childrens;
    public List<CharacterAttribute> attributes;
    public List<CharacterMethod> methods;
    public string description;
    public int depth;


    public Character(string name, string description, List<Character> parents)
    {
        this.name = name;
        this.description = description;

        this.parents = new List<Character>();
        this.childrens = new List<Character>();
        this.attributes = new List<CharacterAttribute>();
        this.methods = new List<CharacterMethod>();

        this.depth = 0;

        PreDetails(parents);
    }

    public void InitializeCharacter(Character character)
    {
        this.name = character.name;
        this.description = character.description;
        this.attributes = character.attributes;
        this.methods = character.methods;
        this.childrens = character.childrens;
        this.parents = character.parents;
        this.depth = character.depth;
    }

    private void PreDetails(List<Character> parents)
    {
        for (int i = 0; i < parents.Count; i++)
        {
            Character character = parents[i];
            if (character.attributes != null)
            {
                for (int j = 0; j < character.attributes.Count; j++)
                {
                    CharacterAttribute attribute = character.attributes[j];
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
                for (int j = 0; j < character.methods.Count; j++)
                {
                    CharacterMethod method = character.methods[j];
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
}
