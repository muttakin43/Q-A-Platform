using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Q_A.DAL.Interface;
using Q_A.Domain;

namespace Q_A.web.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // Add Comment (Question/Answer both)
        [HttpPost]
        public async Task<IActionResult> Create(string content, int? questionId, int? answerId)
        {
            var userId = _userManager.GetUserId(User);

            var comment = new Comment
            {
                Content = content,
                QuestionId = questionId,
                AnswerId = answerId,
                UserId = userId
            };

            await _unitOfWork.Comments.AddAsync(comment);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("Details", "Question", new { id = questionId ?? answerId });
        }

        // Delete Comment (Own Only)
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(id);
            var userId = _userManager.GetUserId(User);

            if (comment.UserId != userId)
                return Unauthorized();

            _unitOfWork.Comments.Delete(comment);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("Details", "Question", new { id = comment.QuestionId });
        }
    }
}
