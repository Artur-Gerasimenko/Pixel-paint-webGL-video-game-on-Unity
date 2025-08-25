using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelAction : MonoBehaviour
{
    public int number { get; set; }
    public Color PixelColor;
    public bool isPainted { get; set; }
    private SpriteRenderer spriteAction { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        Initialisation();
    }
    private void Initialisation()
    {
        spriteAction = transform.GetComponent<SpriteRenderer>();
    }
    public void Painting(Color PixelColor = default, bool isShow = false, bool click = false)
    {
        if (!isShow)
        {
            foreach (Transform objects in transform)
            {
                objects.gameObject.SetActive(false);
            }
            isPainted = true;
            if(click) MakeSprite.remainderNotPaintPixels--;
        }

        spriteAction.color = !isShow ? this.PixelColor : PixelColor;

    }
}
