using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

[HelpURL("https://www.youtube.com/watch?v=6_uAAzbv4IE&t=1522s")]
public class PointAction : MonoBehaviour
{
    // Start is called before the first frame 
    [SerializeField,Tooltip("Speed changed veiw of camera")] private float WheelSpeed;

    [Header("Parametrs"),Range(0,500f)]
    [SerializeField] private float MaxDistance;

    [Range(0,500f)]
    [SerializeField] private float MinDistance;
    private float DistanceStartReactionMovementCamera { get; set; }
    private bool IsMobilePlatform { get { return Application.isMobilePlatform; } }

    private PixelsActing PixelsController;

    private Vector3 StartScrollingPosition { get; set; }

    private Camera MainCamera;
    public static bool isMove { get; set; } = true;
    public static bool isMoveDefault { get; set; } = true;

    private float StartDistanceBeetewenTouches { get; set; }
    private float MaxDistancePositionX { get; set; }
    private float MinDistancePositionX { get; set; }
    private float MaxDistancePositionY { get; set; }
    private float MinDistancePositionY { get; set; }

    public static Action onTouchDownMob, onMouseButtonDown;
    public static Action onInputUp;


    private Touch _touch;

    [SerializeField,Header("PictureBuilder")] private MakeSprite SpriteBuildController;

    public void SwipingApproximation()
    {
        if (isMove)
        {
            if (Input.touchCount == 1)
            {
                _touch = Input.GetTouch(0);

                if (_touch.phase == TouchPhase.Moved)
                {
                    Vector3 movePos = new Vector3(
                        transform.position.x + -_touch.deltaPosition.x * Time.deltaTime,
                        transform.position.y + -_touch.deltaPosition.y * Time.deltaTime,
                        -1f);

                    transform.position = movePos;
                }
            }
            else if (Input.touchCount > 1)
            {
                float NowDistanceBeetwenTouches = Vector3.Distance(
                        Input.touches[0].position,
                        Input.touches[Input.touches.Length - 1].position);
                if (Input.touches[Input.touches.Length - 1].phase == TouchPhase.Began)
                {
                    StartDistanceBeetewenTouches = NowDistanceBeetwenTouches;
                }
                if (Input.touches[Input.touches.Length - 1].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Moved)
                {
                    float NewCameraVeiwSize = StartDistanceBeetewenTouches / NowDistanceBeetwenTouches;
                    MainCamera.orthographicSize *= NewCameraVeiwSize;
                    MainCamera.orthographicSize = Mathf.Clamp(MainCamera.orthographicSize, MinDistance, MaxDistance);
                    StartDistanceBeetewenTouches = NowDistanceBeetwenTouches;
                }
            }
        }
    }

    public void GiveDefaultValue()
    {
        isMove = isMoveDefault;
    }
    public void Initialization()
    {
        MainCamera = Camera.main;
        DistanceStartReactionMovementCamera = MainCamera.orthographicSize / 30;
        PixelsController = SpriteBuildController.gameObject.GetComponent<PixelsActing>();
        var ArrayPixels = PixelsController.BuilderPixelMap.Pixels;

        MaxDistancePositionX = ArrayPixels[0, 0].transform.position.x;
        MaxDistancePositionY = ArrayPixels[0, 0].transform.position.y;
        MinDistancePositionX = ArrayPixels[0, 0].transform.position.x;
        MinDistancePositionY = ArrayPixels[0, 0].transform.position.y;

        foreach (var Pixel in ArrayPixels)
        {
            MaxDistancePositionX = Pixel.transform.position.x > MaxDistancePositionX ? Pixel.transform.position.x : MaxDistancePositionX;
            MaxDistancePositionY = Pixel.transform.position.y > MaxDistancePositionY ? Pixel.transform.position.y : MaxDistancePositionY;

            MinDistancePositionX = Pixel.transform.position.x < MinDistancePositionX ? Pixel.transform.position.x : MinDistancePositionX;
            MinDistancePositionY = Pixel.transform.position.y < MinDistancePositionY ? Pixel.transform.position.y : MinDistancePositionY;
        }
        
    }
    public void WheelScrolling()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");
        if (mw > 0.1 && MainCamera.orthographicSize > MinDistance)
        {
            MainCamera.orthographicSize -= MainCamera.orthographicSize * Time.deltaTime * WheelSpeed;/*Приближение*/
        }
        if (mw < -0.1 && MainCamera.orthographicSize < MaxDistance)
        {
            MainCamera.orthographicSize += MainCamera.orthographicSize * Time.deltaTime * WheelSpeed;/*Отдаление*/
        }

    }
    public void ClickAction()
    {
        if (IsMobilePlatform && Input.touchCount > 0)
        {
            Vector2 ray = MainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);

            PointActions(ray);
        }
        else if(!IsMobilePlatform && Input.GetMouseButton(0))
        {
            Vector2 CurMousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);

            PointActions(CurMousePos);
        }
    }
    public void IsMovingOnPointerUIOverWin()
    {
        bool state = EventSystem.current.IsPointerOverGameObject();
        if (state)
        {
            isMove = false;
        }

    }
    public void IsMovingOnPointerUIOverMob()
    {
        bool state = EventSystem.current.IsPointerOverGameObject();

        if (state)
        {
            isMove = false;
        }
    }

    public void MouseMovement()
    {
        if (isMove) 
        {
             Vector3 NowPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                StartScrollingPosition = NowPosition;
            }
            
            else if (Input.GetMouseButton(0) && Vector3.Distance(StartScrollingPosition, NowPosition) > DistanceStartReactionMovementCamera)
            {
                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x - (NowPosition.x - StartScrollingPosition.x), MinDistancePositionX - 10f, MaxDistancePositionX + 10f),
                    Mathf.Clamp(transform.position.y - (NowPosition.y - StartScrollingPosition.y), MinDistancePositionY - 10f, MaxDistancePositionY + 10f),
                    -1f);
            }
        }
    }
    public void DefautlValuePointerScrollMove()
    {
        isMove = isMoveDefault;
    }
    private void PointActions(Vector2 PointPosition)
    {

        RaycastHit2D rayHit = Physics2D.Raycast(PointPosition, Vector2.zero);

        if (!rayHit.collider) return;

        if (rayHit.transform != null && rayHit.collider.gameObject.GetComponent<SpriteRenderer>().color == PixelsController.ColorAllocate)
        {
            rayHit.collider.gameObject.GetComponent<PixelAction>().Painting(click: true);

            MakeSprite.ProgressInPercent += MakeSprite.PercentInPixel;
            int IndexX = (int)rayHit.transform.position.x;
            int IndexY = (int)rayHit.transform.position.y;


            //Saveing the progress
            PlayerPrefs.SetInt($"{SpriteBuildController.PixelTexture.name}:{IndexX},{IndexY}", 1);
            PlayerPrefs.SetInt($"{SpriteBuildController.PixelTexture.name}PercentProgress", (int)MakeSprite.ProgressInPercent);

            if (MakeSprite.remainderNotPaintPixels == 0)
            {
                ButtonAction button = ButtonSpawn.GiveButtonColor(PixelsController.isActiveColour).GetComponent<ButtonAction>();
                button.SaveProgress();
                button.DisactiveButton();
            }
            if (PixelsController.FillPixels() && !GameController.isEndGame)
            {
                PlayerPrefs.SetInt($"{GameController.SaveKey}EndGame", 1);
                GameController.isEndGame = true;
                PixelsController.GetComponent<GameController>()._finishGameAction.Invoke();
            }
        }
    }
    private void OnDrawGizmos()
    {

        Gizmos.DrawSphere(this.transform.localPosition, 2f);
    }
}
