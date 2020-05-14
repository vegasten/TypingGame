using System.Collections.Generic;
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
        Vector2 randomPosition = getRandomPositionOnWordContainer();

        var word = Instantiate(_wordDisplayPrefab, _wordContainer).GetComponent<Word>();
        word.InitalizeWord(wordString, randomPosition, rulesData);

        return word;
    }

    private Vector2 getRandomPositionOnWordContainer()
    {
        var width = _wordContainer.rect.width;
        var height = _wordContainer.rect.height;

        var randomX = Random.Range(0, width) + 200; // TODO Fix these magic numbers
        var randomY = Random.Range(0, height) + 950;

        return new Vector2(randomX, randomY);
    }
}
