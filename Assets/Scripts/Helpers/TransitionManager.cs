using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;



public class TransitionManager : MonoBehaviour
{
    [SerializeField] private GameObject _startingSceneTransition;
    [SerializeField] private GameObject _endingSceneTransition;

    private void Start()
    {
        _startingSceneTransition.SetActive(true);
    }
    private void DisableStartingSceneTransition()
    {
        _startingSceneTransition.SetActive(false);
    }
    public void EnableEndingSceneTransition(string sceneName)
    {
        _endingSceneTransition.SetActive(true);
        if (GameObject.Find("Canvas/Popups") != null) GameObject.Find("Canvas/Popups").SetActive(false);
        StartCoroutine(WaitForEndingSceneTransition(sceneName));
    }
    private void DisableEndingSceneTransition()
    {
        _endingSceneTransition.SetActive(false);
    }
    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
    IEnumerator WaitForEndingSceneTransition(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}   
