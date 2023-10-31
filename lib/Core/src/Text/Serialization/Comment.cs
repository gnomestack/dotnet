namespace GnomeStack.Text.Serialization;

public class Comment : List<string>
{
    public Comment()
    {
    }

    public Comment(IEnumerable<string> collection)
        : base(collection)
    {
    }

    public Comment(string comment)
        : base(comment.Split(Environment.NewLine.ToCharArray()))
    {
    }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, this);
    }
}