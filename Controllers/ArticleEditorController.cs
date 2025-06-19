using System.Reflection.Metadata.Ecma335;
using MementoWebApp.Extensions;
using MementoWebApp.Memento;
using MementoWebApp.Models;
using MementoWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MementoWebApp.Controllers
{
    public class ArticleEditorController : Controller
    {

        private const string SessionEditorKey = "Editor";
        private const string SessionHistoryKey = "History";

        private ArticleEditor GetEditor()
        {
           
            if (HttpContext.Session.Get<ArticleEditor>(SessionEditorKey) == null)
            {
                var article = new Article { Title = "No Title", Body = "No Text" };
                HttpContext.Session.Set(SessionEditorKey, new ArticleEditor(article));
            }
            
           
                return HttpContext.Session.Get<ArticleEditor>(SessionEditorKey);
        }

        private EditorHistory GetHistory()
        {
          
            if (HttpContext.Session.Get<EditorHistory>(SessionHistoryKey) == null)
            {
             
                HttpContext.Session.Set(SessionHistoryKey, new EditorHistory());
            }


            return HttpContext.Session.Get<EditorHistory>(SessionHistoryKey); ;
        }

        public IActionResult Index() 
        { 
            var editor = GetEditor();
            return View(new ArticleViewModel
            {
                Title = editor.Article.Title,
                Body = editor.Article.Body,

            });
        }

        [HttpPost]
        public IActionResult Save(ArticleViewModel vm)
        {
            var editor = GetEditor();
            var history = GetHistory();

            history.SaveState(editor.CreateMemento());

            editor.Article.Title = vm.Title;
            editor.Article.Body = vm.Body;

            HttpContext.Session.Set(SessionEditorKey, editor);
            HttpContext.Session.Set(SessionHistoryKey, history);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Undo()
        {
            var editor = GetEditor();
            var history = GetHistory();

            var memento = history.Undo(editor);
            if (memento != null)
            {
                editor.Restore(memento);
                HttpContext.Session.Set(SessionEditorKey, editor);
                HttpContext.Session.Set(SessionHistoryKey, history);
             }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Redo()
        {
            var editor = GetEditor();
            var history = GetHistory();

            var memento = history.Redo(editor);
            if (memento != null)
            {
                editor.Restore(memento);
                HttpContext.Session.Set(SessionEditorKey, editor);
                HttpContext.Session.Set(SessionHistoryKey, history);
               
            }

            return RedirectToAction("Index");
        }
    }
}
