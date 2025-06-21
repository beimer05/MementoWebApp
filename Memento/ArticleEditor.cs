using MementoWebApp.Models;

namespace MementoWebApp.Memento
{
    public class ArticleEditor
    {
        public Article Article { get; set; }

        public ArticleEditor(Article article)
        {
            Article = article;
        }

        public ArticleMemento CreateMemento()
        {
            return new ArticleMemento(
                string.Copy(Article.Title ?? string.Empty),
                string.Copy(Article.Body ?? string.Empty));
        }

        public void Restore(ArticleMemento memento)
        {
            Article.Title = memento.Title;
            Article.Body = memento.Body;
        }

    }
}
