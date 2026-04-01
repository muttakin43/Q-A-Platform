using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Q_A.DAL.Interface;
using Q_A.Domain;

namespace Q_A.web.Controllers
{
    [Authorize]
    public class AnswerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnswerController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // Add Answer
        [HttpPost]
        public async Task<IActionResult> Create(int questionId, string content)
        {
            var userId = _userManager.GetUserId(User);

            var answer = new Answer
            {
                Content = content,
                QuestionId = questionId,
                UserId = userId
            };

            await _unitOfWork.Answers.AddAsync(answer);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("Details", "Question", new { id = questionId });
        }

        // Delete Answer (Own Only)
        public async Task<IActionResult> Delete(int id)
        {
            var answer = await _unitOfWork.Answers.GetByIdAsync(id);
            var userId = _userManager.GetUserId(User);

            if (answer.UserId != userId)
                return Unauthorized();

            _unitOfWork.Answers.Delete(answer);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("Details", "Question", new { id = answer.QuestionId });
        }
    }
}
