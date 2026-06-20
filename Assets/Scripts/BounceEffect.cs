using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    public float bounceHeight = 0.3f;
    public float bounceDuration = 0.4f;
    public int bounceCount = 2;

    public void StartBounce()
    {
        StartCoroutine(BounceHandler());
    }
    private IEnumerator BounceHandler()
    {
        Vector3 startPosition = transform.position;
        float localHeight = bounceHeight;
        float localDuration = bounceDuration;

        for (int i = 0; i < bounceCount; i++)
        {
            yield return Bounce(startPosition, localHeight, localDuration/2);
            localHeight *= 0.5f;
            localDuration *= 0.8f;
        }
        transform.position = startPosition;

    }
    private IEnumerator Bounce( Vector3 start, float height,float duration)
    {
        Vector3 peek = start + Vector3.up * height;
        float elaspeed = 0f;    // 경과 시간

        while(elaspeed < duration) {
            // Lerp()는 두 점 사이의 중간 지점을 찾는 함수
            transform.position = Vector3.Lerp(start, peek, elaspeed / duration); 
            elaspeed += Time.deltaTime;
            yield return null;

        }
    }
}
