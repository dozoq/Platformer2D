using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.scenemanagment
{
    public class LoadingFader : MonoBehaviour
    {
        [SerializeField] private float time = 1f;
        private Coroutine currentlyActiveFade = null;

        private CanvasGroup canvasGroup;
        private float deltaAlpha;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutInstantly()
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut(float time)
        {
            if(currentlyActiveFade != null)
            {
                StopCoroutine(currentlyActiveFade);
            }

            currentlyActiveFade = StartCoroutine(FadeOutRoutine(time));
            return currentlyActiveFade;
        }

        public IEnumerator FadeOutRoutine(float time)
        {
            deltaAlpha = Time.deltaTime / time;
            
            while(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += deltaAlpha;
                yield return null;
            }
        }

        public Coroutine FadeIn(float time)
        {
            if(currentlyActiveFade != null)
            {
                StopCoroutine(currentlyActiveFade);
            }

            currentlyActiveFade = StartCoroutine(FadeInRoutine(time));
            return currentlyActiveFade;
        }

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