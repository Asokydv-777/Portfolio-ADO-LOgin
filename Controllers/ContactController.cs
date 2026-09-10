using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class ContactController : Controller
    {
        private readonly ContactDataAccessLayer _dal;

        public ContactController(ContactDataAccessLayer dal)
        {
            _dal = dal;
        }

        // GET: Contact
        public IActionResult Index()
        {
            return View();
        }

        // POST: Contact/Send
        [HttpPost]
        public IActionResult Send(ContactMessage model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            model.SubmittedAt = DateTime.Now;
            _dal.AddMessage(model);
            TempData["Success"] = "Your message has been sent successfully!";
            return RedirectToAction("Index");
        }

        // GET: Contact/List (Admin: view all messages)
        public IActionResult List()
        {
            var messages = _dal.GetAllMessages();
            return View(messages);
        }

        // GET: Contact/Details/{id}
        public IActionResult Details(int id)
        {
            var message = _dal.GetMessageById(id);
            if (message == null)
                return NotFound();
            return View(message);
        }

        // GET: Contact/Delete/{id}
        public IActionResult Delete(int id)
        {
            var message = _dal.GetMessageById(id);
            if (message == null)
                return NotFound();
            return View(message);
        }

        // POST: Contact/Delete/{id}
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _dal.DeleteMessage(id);
            return RedirectToAction("List");
        }
    }
}