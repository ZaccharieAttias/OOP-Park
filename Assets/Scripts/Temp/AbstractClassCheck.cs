using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class AbstractClassCheck : MonoBehaviour
{
    public GameObject ConfirmButton;
    public CharactersManager CharactersManager;
    public List<Character> AbsractClasses;
    public GameObject MessagePopup;
    
    //set the possibility to change the tree after completing the first stage etc

    public void Start()
    {
        CharactersManager = GameObject.Find("Player").GetComponent<CharactersManager>();
        AbsractClasses = new List<Character>();
        foreach (Character character in CharactersManager.CharactersCollection) if (character.IsAbstract) AbsractClasses.Add(character);  
        
        ConfirmButton = GameObject.Find("Canvas/Popups/Abstract/Confirm");  
        ConfirmButton.GetComponent<Button>().onClick.AddListener(() => ConfirmAbstractClasses());

        MessagePopup = GameObject.Find("Canvas/Popups/Abstract/Message");
    }
    public void ConfirmAbstractClasses()
    {
        Dictionary<string, List<Character>> AttributeMap = new Dictionary<string, List<Character>>();
        Dictionary<string, List<Character>> MethodMap = new Dictionary<string, List<Character>>();
        foreach (Character character in AbsractClasses) BuildMaps(ref AttributeMap, ref MethodMap, character);

        List<string> AkeysToRemove = new List<string>();
        foreach (KeyValuePair<string, List<Character>> entry in AttributeMap) if (entry.Value.Count == 1) AkeysToRemove.Add(entry.Key);
        foreach (string key in AkeysToRemove) AttributeMap.Remove(key);

        List<string> MkeysToRemove = new List<string>();
        foreach (KeyValuePair<string, List<Character>> entry in MethodMap) if (entry.Value.Count == 1) MkeysToRemove.Add(entry.Key);
        foreach (string key in MkeysToRemove) MethodMap.Remove(key);
        
        if (AttributeMap.Keys.Count > 0 || MethodMap.Keys.Count > 0)
        {
            MessagePopup.SetActive(true);
            MessagePopup.GetComponentInChildren<TMP_Text>().text = "The following attributes and methods are duplicated: \n";
            foreach (KeyValuePair<string, List<Character>> entry in AttributeMap.ToList())
                MessagePopup.GetComponentInChildren<TMP_Text>().text += entry.Key + " is duplicated in " + entry.Value.Count + " classes \n";
            foreach (KeyValuePair<string, List<Character>> entry in MethodMap.ToList())
                MessagePopup.GetComponentInChildren<TMP_Text>().text += entry.Key + " is duplicated in " + entry.Value.Count + " classes \n";
        }
        else 
        {
            MessagePopup.SetActive(true);
            MessagePopup.GetComponentInChildren<TMP_Text>().text = "All abstract classes are valid";
            ConfirmButton.SetActive(false);
        }
    }
    public void BuildMaps(ref Dictionary<string, List<Character>> AttributeMap, ref Dictionary<string, List<Character>> MethodMap, Character character)
    {
        foreach (Character child in character.Childrens)
            BuildMaps(ref AttributeMap, ref MethodMap, child);

        foreach (CharacterAttribute attribute in character.Attributes) 
        {
            if (!AttributeMap.ContainsKey(attribute.Name)) {
                    AttributeMap.Add(attribute.Name, new List<Character>()); 
                    AttributeMap[attribute.Name].Add(character);
                }
            else 
                AttributeMap[attribute.Name].Add(character);
        }
        foreach (CharacterMethod method in character.Methods) 
        {
            if (!MethodMap.ContainsKey(method.Name)) {
                    MethodMap.Add(method.Name, new List<Character>()); 
                    MethodMap[method.Name].Add(character);
                }
            else 
                MethodMap[method.Name].Add(character);
        }
    }
}
