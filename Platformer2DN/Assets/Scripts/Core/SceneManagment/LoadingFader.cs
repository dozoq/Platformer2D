using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace platformer.scenemanagment
{
    public class LoadingFader : MonoBehaviour
    {
        [Tooltip("Color of the fade")]
        [SerializeField] private Color fadeColor = Color.black;

        [Tooltip("How long does it takes to fade in completly")]
        [SerializeField] private float loadTime = 1f;
        private Coroutine currentlyActiveFade = null;

        private CanvasGroup canvasGroup;
        private float deltaAlpha;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            GetComponent<Image>().color = fadeColor;
        }

        // Used when loading first time, so there is no FadeIn effect first
        public void FadeOutInstantly()
        {
            canvasGroup.alpha = 1;
        }

        // Slowly Fade whole screen into configured color (Image)
        public Coroutine FadeOut(float time)
        {
            if(currentlyActiveFade != null)
            {
                StopCoroutine(currentlyActiveFade);
            }

            currentlyActiveFade = StartCoroutine(FadeOutRoutine(time));
            return currentlyActiveFade;
        }

        // Invoked in FadeOut
        public IEnumerator FadeOutRoutine(float time)
        {
            deltaAlpha = Time.deltaTime / time;
            
            while(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += deltaAlpha;
                yield return null;
            }
        }

        //Slowly make the game scene visible
        public Coroutine FadeIn(float time)
        {

            if(currentlyActiveFade != null)
            {
                StopCoroutine(currentlyActiveFade);
            }

            currentlyActiveFade = StartCoroutine(FadeInRoutine(time));
            return currentlyActiveFade;
        }

        // Invoked by FadeIn
        public IEnumerator FadeInRoutine(float time)
        {
            deltaAlpha = Time.deltaTime / time;
            
            while(canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= deltaAlpha;
                yield return null;
            }
        }

    }

}