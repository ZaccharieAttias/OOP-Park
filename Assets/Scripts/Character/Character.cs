using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class Character
{
    public string name;
    public string description;

    public List<Character> parents;
    public List<Character> childrens;
    public List<CharacterAttribute> attributes;
    public List<CharacterMethod> methods;

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

    private void PreDetails(List<Character> parents)
    {
        foreach (Character character in parents)
        {
            foreach (CharacterAttribute attribute in character.attributes)
                if (attribute.accessModifier != AccessModifier.Private)
                    if (!(this.attributes.Any(item => item.name == attribute.name)))
                        this.attributes.Add(new CharacterAttribute(attribute.name, attribute.description, attribute.accessModifier));

            foreach (CharacterMethod method in character.methods)
                if (method.accessModifier != AccessModifier.Private)
                    if (!(this.methods.Any(item => item.name == method.name)))
                        this.methods.Add(new CharacterMethod(method.name, method.description, method.accessModifier));

            this.depth = Math.Max(this.depth, character.depth + 1);
        }
    }

    public void InitializeCharacter(Character character)
    {
        this.name = character.name;
        this.description = character.description;

        this.parents = character.parents;
        this.childrens = character.childrens;
        this.attributes = character.attributes;
        this.methods = character.methods;

        this.depth = character.depth;
    }
}
