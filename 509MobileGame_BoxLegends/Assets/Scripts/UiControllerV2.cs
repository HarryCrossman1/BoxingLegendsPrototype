using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiControllerV2 : MonoBehaviour
{
    public Character[] character;

    private Text Stats;
    private Text CharacterName;
    private Text Bios;

    public static int ImageIndex = 0;
    [SerializeField] private int imageindexforinspector;
     private RawImage CharactersImageContainer,FlagImageContainer;
    [SerializeField] private Character mycharacter;
     private Button P1Button, P2button,StartButton;


     private enum SelectStates {Start,P1Confirmed,P2Confirmed }
     private SelectStates CurrrentConfirmState;

    private void Start()
    {
        CurrrentConfirmState = SelectStates.Start;

        P1Button = GameObject.Find("SelectP1").GetComponent<Button>();
        P2button = GameObject.Find("SelectP2").GetComponent<Button>();
        StartButton = GameObject.Find("Start").GetComponent<Button>();

        Stats = GameObject.Find("CharacterStats").GetComponent<Text>();
        CharacterName = GameObject.Find("CharacterName").GetComponent<Text>();
        Bios = GameObject.Find("CharacterBio").GetComponent<Text>();

        CharactersImageContainer = GameObject.Find("CharacterImage").GetComponent<RawImage>();
        FlagImageContainer = GameObject.Find("CountryFlag").GetComponent<RawImage>();
    }
    void Update()
    {
        UpdateCharacterChanges();
        Selection();
        imageindexforinspector = ImageIndex;
    }

    private void UpdateCharacterChanges()
    {
        mycharacter = character[ImageIndex];


        CharactersImageContainer.texture = mycharacter.CharacterImage;
        FlagImageContainer.texture = mycharacter.FlagImage;
        Bios.text = mycharacter.CharacterBio;
        CharacterName.text = mycharacter.CharacterName;
        Stats.text = ("Strength :" + mycharacter.Strength + "\nDodge :" + mycharacter.Dodge + "\nAccuracy :" + mycharacter.accuracy + "\nDefense :" + mycharacter.defense + "\nStamina:" + mycharacter.stamina);
    }

    //Buttons On Click 
    public void Forward()
    {
        if (ImageIndex == character.Length)
        {
            Debug.Log("No Further");
        }
        else
        {
            ImageIndex++;
        }
    }

    public void Back() 
    {
        if (ImageIndex <=0)
        {
            Debug.Log("No Further");
        }
        else
        {
            ImageIndex--;
        }
    }
    public void ConfirmP1()
    {
            PlayerPrefs.SetFloat("Strength_p1", mycharacter.Strength);
            PlayerPrefs.SetFloat("Dodge_p1", mycharacter.Dodge);
            PlayerPrefs.SetFloat("Accuracy_p1", mycharacter.accuracy);
            PlayerPrefs.SetFloat("Defense_p1", mycharacter.defense);
            PlayerPrefs.SetFloat("Stamina_p1", mycharacter.stamina);
            CurrrentConfirmState = SelectStates.P1Confirmed;
    }
    public void ConfirmP2()
    {
        PlayerPrefs.SetFloat("Strength_p2", mycharacter.Strength);
        PlayerPrefs.SetFloat("Dodge_p2", mycharacter.Dodge);
        PlayerPrefs.SetFloat("Accuracy_p2", mycharacter.accuracy);
        PlayerPrefs.SetFloat("Defense_p2", mycharacter.defense);
        PlayerPrefs.SetFloat("Stamina_p2", mycharacter.stamina);
        CurrrentConfirmState = SelectStates.P2Confirmed;
    }
    public void StartGameButton()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void BackToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Selection()
    {
        switch (CurrrentConfirmState)
        {
            case SelectStates.Start :
                {
                    P2button.gameObject.SetActive(false);     
                    StartButton.gameObject.SetActive(false);
                    break;
                }
            case SelectStates.P1Confirmed:
                {
                    P2button.gameObject.SetActive(true);
                    P1Button.gameObject.SetActive(false);
                    break;
                }
            case SelectStates.P2Confirmed:
                {
                    P2button.gameObject.SetActive(false);
                    P1Button.gameObject.SetActive(false);
                    StartButton.gameObject.SetActive(true);
                    break;
                }
        }
    }
}
