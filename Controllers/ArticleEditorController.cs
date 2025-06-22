using System.Reflection.Metadata.Ecma335;
using MementoWebApp.Extensions;
using MementoWebApp.Memento;
using MementoWebApp.Models;
using MementoWebApp.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
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
            var editor = HttpContext.Session.Get<ArticleEditor>(SessionEditorKey);


            if (editor == null)
            {
                var article = new Article { Title = "No Title", Body = "No Text" };
                editor = new ArticleEditor(article);
                HttpContext.Session.Set(SessionEditorKey, editor );
            }
            
           
                return editor;
        }

        private EditorHistory GetHistory()
        {
            var history = HttpContext.Session.Get<EditorHistory>(SessionHistoryKey);

            if ( history == null)
            {
                Console.WriteLine("History is null - creating new one");
                history = new EditorHistory();
                HttpContext.Session.Set(SessionHistoryKey, history);
            }
            else
            {
                Console.WriteLine("Restored history with undo count: " + history.GetUndoCount());
            }

            return history;
        }

        public IActionResult Index() 
        { 
            var editor = GetEditor();
            var history = GetHistory();
            Console.WriteLine("Loading editor: " + editor.Article.Title);

           

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


            Console.WriteLine("Saving previous state...");
            Console.WriteLine("Undo stack before: " + history.GetUndoCount());

            var mementoBeforeChange = editor.CreateMemento();

            history.SaveState(mementoBeforeChange);

           

            editor.Article.Title = vm.Title;
            editor.Article.Body = vm.Body;

            HttpContext.Session.Set(SessionEditorKey, editor);
            HttpContext.Session.Set(SessionHistoryKey, history);


            Console.WriteLine("Undo stack after: " + history.GetUndoCount());

       

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Undo()
        {
            var editor = GetEditor();
            var history = GetHistory();

            Console.WriteLine("Undo clicked. Undo stack before: " + history.GetUndoCount());


            var memento = history.Undo(editor);
            if (memento != null)
            {
                Console.WriteLine("Restoring to title: " + memento.Title);
                editor.Restore(memento);
                HttpContext.Session.Set(SessionEditorKey, editor);
                HttpContext.Session.Set(SessionHistoryKey, history);
            }
            else
            {
                Console.WriteLine("No memento to restore.");
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
