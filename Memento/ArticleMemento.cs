namespace MementoWebApp.Memento
{
    public class ArticleMemento
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public ArticleMemento() { }
        public ArticleMemento (string title, string body)
        {
            Title = title; Body = body;
        }
    }
}
