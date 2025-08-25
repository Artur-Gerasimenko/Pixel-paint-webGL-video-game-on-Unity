using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class ButtonAction : MonoBehaviour
{
    private PixelsActing PixelsController { get; set; }
    public Color MyColour { get; private set; }
    [SerializeField] private Image frame;
    [SerializeField] private Sprite ActiveScroll;
    [SerializeField] private Sprite DisactiveScroll;

    [SerializeField, Range(-202, 202)] private float PowerOfChangeView;

    [SerializeField] private bool isShowColorButton;
    [SerializeField] private bool isOpenSceneButton;
    [SerializeField] private string PathForWindows;
    [SerializeField] private string PathForMobile;

    //[SerializeField,Header("Mark the button")] private UnityEvent MarkingButton;

    [SerializeField,Header("On Disable button")] private UnityEvent action;

    [SerializeField] private UnityEvent _firstAction;
    [SerializeField] private UnityEvent _lastAction;

    private bool _isLast = false;
    private Camera MainCamera { get; set; }
    private Image transformImage { get; set; }
    public bool isMarking { get; private set; }

    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }
    private void Initialization()
    {
        transformImage = GetComponent<Image>();
        MainCamera = Camera.main;
        
        if (GetComponent<AudioSource>())
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = ProjectSettings._volumeSoundTracks;
        }
        if (isShowColorButton)
        {
            PixelsController = GameObject.FindWithTag("GameController")?.GetComponent<PixelsActing>();
            MyColour = transform.GetComponent<Image>().color;
        }
    }
    public void CallTAkeOffAllMarks()
    {
        ButtonSpawn.TakeOffMarks();
    }
    public void ActiveMark(bool active)
    {
        if (GetComponent<Button>().interactable)
        {
            isMarking = isMarking ? false : true;
            frame.gameObject.SetActive(active);
        }
    }
    public void OnClickShowColor()
    {
        PixelsController.isActiveColour = MyColour;
        PixelsController.ShowActivePixelsColor(MyColour);
    }

    public void ChangeView()
    {
        MainCamera.orthographicSize += PowerOfChangeView;
        MainCamera.orthographicSize = Mathf.Clamp(MainCamera.orthographicSize, 10f, 150f);        
    }
    public void SaveProgress()
    {
        PlayerPrefs.SetInt($"{PixelsController.BuilderPixelMap.PixelTexture.name}ButtonColour{MyColour}", 1);
    }
    public void DisactiveButton()
    {
        action.Invoke();
    }
    public void OnClickToBlockScroll()
    {
        switch (PointAction.isMoveDefault)
        {
            case true:
                transformImage.sprite = DisactiveScroll;
                PointAction.isMove = false;
                PointAction.isMoveDefault = false;
                return;
            case false:
                transformImage.sprite = ActiveScroll;
                PointAction.isMove = true;
                PointAction.isMoveDefault = true;
                return;
        }
    }
    public void ActingChange()
    {
        if (!_isLast)
        {
            _firstAction.Invoke();
            _isLast = true;
            return;
        }
        else
        {
            _lastAction.Invoke();
            _isLast = false;
            return;
        }
    }
}
