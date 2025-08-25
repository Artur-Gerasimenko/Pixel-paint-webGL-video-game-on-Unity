 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonSpawn : MonoBehaviour
{
    [SerializeField] private Button Button;
    public MakeSprite MapAction { get; private set; }

    private static Button[] _Buttons;

    private float DistanceBetweenButtons 
    {    get
         {
            return GetComponent<RectTransform>().rect.height * 1.5f;
         }     
    }
    // Start is called before the first frame update
    void Start()
    {
        MapAction = GameObject.FindWithTag("GameController").GetComponent<MakeSprite>();
        AddColorButtons();
    }
    private float _baseScale;
    private float _baseDistance;


    public static Button GiveButtonColor(Color buttonColor)
    {
        foreach (Button button in _Buttons)
        {
            if(button.GetComponent<ButtonAction>().MyColour == buttonColor)
            {
                return button;
            }
        }
        return null;
    }
    public static void TakeOffMarks()
    {
        foreach (Button button in _Buttons)
        {
            if (button.interactable)
            {
                button.GetComponent<ButtonAction>().ActiveMark(false);
            }
        }
    }
    public void AddColorButtons()
    {
        _Buttons = new Button[MapAction.CountColours.Length];
        for(int Index = 0;Index < _Buttons.Length; Index++)
        {
            _Buttons[Index] = Instantiate(Button, transform.position, Quaternion.identity);

            _Buttons[Index].GetComponent<Image>().color = MapAction.CountColours[Index];

            _Buttons[Index].GetComponentInChildren<TextMeshProUGUI>().text = MapAction.FaceNumbers[Index].ToString();

            if (PlayerPrefs.HasKey($"{MapAction.PixelTexture.name}ButtonColour{MapAction.CountColours[Index]}")
                && PlayerPrefs.GetInt($"{MapAction.PixelTexture.name}ButtonColour{MapAction.CountColours[Index]}") >= 1)
                _Buttons[Index].GetComponent<ButtonAction>().DisactiveButton();

            _Buttons[Index].transform.SetParent(transform);
        }
        transform.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        transform.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
}
