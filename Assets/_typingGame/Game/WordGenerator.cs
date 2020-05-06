using System.Collections.Generic;
using System.Linq;

public class WordGenerator
{
    private RandomWordDatabase _wordGenerator;

    public WordGenerator(GameManager wordManager)
    {
        _wordGenerator = new RandomWordDatabase();
    }

    public string GenerateRandomWordWithUniqueFirstLetter(HashSet<Word> existingWords)
    {
        bool acceptableWordIsFound = false;
        string generatedWord = "";

        while (!acceptableWordIsFound)
        {
            generatedWord = _wordGenerator.GetRandomWord();

            if (hasNoEqualFirstLetters(existingWords, generatedWord))
            {
                acceptableWordIsFound = true;
            }
        }

        return generatedWord;
    }

    private static bool hasNoEqualFirstLetters(HashSet<Word> existingWords, string generatedWord)
    {
        return !existingWords.Any(w => w.GetWord[0] == generatedWord[0]);
    }
}
