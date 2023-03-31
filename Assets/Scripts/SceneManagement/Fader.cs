using System.Collections;
using UnityEngine;


namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentActiveFade = null;


        public IEnumerator Fade(float alphaTarget, float time)
        {
            if (currentActiveFade != null)
                StopCoroutine(currentActiveFade);

            currentActiveFade = StartCoroutine(FadeRoutine(alphaTarget, time));
            yield return currentActiveFade;
        }


        IEnumerator FadeRoutine(float alphaTarget, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, alphaTarget))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, alphaTarget, Time.deltaTime / time);
                yield return null;
            }
        }
        

        public IEnumerator FadeIn(float time)  => Fade(0, time);
        public IEnumerator FadeOut(float time) => Fade(1f, time);
        public void FadeOutImmediate() => Fade(0, 0);
        // GetComponent<CanvasGroup>().alpha = 1;
    }
}