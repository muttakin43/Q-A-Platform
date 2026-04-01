using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Q_A.BLL.Interface;
using Q_A.DAL.Interface;
using Q_A.Domain;

public class AnswerService : IAnswerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public AnswerService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task AddAnswerAsync(int questionId, string userEmail, string content)
    {
        if (string.IsNullOrWhiteSpace(userEmail))
            throw new ArgumentException("User email is required.", nameof(userEmail));
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content is required.", nameof(content));

        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = userEmail,
                Email = userEmail
            };
            await _userManager.CreateAsync(user);
        }

        var answer = new Answer
        {
            QuestionId = questionId,
            UserId = user.Id,
            Content = content.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Answers.AddAsync(answer);
        await _unitOfWork.SaveChangesAsync();
    }

   

   
}