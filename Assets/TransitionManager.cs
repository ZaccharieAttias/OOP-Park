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
        StartCoroutine(Wait(1f));
    }
    private void DisableStartingSceneTransition()
    {
        _startingSceneTransition.SetActive(false);
    }
    public void EnableEndingSceneTransition(string sceneName)
    {
        _endingSceneTransition.SetActive(true);
        GameObject.Find("Canvas/Popups").SetActive(false);
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
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}   
