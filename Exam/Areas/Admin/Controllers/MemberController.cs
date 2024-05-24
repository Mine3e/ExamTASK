using Exam.Business.Exceptions;
using Exam.Business.Services.Abstracts;
using Exam.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MemberController : Controller
    {
        private readonly IMemberSercvice _memberSercvice;
        public MemberController(IMemberSercvice memberSercvice)
        {
            _memberSercvice = memberSercvice;
        }
        public IActionResult Index()
        {
            var members = _memberSercvice.GetAllMember();
            return View(members);
        }
        public IActionResult Delete(int id)
        {
            var existmember = _memberSercvice.GetMember(x => x.Id == id);
            if (existmember == null) return NotFound();
            return View(existmember);
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Update(int id)
        {
            var member = _memberSercvice.GetMember(x => x.Id == id);
            if (member == null) return NotFound();
            return View(member);
        }
        [HttpPost]
        public IActionResult Create(Member member)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _memberSercvice.AddMember(member);
            }
            catch (FileSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (FileContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (ImageFileNullException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult DeleteMember(int id )
        {
            try
            {
                _memberSercvice.DeleteMember(id);
            }
            catch(EntityNotFondException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(ImageFileNotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Update(Member member)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _memberSercvice.UpdateMember(member.Id, member);
            }
            catch(FileSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (FileContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (EntityNotFondException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (ImageFileNotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
