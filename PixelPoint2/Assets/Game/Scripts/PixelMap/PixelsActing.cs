using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PixelsActing : MonoBehaviour
{
    public Color ColorAllocate;
    [SerializeField] private Color ColorDefault;
    public bool[] ColorActing { get; private set; }
    public MakeSprite BuilderPixelMap { get; private set; }
    public Color isActiveColour { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        BuilderPixelMap = GetComponent<MakeSprite>();
    }
    public bool FillPixels()
    {
        int Alpha = default;
        int NotAlpha = default;
        for (int X = 0; X < BuilderPixelMap.Pixels.GetLength(0); X++)
        {
            for (int Y = 0; Y < BuilderPixelMap.Pixels.GetLength(1); Y++)
            {
                if (BuilderPixelMap.PixelTexture.GetPixel(X, Y).a >= 1) Alpha++;
                else NotAlpha++;
            }
        }
        for (int X = 0; X < BuilderPixelMap.Pixels.GetLength(0); X++)
        {
            for (int Y = 0; Y < BuilderPixelMap.Pixels.GetLength(1); Y++)
            {
                if (BuilderPixelMap.Pixels[X, Y].isPainted )
                {

                    continue;
                }
                else return false;
            }
        }
        return true;
    }

    public void ShowActivePixelsColor(Color ActiveColor)
    {
        int PaintedPixels = 0;
        foreach(PixelAction pixel in BuilderPixelMap.Pixels)
        {
            PixelAction action = pixel.GetComponent<PixelAction>();
            SpriteRenderer rendererPixel = pixel.GetComponent<SpriteRenderer>();

            if (!action.isPainted && action.PixelColor == ActiveColor)
            {
                PaintedPixels++;
                action.Painting(ColorAllocate, true);
            }
            else if (!action.isPainted && rendererPixel.color == ColorAllocate) action.Painting(ColorDefault, true);
        }
        MakeSprite.remainderNotPaintPixels = PaintedPixels;
    }
}
