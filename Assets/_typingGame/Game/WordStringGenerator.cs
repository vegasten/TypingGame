using System.Collections.Generic;
using System.Linq;

public class WordStringGenerator
{
    private RandomWordDatabase _wordGenerator;

    public WordStringGenerator()
    {
        _wordGenerator = new RandomWordDatabase();
        _wordGenerator.InitializeDataBase();
    }

    public string GenerateRandomWordWithUniqueFirstLetter(HashSet<Word> existingWords, RulesDataModel rulesData)
    {
        bool acceptableWordIsFound = false;
        string generatedWord = "";

        while (!acceptableWordIsFound)
        {
            generatedWord = _wordGenerator.GetRandomWord(rulesData.MinWordLength, rulesData.MaxWordLength);

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
