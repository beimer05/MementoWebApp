using MementoWebApp.Models;

namespace MementoWebApp.Memento
{
    public class ArticleEditor
    {
        public Article Article { get; private set; }

        public ArticleEditor(Article article)
        {
            Article = article;
        }

        public ArticleMemento CreateMemento()
        {
            return new ArticleMemento(Article.Title, Article.Body);
        }

        public void Restore(ArticleMemento memento)
        {
            Article.Title = memento.Title;
            Article.Body = memento.Body;
        }

    }
}
