namespace MementoWebApp.Memento
{
    public class ArticleMemento
    {
        public string Title { get; }
        public string Body { get; }

        public ArticleMemento (string title, string body)
        {
            Title = title; Body = body;
        }
    }
}
