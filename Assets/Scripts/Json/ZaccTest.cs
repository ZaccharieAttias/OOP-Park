using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class ZaccTest : Singleton<ZaccTest> {
    Dictionary<string, CharacterAttribute> Attributes = new Dictionary<string, CharacterAttribute>();
    Dictionary<string, CharacterMethod> Methods = new Dictionary<string, CharacterMethod>();
    // Dictionary<string, SpecialAbility> SpecialAbilities = new Dictionary<string, SpecialAbility>();
    Dictionary<string, Character> Characters = new Dictionary<string, Character>();

    Dictionary<string, AccessModifier> AccessModifiersOfTheObjects = new Dictionary<string, AccessModifier>();
    Dictionary<string, CharacterMethod> AttributesOfTheMethod = new Dictionary<string, CharacterMethod>();

    Dictionary<string, List<Character>> CharacterChildren = new Dictionary<string, List<Character>>();
    Dictionary<string, List<Character>> CharacterParents = new Dictionary<string, List<Character>>();
    Dictionary<string, CharacterAttribute> AttributesOfTheCharacter = new Dictionary<string, CharacterAttribute>();
    Dictionary<string, CharacterMethod> MethodsOfTheCharacter = new Dictionary<string, CharacterMethod>();
    // Dictionary<string, SpecialAbility> SpecialAbilitiesOfTheCharacter = new Dictionary<string, SpecialAbility>();
    // Dictionary<Character, CharacterMethod> CharracterUpcastMethod = new Dictionary<Character, CharacterMethod>();

    

    public string FolderPath;

    public void Start() {
        FolderPath = Path.Combine(Application.dataPath, "Resources/Json", SceneManager.GetActiveScene().name);

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) Save();
        // if (Input.GetKeyDown(KeyCode.L)) Load();
    }

    private void InitAttributes()
    {
        List<CharacterAttribute> attributes = new List<CharacterAttribute>(GameObject.Find("Canvas/Popups").GetComponent<AttributesManager>().AttributesCollection);
        foreach (CharacterAttribute attribute in attributes)
            if (!Attributes.ContainsKey(attribute.Name))
                Attributes.Add(attribute.Name, attribute);
    }
    private void InitMethods()
    {
        List<CharacterMethod> methods = new List<CharacterMethod> (GameObject.Find("Canvas/Popups").GetComponent<MethodsManager>().MethodsCollection);
        foreach (CharacterMethod method in methods)
            if (!Methods.ContainsKey(method.Name))
                Methods.Add(method.Name, method);
    }
    // private void InitSpecialAbilities()
    // {
    //     Dictionary<SpecialAbility, List<CharacterSpecialAbility>> specialAbilities = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilityManager>().SpecialAbilitiesCollection;
    //     foreach (KeyValuePair<SpecialAbility, List<CharacterSpecialAbility>> specialAbility in specialAbilities)
    //         if (!SpecialAbilities.ContainsKey(specialAbility.Key.Name))
    //             SpecialAbilities.Add(specialAbility.Key.Name, specialAbility.Key);
    // }
    private void InitCharacters()
    {
        List<Character> characters = GameObject.Find("Player").GetComponent<CharacterManager>().CharactersCollection;
        foreach (Character character in characters)
            if (!Characters.ContainsKey(character.Name))
                Characters.Add(character.Name, character);
    }
    private void InitReferences()
    {
        //save the references of the AccessModifiersOfTheObjects
        foreach (CharacterMethod method in Methods.Values)
            if (!AccessModifiersOfTheObjects.ContainsKey(method.AccessModifier.ToString()))
                AccessModifiersOfTheObjects.Add(method.AccessModifier.ToString(), method.AccessModifier);

        foreach (CharacterAttribute attribute in Attributes.Values)
            if (!AccessModifiersOfTheObjects.ContainsKey(attribute.AccessModifier.ToString()))
                AccessModifiersOfTheObjects.Add(attribute.AccessModifier.ToString(), attribute.AccessModifier);

        //save the references of the AttributesOfTheMethod
        foreach (CharacterMethod method in Methods.Values)
            if (!AttributesOfTheMethod.ContainsKey(method.Attribute.Name))
                AttributesOfTheMethod.Add(method.Attribute.Name, method);

        //save the references of the CharacterChildren
        foreach (Character character in Characters.Values)
            foreach (Character child in character.Childrens)
                if (!CharacterChildren.ContainsKey(character.Name))
                    CharacterChildren.Add(character.Name, new List<Character> { child });
                else
                    CharacterChildren[character.Name].Add(child);
        
        //save the references of the CharacterParents
        foreach (Character character in Characters.Values)
            foreach (Character parent in character.Parents)
                if (!CharacterParents.ContainsKey(character.Name))
                    CharacterParents.Add(character.Name, new List<Character> { parent });
                else
                    CharacterParents[character.Name].Add(parent);

        //save the references of the AttributesOfTheCharacter
        foreach (Character character in Characters.Values)
            foreach (CharacterAttribute attribute in character.Attributes)
                if (!AttributesOfTheCharacter.ContainsKey(character.Name))
                    AttributesOfTheCharacter.Add(character.Name, attribute);
        
        //save the references of the MethodsOfTheCharacter
        foreach (Character character in Characters.Values)
            foreach (CharacterMethod method in character.Methods)
                if (!MethodsOfTheCharacter.ContainsKey(character.Name))
                    MethodsOfTheCharacter.Add(character.Name, method);

        //save the references of the SpecialAbilitiesOfTheCharacter
        // foreach (Character character in Characters.Values)
        //     if (!SpecialAbilitiesOfTheCharacter.ContainsKey(character.SpecialAbility.Name))
        //         SpecialAbilitiesOfTheCharacter.Add(character.SpecialAbility.Name, character.SpecialAbility);
        
        //save the references of the CharracterUpcastMethod
        // foreach (Character character in Characters.Values)
        //     if (!CharracterUpcastMethod.ContainsKey(character))
        //         CharracterUpcastMethod.Add(character, character.UpcastMethod.CharacterMethod);
    }
    public void Save()
    {
        InitAttributes();
        InitMethods();
        // InitSpecialAbilities();
        InitCharacters();

        InitReferences();



        List<AttributeData> attributeData = new List<AttributeData>();
        foreach (CharacterAttribute attribute in Attributes.Values)
        {
            AttributeData data = new AttributeData();
            data.key = attribute.Name;
            data.attributes = new List<AttributeInfo>
            {
                new AttributeInfo(attribute.Name, attribute.Description, attribute.Value, attribute.AccessModifier.ToString())
            };
            attributeData.Add(data);
        }
        FileHandler.SaveToJSON<AttributeData>(attributeData , Path.Combine(FolderPath, "Attributes.json"));

        List<MethodData> methodData = new List<MethodData>();
        foreach (CharacterMethod method in Methods.Values)
        {
            MethodData data = new MethodData();
            data.key = method.Name;
            data.methods = new List<MethodInfo>
            {
                new MethodInfo(method.Name, method.Description, method.AccessModifier.ToString(), method.Attribute.Name)
            };
            methodData.Add(data);
        }
        FileHandler.SaveToJSON<MethodData>(methodData , Path.Combine(FolderPath, "Methods.json"));

        //List<SpecialAbilityData> specialAbilityData = new List<SpecialAbilityData>();
        // foreach (SpecialAbility specialAbility in SpecialAbilities.Values)
        // {
        //     SpecialAbilityData data = new SpecialAbilityData();
        //     data.key = specialAbility.Name;
        //     data.specialAbilities = new List<SpecialAbilityInfo>
        //     {
        //         new SpecialAbilityInfo(specialAbility.Name, specialAbility.Description, specialAbility.Value, specialAbility.Type.Name)
        //     };
        //     specialAbilityData.Add(data);
        // }
        // json = JsonUtility.ToJson(specialAbilityData, true);
        // File.WriteAllText(Path.Combine(FolderPath, "SpecialAbilities.json"), json);

        List<CharacterData> characterData = new List<CharacterData>();
        foreach (Character character in Characters.Values)
        {
            CharacterData data = new CharacterData();
            data.key = character.Name;
            data.characters = new List<CharacterInfo>
            {
                new CharacterInfo(character.Name, character.Description, new List<string>(), new List<string>(), character.SpecialAbility.Name, character.IsOriginal, character.IsAbstract, new List<AttributeInfo>(), new List<MethodInfo>())
            };
            characterData.Add(data);
        }
        FileHandler.SaveToJSON<CharacterData>(characterData , Path.Combine(FolderPath, "Characters.json"));

        List<AccessModifierData> accessModifierData = new List<AccessModifierData>();
        foreach (AccessModifier accessModifier in AccessModifiersOfTheObjects.Values)
        {
            AccessModifierData data = new AccessModifierData();
            data.key = accessModifier.ToString();
            data.accessModifiers = new List<AccessModifierInfo>
            {
                new AccessModifierInfo(accessModifier.ToString())
            };
            accessModifierData.Add(data);
        }
        FileHandler.SaveToJSON<AccessModifierData>(accessModifierData , Path.Combine(FolderPath, "AccessModifiers.json"));

        List<AttributeData> attributeDataOfTheMethod = new List<AttributeData>();
        foreach (CharacterMethod method in AttributesOfTheMethod.Values)
        {
            AttributeData data = new AttributeData();
            data.key = method.Name;
            data.attributes = new List<AttributeInfo>
            {
                new AttributeInfo(method.Attribute.Name, method.Attribute.Description, method.Attribute.Value, method.Attribute.AccessModifier.ToString())
            };
            attributeDataOfTheMethod.Add(data);
        }
        FileHandler.SaveToJSON<AttributeData>(attributeDataOfTheMethod , Path.Combine(FolderPath, "AttributesOfTheMethod.json"));

        List<CharacterData> characterChildren = new List<CharacterData>();
        foreach (KeyValuePair<string, List<Character>> character in CharacterChildren)
        {
            CharacterData data = new CharacterData();
            data.key = character.Key;
            data.characters = new List<CharacterInfo>();
            foreach (Character child in character.Value)
                data.characters.Add(new CharacterInfo(child.Name, child.Description, new List<string>(), new List<string>(), child.SpecialAbility.Name, child.IsOriginal, child.IsAbstract, new List<AttributeInfo>(), new List<MethodInfo>()));
            characterChildren.Add(data);
        }
        FileHandler.SaveToJSON<CharacterData>(characterChildren , Path.Combine(FolderPath, "CharacterChildren.json"));

        List<CharacterData> characterParents = new List<CharacterData>();
        foreach (KeyValuePair<string, List<Character>> character in CharacterParents)
        {
            CharacterData data = new CharacterData();
            data.key = character.Key;
            data.characters = new List<CharacterInfo>();
            foreach (Character parent in character.Value)
                data.characters.Add(new CharacterInfo(parent.Name, parent.Description, new List<string>(), new List<string>(), parent.SpecialAbility.Name, parent.IsOriginal, parent.IsAbstract, new List<AttributeInfo>(), new List<MethodInfo>()));
            characterParents.Add(data);
        }
        FileHandler.SaveToJSON<CharacterData>(characterParents , Path.Combine(FolderPath, "CharacterParents.json"));

        //save the references of the Attributes for each Character
        List<AttributeData> attributesOfTheCharacter = new List<AttributeData>();
        foreach (KeyValuePair<string, CharacterAttribute> attribute in AttributesOfTheCharacter)
        {
            AttributeData data = new AttributeData();
            //key is the character name
            data.key = attribute.Key;
            data.attributes = new List<AttributeInfo>
            {
                new AttributeInfo(attribute.Value.Name, attribute.Value.Description, attribute.Value.Value, attribute.Value.AccessModifier.ToString())
            };
            attributesOfTheCharacter.Add(data);
        }
        FileHandler.SaveToJSON<AttributeData>(attributesOfTheCharacter , Path.Combine(FolderPath, "AttributesOfTheCharacter.json"));

        //save the references of the Methods for each Character
        List<MethodData> methodsOfTheCharacter = new List<MethodData>();
        foreach (KeyValuePair<string, CharacterMethod> method in MethodsOfTheCharacter)
        {
            MethodData data = new MethodData();
            data.key = method.Key;
            data.methods = new List<MethodInfo>
            {
                new MethodInfo(method.Value.Name, method.Value.Description, method.Value.AccessModifier.ToString(), method.Value.Attribute.Name)
            };
            methodsOfTheCharacter.Add(data);
        }
        FileHandler.SaveToJSON<MethodData>(methodsOfTheCharacter , Path.Combine(FolderPath, "MethodsOfTheCharacter.json"));


    }


}






















[Serializable]
public class AttributeData
{
    public string key;
    public List<AttributeInfo> attributes = new List<AttributeInfo>();
}

[Serializable]
public class AttributeInfo
{
    public string Name;
    public string description;
    public float value;
    public string accessModifier;

    public AttributeInfo(string Name, string description, float value, string accessModifier)
    {
        this.Name = Name;
        this.description = description;
        this.value = value;
        this.accessModifier = accessModifier;
    }
}

[Serializable]
public class MethodData
{
    public string key;
    public List<MethodInfo> methods = new List<MethodInfo>();
}

[Serializable]
public class MethodInfo
{
    public string Name;
    public string description;
    public string accessModifier;
    public string attribute;

    public MethodInfo(string Name, string description, string accessModifier, string attribute)
    {
        this.Name = Name;
        this.description = description;
        this.accessModifier = accessModifier;
        this.attribute = attribute;
    }
}

// [Serializable]
// public class SpecialAbilityData
// {
//     public string key;
//     public List<SpecialAbilityInfo> specialAbilities = new List<SpecialAbilityInfo>();
// }

// [Serializable]
// public class SpecialAbilityInfo
// {
//     public string Name;
//     public string description;
//     public float value;
//     public string type;

//     public SpecialAbilityInfo(string Name, string description, float value, string type)
//     {
//         this.Name = Name;
//         this.description = description;
//         this.value = value;
//         this.type = type;
//     }
// }

[Serializable]
public class CharacterData
{
    public string key;
    public List<CharacterInfo> characters = new List<CharacterInfo>();
}

[Serializable]
public class CharacterInfo
{
    public string Name;
    public string description;
    public List<string> parents;
    public List<string> children;
    public string specialAbility;
    public bool isOriginal;
    public bool isAbstract;

    public List<AttributeInfo> attributes;
    public List<MethodInfo> methods;
    // public List<SpecialAbilityInfo> specialAbilities;

    public CharacterInfo(string Name, string description, List<string> parents, List<string> children, string specialAbility, bool isOriginal, bool isAbstract, List<AttributeInfo> attributes, List<MethodInfo> methods)
    {
        this.Name = Name;
        this.description = description;
        this.parents = new List<string>(parents);
        this.children = new List<string>(children);
        this.specialAbility = specialAbility;
        this.isOriginal = isOriginal;
        this.isAbstract = isAbstract;
        this.attributes = new List<AttributeInfo>(attributes);
        this.methods = new List<MethodInfo>(methods);
        // this.specialAbilities = new List<SpecialAbilityInfo>(specialAbilities);
        // this.upcastMethod = upcastMethod;
    }
}

[Serializable]
public class AccessModifierData
{
    public string key;
    public List<AccessModifierInfo> accessModifiers = new List<AccessModifierInfo>();
}

[Serializable]
public class AccessModifierInfo
{
    public string Name;

    public AccessModifierInfo(string Name)
    {
        this.Name = Name;
    }
}

