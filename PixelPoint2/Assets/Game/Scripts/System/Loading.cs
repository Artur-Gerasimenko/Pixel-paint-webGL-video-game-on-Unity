using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Loading : MonoBehaviour
{
    [SerializeField] private State order = State.Index;
    [SerializeField] private Image LoadingPanel;
    [SerializeField] private Image ScrollViewLoad;
    [SerializeField] private TextMeshProUGUI TextProgress;

    enum State : int
    {
        [InspectorName("IndexScene")]
        Index = 0,
        [InspectorName("NextScene")]
        Next = 1,
    }

    public void OnClickToOpenIndexSceneWithLoading(int IndexScene)
    {
        //LoadingPanel.gameObject.SetActive(true);
        StartCoroutine(routine: LoadingLevel(IndexScene));
    }
    IEnumerator LoadingLevel(int Index) // order == 0 по индексу; order == 1 следующий;
    {
        LoadingPanel.gameObject.SetActive(true);
        int IndexScene = (int)order < 1 ? Index : SceneManager.GetActiveScene().buildIndex + 1;
        AsyncOperation asyncAction = SceneManager.LoadSceneAsync(IndexScene);
        asyncAction.allowSceneActivation = false;

        while (!asyncAction.isDone)
        {
            TextProgress.text = $"{asyncAction.progress * 100f}%";
            ScrollViewLoad.fillAmount += (asyncAction.progress - ScrollViewLoad.fillAmount) * 1f;
            if(asyncAction.progress >= .9f && !asyncAction.allowSceneActivation)
            {
                yield return new WaitForSeconds(2f);
                asyncAction.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
