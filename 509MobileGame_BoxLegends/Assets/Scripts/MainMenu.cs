using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private Button Exib, Career, Settings, Quit;
    private Text ErrorMessage;
    void Start()
    { 
        Career = GameObject.Find("StartCareer").GetComponent<Button>();
        Exib = GameObject.Find("StartExbition").GetComponent<Button>();
        Settings = GameObject.Find("Settings").GetComponent<Button>();
        Quit = GameObject.Find("Quit").GetComponent<Button>();
        ErrorMessage = GameObject.Find("ErrorMessage").GetComponent<Text>();
    }
    public void CareerSettingsOnClick(string String)
    {
        ErrorMessage.text = "There is no " + String + " in a demo ! ";
        ErrorMessage.enabled = true;
        StartCoroutine(Textfade(3));
    }
    private IEnumerator Textfade(float seconds)
    { 
        yield return new WaitForSeconds(seconds);
        ErrorMessage.enabled = false;
    }
    public void ExibOnClick()
    {
        SceneManager.LoadScene("CharacterSelect");
    }
    public void QuitOnClick()
    {
        Application.Quit();
    }
    public void CareerStart()
    {
        CareerSettingsOnClick("career mode");
    }
    public void SettingsStart()
    {
        CareerSettingsOnClick("settings");
    }

}
