[System.Serializable]
public class Sentence
{
    public Character character { get; private set; }
    public string sentence { get; private set; }

    public Sentence(Character character, string sentence)
    {
        this.character = character;
        this.sentence = sentence;
    }
    
}
