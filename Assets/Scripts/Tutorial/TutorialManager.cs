using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;

public class TutorialManager : MonoBehaviour
{
    public GameObject Popup;
    public List<Texture2D> Images;
    public int CurrentImageIndex = 0;
    int TotalImages=0;



    void Start()
    {
        Time.timeScale = 0;
        Popup = GameObject.Find("Canvas/Popups/TutorialImage");
        Debug.Log(SceneManager.GetActiveScene().name[^1]);
        Images = Resources.LoadAll<Texture2D>("Tutorial/" + SceneManager.GetActiveScene().name[^1]).ToList();
        ToggleOn();

        Popup.GetComponent<Image>().sprite = Sprite.Create(Images[CurrentImageIndex], new Rect(0f, 0f, Images[CurrentImageIndex].width, Images[CurrentImageIndex].height), Vector2.zero);

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) TakeScreenshot();
        //for taking screenshots press enter
        if (Input.GetKeyDown(KeyCode.H)) NextImage();
    }



    public void NextImage()
    {
        if (CurrentImageIndex < Images.Count)
        {
            Popup.GetComponent<Image>().sprite = Sprite.Create(Images[CurrentImageIndex], new Rect(0f, 0f, Images[CurrentImageIndex].width, Images[CurrentImageIndex].height), Vector2.zero);
        }
        else {ToggleOff(); }
        CurrentImageIndex++;
    }
    public void ToggleOn() {Popup.SetActive(true);}
    public void ToggleOff() {Popup.SetActive(false); Time.timeScale = 1;}





    public void TakeScreenshot()
    {
        string filepath = Directory.GetCurrentDirectory() + "/Assets/Resources/Tutorial/" + SceneManager.GetActiveScene().name[^1];
        ScreenCapture.CaptureScreenshot(Path.Combine(filepath, $"Level-Screenshot{TotalImages++}.png"));
    }



}
