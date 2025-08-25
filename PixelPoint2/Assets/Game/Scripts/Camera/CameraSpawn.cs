using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpawn : MonoBehaviour
{
    [SerializeField] private MakeSprite MakeSpriteScript;
    private Camera MainCamera { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        MainCamera = Camera.main;
        StartCameraPosition();
    }
    public void StartCameraPosition()
    {
        Vector3 NewCameraPosition = new Vector3(0,0,MainCamera.transform.position.z);
        PixelAction[,] Pixels = MakeSpriteScript.Pixels;
        float MinXPosition = Pixels[0, 0].transform.position.x;
        float MaxXPosition = Pixels[0, 0].transform.position.x;
        float MinYPosition = Pixels[0, 0].transform.position.y;
        float MaxYPosition = Pixels[0, 0].transform.position.x;

        foreach(PixelAction Pixel in Pixels)
        {
            if (Pixel.PixelColor.a >= 1)
            {
                if (Pixel.transform.position.x < MinXPosition) MinXPosition = Pixel.transform.position.x;
                if (Pixel.transform.position.x > MaxXPosition) MaxXPosition = Pixel.transform.position.x;
                if (Pixel.transform.position.y < MinYPosition) MinYPosition = Pixel.transform.position.y;
                if (Pixel.transform.position.y > MaxYPosition) MaxYPosition = Pixel.transform.position.y;
            }
        }
        NewCameraPosition = new Vector3(MaxXPosition - (MaxXPosition - MinXPosition) / 2f, MaxYPosition - (MaxYPosition - MinYPosition) / 2f,NewCameraPosition.z);

        MainCamera.transform.position = NewCameraPosition;
    }
}
