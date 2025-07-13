using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarTwinkle : MonoBehaviour
{
    [Header("Twinkle Timing Settings")]
    [SerializeField] private Vector2 fadeDurationRange = new Vector2(0.5f, 2f); // Min/Max fade duration
    [SerializeField] private Vector2 waitDurationRange = new Vector2(0.5f, 3f); // Min/Max wait duration before next fade

    [Header("Optional Settings")]
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Smooth fade

    private List<SpriteRenderer> stars = new List<SpriteRenderer>();

    private void Awake()
    {
        // Find all child objects that have SpriteRenderer
        foreach (Transform child in transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                stars.Add(sr);
            }
        }
    }

    private void Start()
    {
        foreach (var star in stars)
        {
            // Start twinkle coroutine for each star with random delay so they're desynchronized
            float initialDelay = Random.Range(0f, 2f);
            StartCoroutine(TwinkleRoutine(star, initialDelay));
        }
    }

    private IEnumerator TwinkleRoutine(SpriteRenderer star, float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            float fadeDuration = Random.Range(fadeDurationRange.x, fadeDurationRange.y);
            float waitDuration = Random.Range(waitDurationRange.x, waitDurationRange.y);

            // Fade out
            yield return Fade(star, 1f, 0f, fadeDuration);

            // Fade in
            yield return Fade(star, 0f, 1f, fadeDuration);

            // Wait before next twinkle
            yield return new WaitForSeconds(waitDuration);
        }
    }

    private IEnumerator Fade(SpriteRenderer sr, float from, float to, float duration)
    {
        float elapsed = 0f;
        Color c = sr.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float alpha = Mathf.Lerp(from, to, fadeCurve.Evaluate(t));
            sr.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        sr.color = new Color(c.r, c.g, c.b, to);
    }
}
