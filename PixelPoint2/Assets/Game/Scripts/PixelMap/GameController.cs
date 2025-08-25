using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class GameController : MonoBehaviour
{
    [SerializeField] private Image EndGamePanel;
    [SerializeField] private ParticleSystem Firework;

    public UnityEvent _finishGameAction;

    public static int GetIndexScene { get { return SceneManager.GetActiveScene().buildIndex; } }

    private Camera MainCamera;
    public static string SaveKey { get; private set; }

    static public bool isEndGame = false;
    private static MakeSprite pixelMapMaker;

    public static void Remove()
    {
        PlayerPrefs.DeleteAll();
    }
    // Start is called before the first frame update
    private void Start()
    {
        pixelMapMaker = GetComponent<MakeSprite>();
        //Remove();
        SaveKey = GetComponent<MakeSprite>().PixelTexture.name;

        if (PlayerPrefs.HasKey($"{SaveKey}EndGame"))
            isEndGame = System.Convert.ToBoolean(PlayerPrefs.GetInt($"{SaveKey}EndGame"));
        else 
            PlayerPrefs.SetInt($"{SaveKey}EndGame", 0);

        Application.targetFrameRate = 60;
        MainCamera = Camera.main;
    }
    
    private void SpawnFireworks(int CountFireworks)
    {
        float CameraViewWidth = Screen.width / Screen.height * MainCamera.orthographicSize * 2f;
        float CameraViewHeight = MainCamera.orthographicSize * 2f;
        for(int firework = 0;firework < CountFireworks; firework++)
        {
            Vector3 FireworkPosition = new Vector3(
                MainCamera.transform.position.x + Random.Range(-CameraViewWidth / 2f, CameraViewWidth / 2f),
                MainCamera.transform.position.y + Random.Range(-CameraViewHeight / 2f, CameraViewHeight / 2f),
                MainCamera.transform.position.z + 1f);

            Instantiate(Firework, FireworkPosition, Quaternion.identity);

            float RandomSize = Random.Range(MainCamera.orthographicSize / 20f, MainCamera.orthographicSize / 25f);

            Firework.transform.localScale =
                new Vector3(
                RandomSize,
                RandomSize,
                0f);
        }
    }
    public void ShowingEndPanel()
    {
        StartCoroutine(ShowEndPanel());
    }
    private IEnumerator ShowEndPanel()
    {
        for (int number = 0; number < 15; number++)
        {
            SpawnFireworks(1);
            yield return null;
        }

        Invoke("OpenEndPanel",2f);
    }
    private void OpenEndPanel()
    {
        EndGamePanel.gameObject.SetActive(true);
    }
}
