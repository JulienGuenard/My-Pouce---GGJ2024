using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public TextMeshProUGUI timer;
    float timerIntActual = 15f;

    public GameObject selectorLockSprite1;
    public GameObject selectorLockSprite2;

    public Player actions;

    public Fade fade;
    public List<Image> Fighters = new List<Image>(3);
    public Image P1Fighter;
    public Image P2Fighter;

    private int p1Selected;
    public int P1Selected
    {
        get => p1Selected;
        set
        {
            p1Selected = value % 3;

            if (value < 0)
                p1Selected = 2;
        }
    }

    private int p2Selected;
    public int P2Selected
    {
        get => p2Selected;
        set
        {
            p2Selected = value % 3;

            if (value < 0)
                p2Selected = 2;
        }
    }

    private int p1Locked;
    private int p2Locked;
    // Start is called before the first frame update
    void Awake()
    {
        fade.Out();
        actions = new Player();

        actions.Pouce.JoystickLeft.performed += P1Input;
        actions.Pouce.JoystickLeft.canceled += P1Input;

        actions.Pouce.JoystickRight.performed += P2Input;
        actions.Pouce.JoystickRight.canceled += P2Input;

        p1Locked = -2;
        p2Locked = -2;
    }

    private void Start()
    {
        P1Fighter.color = Fighters[0].color;
        P2Fighter.color = Fighters[0].color;

        StartCoroutine(StartAfterDelay());
        StartCoroutine(TimerCountdown());
    }

    private void Update()
    {
        if (p1Locked == P1Selected && p2Locked == P2Selected) NextScene();
    }

    bool waitForDrop1 = false;
    public void P1Input(InputAction.CallbackContext context)
    {
        if (p1Locked == -2)
        {
            if (context.ReadValue<Vector2>().y < -.6f && !waitForDrop1)
            {
                waitForDrop1 = true;

                P1Selected++;
                if (P1Selected == p2Locked)
                    P1Selected++;
            }
            if (context.ReadValue<Vector2>().y > .6f && !waitForDrop1)
            {
                waitForDrop1 = true;

                P1Selected--;
                if (P1Selected == p2Locked)
                    P1Selected--;
            }

            if (context.ReadValue<Vector2>().x < -.6f)
            {
                p1Locked = P1Selected;
                selectorLockSprite1.SetActive(true);
            }
                

            P1Fighter.sprite = Fighters[P1Selected].sprite;
            Debug.Log(context.ReadValue<Vector2>().y);

            if (context.canceled)
                waitForDrop1 = false;
        }

        if (context.ReadValue<Vector2>().x > .6f)
            p1Locked = -2;
    }

    bool waitForDrop2 = false;
    public void P2Input(InputAction.CallbackContext context)
    {
        if (p2Locked == -2)
        {
            if (context.ReadValue<Vector2>().y > .6f && !waitForDrop2)
            {
                waitForDrop2 = true;

                P2Selected++;
                if (P2Selected == p1Locked)
                    P2Selected++;
            }
            if (context.ReadValue<Vector2>().y < -.6f && !waitForDrop2)
            {
                waitForDrop2 = true;

                P2Selected--;
                if (P2Selected == p1Locked)
                    P2Selected--;
            }

            if (context.ReadValue<Vector2>().x > .6f)
            {
                p2Locked = P2Selected;
                selectorLockSprite2.SetActive(true);
            }
                

            P2Fighter.sprite = Fighters[P2Selected].sprite;
            Debug.Log(context.ReadValue<Vector2>().y);

            if (context.canceled)
                waitForDrop2 = false;
        }

        if (context.ReadValue<Vector2>().x < -.6f)
            p2Locked = -2;
    }

    private void OnEnable()
    {
        actions.Pouce.Enable();
    }

    private void OnDisable()
    {
        actions.Pouce.JoystickLeft.performed -= P1Input;
        actions.Pouce.JoystickLeft.canceled -= P1Input;
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(15f);
        NextScene();
    }

    void NextScene()
    {
        SceneManager.LoadScene("Thumb Fight");
    }

    IEnumerator TimerCountdown()
    {
        timerIntActual -= 1f;
        timer.text = timerIntActual.ToString();
        yield return new WaitForSeconds(1f);
        if (timerIntActual == 0) NextScene();
        StartCoroutine(TimerCountdown());
    }
}
