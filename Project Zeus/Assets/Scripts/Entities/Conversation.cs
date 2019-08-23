public class Conversation
{
    private Sentence[] sentences;
    private int position = 0;

    public Conversation(Sentence[] sentences)
    {
        this.sentences = sentences;
    }

    public Sentence GetNext()
    {
        if (this.sentences == null || this.position > this.sentences.Length)
            return null;
        else
        {
            Sentence result = this.sentences[position];
            this.position++;
            return result;
        }
        
    }
}
