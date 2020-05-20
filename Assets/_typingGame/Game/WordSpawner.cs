using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _wordDisplayPrefab = null;
    [SerializeField] private RectTransform _wordContainer = null;

    private WordStringGenerator _wordStringGenerator;

    private void Awake()
    {
        _wordStringGenerator = new WordStringGenerator();
    }

    public Word SpawnRandomWord(HashSet<Word> existingWords, RulesDataModel rulesData)
    {
        string wordString = _wordStringGenerator.GenerateRandomWordWithUniqueFirstLetter(existingWords, rulesData);
        Vector2 randomPosition = getRandomPositionOnWordContainer(existingWords);
        var word = Instantiate(_wordDisplayPrefab, _wordContainer).GetComponent<Word>();
        word.InitalizeWord(wordString, randomPosition, rulesData);

        return word;
    }

    private Vector2 getRandomPositionOnWordContainer(HashSet<Word> existingWords) // TODO Clean up this shit
    {
        var rectsToTest = new List<Rect>();
        foreach (var word in existingWords)
        {
            var rectTransform = word.GetComponent<RectTransform>();

            var x = rectTransform.position.x;
            var y = rectTransform.position.y;
            var w = rectTransform.rect.width;
            var h = rectTransform.rect.height;

            var rect = new Rect(x - w / 2, Screen.height - y - h / 2, w, h);
            rectsToTest.Add(rect);
        }

        var width = _wordContainer.rect.width;
        var height = _wordContainer.rect.height;

        bool isPlacementAccepted = false;

        float randomLocalX = 0f;
        float randomLocalY = 0f;

        while (!isPlacementAccepted)
        {
            randomLocalX = Random.Range(-width / 2, width / 2);
            randomLocalY = Random.Range(0, -height); // Has to be like this because of anchor? 

            var testRectWidth = 300f;
            var testRectHeight = 60f;
            var testRectX = randomLocalX + Screen.width / 2 - testRectWidth / 2;
            var testRectY = -randomLocalY + 50 - testRectHeight / 2;

            if (!rectsToTest.Any(r => r.Overlaps(new Rect(testRectX, testRectY, testRectWidth, testRectHeight))))
            {
                isPlacementAccepted = true;
            }
        }

        return new Vector2(randomLocalX, randomLocalY);
    }
}
