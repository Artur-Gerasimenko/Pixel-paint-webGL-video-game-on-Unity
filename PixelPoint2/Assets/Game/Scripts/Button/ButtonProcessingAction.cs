using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

 [SelectionBase]
public class ButtonProcessingAction : MonoBehaviour
{
    [SerializeField] private Texture2D PictureSprite;
    [SerializeField] private TextMeshProUGUI TextTime;
    [SerializeField] private TextMeshProUGUI TextProgress;
    [SerializeField] private Slider ProgressSlider;
    // Start is called before the first frame update
    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        int Progress = PlayerPrefs.HasKey($"{PictureSprite.name}PercentProgress") ? PlayerPrefs.GetInt($"{PictureSprite.name}PercentProgress") : 0;
        TextProgress.text = $"{Progress}%";
        TextTime.text = PlayerPrefs.GetString($"{PictureSprite.name}Time");
        ProgressSlider.value = Progress / (100 / ProgressSlider.maxValue);
    }

}
