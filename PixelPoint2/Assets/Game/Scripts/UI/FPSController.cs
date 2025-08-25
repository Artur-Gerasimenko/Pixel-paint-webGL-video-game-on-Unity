using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSController : MonoBehaviour
{
    private TextMeshProUGUI Text { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(CallFPS());
    }

    private IEnumerator CallFPS()
    {
        while (true)
        {
            Text.text = ((int)(1 / Time.deltaTime)).ToString();
            yield return new WaitForSeconds(1f);
        }

    }
}
