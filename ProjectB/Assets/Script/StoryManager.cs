using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private Image[] storyImages;
    private int currentIndex = 0;
    public float fadeDuration = 1f;
    private bool isFading = false;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
        for(int i = 0; i < storyImages.Length; i++)
        {
            storyImages[i].color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isFading)
        {
            if (currentIndex < storyImages.Length)
            {
                StartCoroutine(FadeOut(storyImages[currentIndex]));
                currentIndex++;
                if(currentIndex >= storyImages.Length)
                {
                    if(SceneManager.GetActiveScene().buildIndex == 1)
                    {
                        SceneLoadManager.Instance.GoInGame();
                    }
                    else if(SceneManager.GetActiveScene().buildIndex == 3)
                    {
                        SceneLoadManager.Instance.GoMenu();
                    }
                }
            }
        }
    }
    IEnumerator FadeOut(Image image)
    {
        isFading = true;

        float currentTime = 0f;
        Color originalColor = image.color;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // 완전히 투명하게 설정
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        isFading = false;
    }
    void asdfadeout(Image image)
    {
        float currentTime = 0f;
        float percent = 0f;

        while(percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 1;

            Color color = image.color;
            
        }
    }

}
