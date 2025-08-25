using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MakeSprite : MonoBehaviour
{
    [SerializeField] private Texture2D _PixelTexture;
    public Texture2D PixelTexture { get; private set; }
    public int SizeOnePixelWidth;
    public int SizeOnePixelHeigth;
    public int[] FaceNumbers { get; private set; }
    public PixelAction Pixel;
    public int Width;
    public int Height;
    public Color[] PixelsColour { get; private set; } = new Color[default];
    public PixelAction[,] Pixels { get; private set; }
    public Color[] CountColours { get; private set; } = new Color[default];
    public bool[] ColorActing { get; private set; }
    public ButtonSpawn ScrollView { get; private set; } 

    private float PaintinPixelsOfStart { get; set; }

    public static float PercentInPixel { get; private set; }

    public static Single ProgressInPercent { get; set; }
    public static int remainderNotPaintPixels { get; set; }



    void Awake()
    {
        PixelTexture = _PixelTexture;
        FulledNumbersArray();
        Initialization();
        CreateColourArray();
        Pixels = new PixelAction[Width, Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Pixels[x, y] = Pixel;
            }
        }
        FillColorArray();
        ShowPixelSprite();
        CountedColors();
    }
    private void Start()
    {
        PercentInPixel = 100f / PixelsColour.Length;
        ProgressInPercent = PaintinPixelsOfStart * PercentInPixel;
    }
    private void FulledNumbersArray()
    {
        FaceNumbers = new int[1000];
        for (int Index = default; Index < FaceNumbers.Length; Index++) { FaceNumbers[Index] = Index; }
    }
    private void CountedColors()
    {

        Color[] colours = new Color[default];
        
            for (int Index = 0; Index < PixelsColour.Length; Index++)
            {
                if(PixelsColour[Index].a >= 1 && !BooleanValueArrayColor(PixelsColour[Index], colours))
                {
                    CountColours = new Color[colours.Length];
                    FulledArray(CountColours, colours);
                    colours = new Color[colours.Length + 1];
                    FulledArray(colours, CountColours);
                    colours[colours.Length - 1] = PixelsColour[Index];
                }
            }
        
        CountColours = colours;
        ColorActing = new bool[CountColours.Length];
    }
    private void FulledArray(Color[] First_array, Color[] Second_array)
    {
        int length = Math.Min(First_array.Length, Second_array.Length);

        for(int i = 0;i < length; i++)
        {
            First_array[i] = Second_array[i];
        }
    }
    private void Initialization()
    {
        ScrollView = GameObject.FindGameObjectWithTag("ScrollView").GetComponent<ButtonSpawn>();
    }
    private void CreateColourArray()
    {
        int ArrayLength = default;
        for (int x = 0; x < PixelTexture.width; x++)
        {
            int PositionX = x * SizeOnePixelWidth;
            for (int y = 0; y < PixelTexture.height; y++)
            {
                int PositionY = y * SizeOnePixelHeigth;

                Color PixelColor = PixelTexture.GetPixel(PositionX, PositionY);
                if(PixelColor.a >= 1 && ReturnNewColor(PixelColor))
                {
                    ArrayLength++;
                }
            }
        }
        PixelsColour = new Color[ArrayLength];
    }
    private void FillColorArray()
    {
        for (int x = 0, IndexColor = 0; x < Pixels.GetLength(0); x++)
        {
            int CoordinateX = x * SizeOnePixelWidth;

            for (int y = 0; y < Pixels.GetLength(1); y++)
            {
                int CoordinateY = y * SizeOnePixelHeigth;

                Color colorPixel = PixelTexture.GetPixel(CoordinateX, CoordinateY);

                if (colorPixel.a >= 1 && ReturnNewColor(colorPixel))
                {
                    PixelsColour[IndexColor] = colorPixel;
                    IndexColor++;
                }
            }
        }

    }
    private bool ReturnNewColor(Color color)
    {
        for (int i = 0; i < PixelsColour.Length; i++)
        {
            if (color == PixelsColour[i]) return false;
        }
        return true;
    }
    private int IndexArrayColor(Color color, Color[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (color == array[i]) return i;
        }
        return default;
    }
    private bool BooleanValueArrayColor(Color color,Color[] ColorArray)
    {
        for (int Index = 0; Index < ColorArray.Length; Index++)
        {
            if (color == ColorArray[Index]) return true;
        }
        return false;
    }
    public void ShowPixelSprite()
    {
        int sceneActiveNumber = GameController.GetIndexScene;
        Vector3 PositionPixel = new Vector3(0, 0, 0);
        for (int x = 0; x < Pixels.GetLength(0); x++)
        {
            int CoordinateX = x * SizeOnePixelWidth;
            for (int y = 0;y < Pixels.GetLength(1); y++)
            {
                int CoordinateY = y * SizeOnePixelHeigth;
                PositionPixel = new Vector3(x * (Pixel.transform.localScale.x * 4), y * (Pixel.transform.localScale.y * 4), 0); 

                if (PixelTexture.GetPixel(CoordinateX, CoordinateY).a >= 1) Pixels[x, y] = Instantiate(Pixel, PositionPixel, Quaternion.identity);
                else Pixels[x, y].isPainted = true;

                Pixels[x, y].gameObject.GetComponentInChildren<TextMesh>().text = FaceNumbers[IndexArrayColor(PixelTexture.GetPixel(CoordinateX, CoordinateY), PixelsColour)].ToString();

                Pixels[x, y].PixelColor = PixelTexture.GetPixel(CoordinateX, CoordinateY);

                int IndexX = (int)Pixels[x, y].transform.position.x;
                int IndexY = (int)Pixels[x, y].transform.position.y;
                if (PlayerPrefs.GetInt($"{PixelTexture.name}:{IndexX},{IndexY}") > 0) // Check on color alpha
                {
                    PaintinPixelsOfStart++;
                    Pixels[x, y].Painting();
                }
            }
        }
    }
}
