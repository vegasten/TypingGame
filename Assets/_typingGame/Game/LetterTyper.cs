using System.Collections.Generic;
using System.Linq;

public class LetterTyper
{
    public Word TryToTypeLetter(char letter, Word activeWord, HashSet<Word> existingWords) 
    {
        if (activeWord == null || string.IsNullOrEmpty(activeWord?.GetWord))
        {
            Word wordWithCorrectFirstLetter = existingWordWithFirstLetter(letter, existingWords);
            if (wordWithCorrectFirstLetter == null)
            {
                return null;
            }

            activeWord = wordWithCorrectFirstLetter;
            activeWord.TryToTypeNextLetter(letter);
            return activeWord;
        }

        else
        {
            activeWord.TryToTypeNextLetter(letter);
        }
        return null;
    }

    private Word existingWordWithFirstLetter(char letter, HashSet<Word> wordsToCheck)
    {
        var firstWordWithCorrectFirstLetter = wordsToCheck.Where(word => word.GetWord[0] == letter).FirstOrDefault();
        return firstWordWithCorrectFirstLetter;
    }
}
