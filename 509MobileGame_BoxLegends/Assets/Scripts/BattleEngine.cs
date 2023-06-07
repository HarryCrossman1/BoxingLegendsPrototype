
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class BattleEngine : MonoBehaviour
{
    [Header("Canvas")]
     private Canvas AttacksMain;
     private Canvas AttacksHead;
     private Canvas Attacksbody;

    [Header("DialogueBox")]
    private Image dialoguebackground;
    private Text DialogueText;

    [SerializeField] private float Strengthp1, Dodgep1, Accuracyp1, Defensep1, Staminap1, Strengthp2, Dodgep2, Accuracyp2, Defensep2, Staminap2;

    private Text RoundNumber;
    private Text RoundTimer;
    [SerializeField] private float RoundNumberFloat;
    [SerializeField] private float RoundTimerFloat;

    [SerializeField] private float PlayerHealth = 100f;
    [SerializeField] private float PlayerStamina = 100f;
    [SerializeField] private float EnemyHealth = 100f;
    [SerializeField] private float EnemyStamina = 100f;

    private float totalAttackDamage = 0;
    private float totalStaminaCost = 0;
    private float totalstaminadrain = 0;

    private Slider PlayerHp, EnemyHp, Playerstam, EnemyStam;


    [SerializeField] private Toggle[] toggles;
    private List<int> activeToggles = new List<int>();
    private Button ConfirmButton,HeadbodySwitch;
    private Toggle SpecialAttack;
    private bool ButtonIsClicked, AttackSucc, P1Turn;
    private Dictionary<int, (int, int, int)> toggleValues = new Dictionary<int, (int, int, int)>();
    private string TrueOrFalse;

    //Ko data 

    private Text KoNumber;
    private Slider KoSlider;
    private Image KoSliderImageOne, KoSliderImageTwo;
    [SerializeField] private float Knockouttimer;
    [SerializeField] private float Magnitude;
    [SerializeField] private int AmountKnockdownP1, AmountKnockdownP2;
    private Text WinnersText;
    private bool GameContinues;
    private float waittimer =3f;



    private enum GameStates {Start,Choosing,PlayerDamage,PlayerTwoStart,PlayerTwoChoose,PlayerTwoDamage,Nextstage}
    [SerializeField]private GameStates CurrentState;

    private void Awake()
    {
            Strengthp1=PlayerPrefs.GetFloat("Strength_p1");
            Dodgep1=PlayerPrefs.GetFloat("Dodge_p1");
            Accuracyp1=PlayerPrefs.GetFloat("Accuracy_p1");
            Defensep1=PlayerPrefs.GetFloat("Defense_p1");
            Staminap1=PlayerPrefs.GetFloat("Stamina_p1");
            Strengthp2=PlayerPrefs.GetFloat("Strength_p2");
            Dodgep2=PlayerPrefs.GetFloat("Dodge_p2");
            Accuracyp2=PlayerPrefs.GetFloat("Accuracy_p2");
            Defensep2=PlayerPrefs.GetFloat("Defense_p2");
            Staminap2=PlayerPrefs.GetFloat("Stamina_p2");
    }
    void Start()
    {
        //Setting Up The Referances 
        AttacksMain = GameObject.Find("Attacks").GetComponent<Canvas>();
        AttacksHead = GameObject.Find("AttacksHead").GetComponent<Canvas>();
        Attacksbody = GameObject.Find("AttacksBody").GetComponent<Canvas>();

        dialoguebackground = GameObject.Find("TextBackground").GetComponent<Image>();
        DialogueText = GameObject.Find("DialogueText").GetComponent<Text>();

        RoundNumber = GameObject.Find("RoundNumber").GetComponent<Text>();
        RoundTimer = GameObject.Find("RoundTimer").GetComponent<Text>();

        PlayerHp = GameObject.Find("PlayerHealth").GetComponent<Slider>();
        EnemyHp = GameObject.Find("OpponentHealth").GetComponent<Slider>();
        Playerstam = GameObject.Find("PlayerStamina").GetComponent<Slider>();
        EnemyStam = GameObject.Find("OpponentStamina").GetComponent<Slider>();

        ConfirmButton = GameObject.Find("ConfirmAttack").GetComponent<Button>();
        HeadbodySwitch = GameObject.Find("HeadBodySwitch").GetComponent<Button>();
        SpecialAttack = GameObject.Find("SpecialAttack").GetComponent<Toggle>();

        KoNumber = GameObject.Find("GetUpFromKoTimer").GetComponent<Text>();
        KoSlider = GameObject.Find("GetUpFromKoSlider").GetComponent<Slider>();
        KoSliderImageOne = GameObject.Find("KoSliderImageOne").GetComponent<Image>();
        KoSliderImageTwo = GameObject.Find("KoSliderImageTwo").GetComponent<Image>();

        WinnersText = GameObject.Find("WinnerText").GetComponent<Text>();

        ButtonIsClicked = false;
        CurrentState = GameStates.Start;
        RoundNumberFloat = 1f;
        RoundTimerFloat = 180f;
        Attacksbody.enabled = false;

        KoSlider.enabled = false;
        KoSliderImageOne.enabled = false;
        KoSliderImageTwo.enabled = false;
        KoNumber.enabled = false;
        GameContinues = true;


        
        for (int i = 0; i < toggles.Length; i++)
        {
            int toggleIndex = i;
            toggles[i].onValueChanged.AddListener(delegate { OnToggleValueChanged(toggleIndex, toggles[toggleIndex].isOn); });
        }
        ConfirmButton.onClick.AddListener(OnButtonClicked);
    }

    void Update()
    {
        switch (CurrentState)
        {
            case GameStates.Start:
                {
                    KoNumber.enabled = false;
                    KoSlider.enabled = false;
                    KoSliderImageOne.enabled = false;
                    KoSliderImageTwo.enabled = false;

                    DialogueController(true);
                    AttacksMain.enabled = false;
                    TextController.instance.Changetext("Player One Choose Your Attack",30);
                    if (Input.GetMouseButtonDown(0))
                    {
                        AttacksMain.enabled = true;
                        ChangeStates(GameStates.Choosing);
                    }
                break;
                }
            case GameStates.Choosing:
                {
                    P1Turn = true;

                    if (ButtonIsClicked == true)
                    {
                        PlayerUpdate(true);
                        ChangeStates(GameStates.PlayerDamage);
                    }
                break;
                }
            case GameStates.PlayerDamage:
                {
                    waittimer -= Time.deltaTime;
                    if (PlayerStamina <= 0)
                    {
                        TextController.instance.Changetext("No Stamina Left ! ", 25);
                    }
                    else
                    {
                        TextController.instance.Changetext(TrueOrFalse + "     Damage = " + totalAttackDamage + "   Stamina Cost = " + totalStaminaCost + "   Stamina Drain = " + totalstaminadrain, 14);
                    }
                        ButtonIsClicked = false;
                        AttacksMain.enabled = false;
                    ToggleLowButtons(false);
                    if (waittimer <= 0)
                    {
                        exceedslimits(false);
                        ClearStats();
                        if (GameContinues == true)
                        {
                            ChangeStates(GameStates.PlayerTwoStart);
                        }
                    }
                }
                    break;
            case GameStates.PlayerTwoStart:
                {
                    waittimer = 3f;
                    KoNumber.enabled = false;
                    KoSlider.enabled = false;
                    KoSliderImageOne.enabled = false;
                    KoSliderImageTwo.enabled = false;
                    ToggleLowButtons(true);

                    P1Turn = false;
                    AttacksMain.enabled = false;
                    TextController.instance.Changetext("Player Two Choose Your Attack",30);
                    if (Input.GetMouseButtonDown(0))
                    {
                        AttacksMain.enabled = true;
                        ChangeStates(GameStates.PlayerTwoChoose);
                    }
                    break;
                }
            case GameStates.PlayerTwoChoose:
                {
                    if (ButtonIsClicked == true)
                    {
                        ChangeStates(GameStates.PlayerTwoDamage);
                        PlayerUpdate(false);
                        exceedslimits(true);
                    }
                    break;
                }
                case GameStates.PlayerTwoDamage:
                {
                    waittimer -= Time.deltaTime;

                    if (EnemyStamina <= 0)
                    {
                        TextController.instance.Changetext("No Stamina Left ! ", 25);
                    }
                    else
                    {
                        TextController.instance.Changetext(TrueOrFalse + "     Damage = " + totalAttackDamage + "   Stamina Cost = " + totalStaminaCost + "   Stamina Drain = " + totalstaminadrain, 14);
                    }
                   

                    ButtonIsClicked = false;
                    AttacksMain.enabled = false;
                    ToggleLowButtons(false);

                    if (waittimer <= 0)
                    {
                       
                        exceedslimits(true);
                        ClearStats();
                        if (GameContinues == true)
                        {
                            ChangeStates(GameStates.Nextstage);
                        }
                    }
                    
                    break;
                }
                case GameStates.Nextstage:
                {
                    waittimer = 3f;
                    ToggleLowButtons(true);
                    if (RoundTimerFloat == 0f)
                    {
                        RoundNumberFloat++;
                        RoundTimerFloat = 180;
                        //
                        if (PlayerStamina <= 0)
                        {
                            PlayerStamina = 0;
                        }
                        PlayerStamina += Staminap1*15;

                        if (EnemyStamina <= 0)
                        { 
                        EnemyStamina= 0;
                        }
                        EnemyStamina += Staminap2*15;
                        //
                        PlayerHealth += 25;
                        EnemyHealth += 25; 
                    }
                    ChangeStates(GameStates.Start);
                    RoundTimerFloat -= 30;
                    break;
                }
            default:
                break;
        }
        RoundNumber.text = RoundNumberFloat.ToString();
        RoundTimer.text = RoundTimerFloat.ToString();
        Convert();
    }
    void OnToggleValueChanged(int toggleIndex, bool value)
    {
        //Values for each different attack
        toggleValues[0] = (1, 1,0);//jab head
        toggleValues[1] = (4, 9,1);//backhand head 
        toggleValues[2] = (6, 15,1);//hook head
        toggleValues[3] = (11, 23,2);//Uppercut Head
        toggleValues[4] = (1, 3,3);//Jab Body
        toggleValues[5] = (2, 6,4);//Backhand Body
        toggleValues[6] = (9, 20,12);//Hook Body
        toggleValues[7] = (12, 15,6);//Uppercut Body
        toggleValues[8] = (15, 33,15);//SpecialAttack

        // debug.log the values to console
        Debug.Log("Toggle " + toggleIndex + ": Attack damage = " + toggleValues[toggleIndex].Item1 + ", Stamina cost = " + toggleValues[toggleIndex].Item2 + ", Stamina Drained + " + toggleValues[toggleIndex].Item3);
    }
    private void ToggleLowButtons(bool Istrue)
    {
        if (Istrue == true)
        {
            ConfirmButton.gameObject.SetActive(true);
            SpecialAttack.gameObject.SetActive(true);
            HeadbodySwitch.gameObject.SetActive(true);
        }
        else
        {
            ConfirmButton.gameObject.SetActive(false);
            SpecialAttack.gameObject.SetActive(false);
            HeadbodySwitch.gameObject.SetActive(false);
        }
    }
   public void OnButtonClicked()
    {
        if (P1Turn == true)
        {
            float RandomChance = Random.Range(0, (Dodgep2 +2));
            print(AttackSucc);
            print("random chance :"+RandomChance);
            if (RandomChance < Accuracyp1)
            {
                AttackSucc = true;
                TrueOrFalse = "Punches Landed!";
            }
            else
            {
                AttackSucc = false;
                TrueOrFalse = "Punches Missed!";
            }
            ButtonIsClicked = true;

            foreach (var toggle in toggleValues)
            {
                if (toggles[toggle.Key].isOn)
                {
                    if (AttackSucc == true)
                    {
                        totalAttackDamage += toggle.Value.Item1 * Strengthp1 / Defensep2;
                        totalstaminadrain += toggle.Value.Item3;
                    }
                    totalStaminaCost += toggle.Value.Item2 / Staminap1;
                }
            }
            // debug the total attack damage and stamina to the console
            Debug.Log("Total attack damage: " + totalAttackDamage);
            Debug.Log("Total stamina cost: " + totalStaminaCost);
            Debug.Log("Total stamina Drain " + totalstaminadrain);
        }
        else
        {
            float RandomChance = Random.Range(0,(Dodgep1 +2));
            print("random chance :"+RandomChance);
            if (RandomChance < Accuracyp2)
            {
                AttackSucc = true;
                TrueOrFalse = "Punches Landed!";
            }
            else
            {
                AttackSucc = false;
                TrueOrFalse = "Punches Missed!";
            }
            ButtonIsClicked = true;
            print(AttackSucc);

            foreach (var toggle in toggleValues)
            {
                if (toggles[toggle.Key].isOn)
                {
                    if (AttackSucc == true)
                    {
                        totalAttackDamage += toggle.Value.Item1 * Strengthp2 / Defensep1;
                        totalstaminadrain += toggle.Value.Item3;
                    }
                    totalStaminaCost += toggle.Value.Item2/ Staminap2;
                }
            }
            // p2
            Debug.Log("Total attack damage: " + totalAttackDamage);
            Debug.Log("Total stamina cost: " + totalStaminaCost);
            Debug.Log("Total stamina Drain " + totalstaminadrain);
        }
    }
    public void DialogueController(bool TrueOrFalse)
    {
        if (TrueOrFalse == true)
        {
            dialoguebackground.enabled = true;
            DialogueText.enabled = true;
        }
        else
        {
            dialoguebackground.enabled = false;
            DialogueText.enabled = false;
        }
    }
    public void BodyHeadswitch()
    {
        if (AttacksHead.enabled == true)
        {
            AttacksHead.enabled = false;
            Attacksbody.enabled = true;
        }
        else
        {
            AttacksHead.enabled = true;
            Attacksbody.enabled = false;
        }
    }
    void ChangeStates(GameStates States)
    {
        CurrentState = States;
    }
    void Convert()
    {
        PlayerHp.value = PlayerHealth;
        Playerstam.value = PlayerStamina;
        EnemyHp.value = EnemyHealth;
        EnemyStam.value = EnemyStamina;
    }
    private void PlayerUpdate(bool P1OrP2)
    {
        if (P1OrP2 == true)
        {
            PlayerStamina -= totalStaminaCost;
            if (PlayerStamina >= 0)
            {
                EnemyHealth -= totalAttackDamage;
               
                EnemyStamina -= totalstaminadrain;
            }
        }
        else
        {
            EnemyStamina -= totalStaminaCost;
            if (EnemyStamina >= 0)
            {
                PlayerHealth -= totalAttackDamage;
                
                PlayerStamina -= totalstaminadrain;
            }
        }
    }
    void ClearStats()
    {
        totalAttackDamage = 0;
        totalStaminaCost = 0;
        totalstaminadrain = 0;
    }
    private void exceedslimits(bool P1OrP2)
    {
        if (PlayerStamina >= 100)
            PlayerStamina = 100;
        //if(PlayerStamina<=0)
        //    PlayerStamina= 0;

        if(PlayerHealth >= 100)
            PlayerHealth = 100;

        if(EnemyStamina >= 100)
            EnemyStamina = 100;
        //if(EnemyStamina <= 0)
        //    EnemyStamina = 0;
        if (EnemyHealth >= 100)
            EnemyHealth = 100;

        ///

        if (P1OrP2 == true)
        {
            Debug.Log(Input.acceleration.magnitude);
            if (PlayerHealth <= 0)
            {
                GameContinues = false;
                Knockouttimer -= Time.deltaTime;

                KoNumber.enabled = true;
                KoSlider.enabled = true;
                KoSliderImageOne.enabled = true;
                KoSliderImageTwo.enabled = true;

                KoNumber.text = Knockouttimer.ToString();

                if (Input.acceleration.magnitude >= 0.1)
                {

                    KoSlider.value = Magnitude;
                    Magnitude += Input.acceleration.magnitude /(20 * AmountKnockdownP1) ;

                    if (Magnitude >= 100)
                    {
                        PlayerHealth = 30f;
                        Knockouttimer = 10f;
                        Magnitude = 0f;
                        AmountKnockdownP1++;
                        GameContinues = true;
                    }
                }
                if (Knockouttimer <= 0)
                {
                    WinnersText.text = "Player 2 WINS ";
                    WinnersText.enabled = true;
                    StartCoroutine(EndGame(5));
                    Debug.Log("You Lost");
                    GameContinues = false;
                }
            }
        }




        if (P1OrP2 == false)
        {
            
            Debug.Log(Input.acceleration.magnitude);
            if (EnemyHealth <= 0)
            {
                GameContinues = false;
                Knockouttimer -= Time.deltaTime;
                KoNumber.text = Knockouttimer.ToString();
                KoNumber.enabled = true;
                KoSlider.enabled = true;
                KoSliderImageOne.enabled = true;
                KoSliderImageTwo.enabled = true;

                if (Input.acceleration.magnitude >= 0.1)
                {
                    KoSlider.value = Magnitude;
                    Magnitude += Input.acceleration.magnitude /(20 * AmountKnockdownP2) ;

                    if (Magnitude >= 100)
                    {
                        EnemyHealth = 30f;
                        Knockouttimer = 10f;
                        Magnitude = 0f;
                        AmountKnockdownP2++;
                        GameContinues = true;
                    }

                }
                if (Knockouttimer <= 0)
                {
                    Debug.Log("P2 Lost");
                    WinnersText.text = "Player 1 WINS ";
                    WinnersText.enabled = true;
                    StartCoroutine(EndGame(5));
                    GameContinues = false;
                }
            }
        }
    }
    private IEnumerator EndGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Application.Quit();
    }
}
