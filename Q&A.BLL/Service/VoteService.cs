using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Q_A.BLL.Interface;
using Q_A.DAL.Interface;
using Q_A.Domain;

public class VoteService : IVoteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public VoteService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<int> GetQuestionScoreAsync(int questionId)
    {
        var votes = await _unitOfWork.Votes
            .Query()
            .Where(v => v.QuestionId == questionId)
            .ToListAsync();
        return votes.Sum(v => (int)v.VoteType);
    }

    public async Task<int> GetAnswerScoreAsync(int answerId)
    {
        var votes = await _unitOfWork.Votes
            .Query()
            .Where(v => v.AnswerId == answerId)
            .ToListAsync();
        return votes.Sum(v => (int)v.VoteType);
    }

    public async Task VoteQuestionAsync(int questionId, string userEmail, bool isUpvote)
    {
        var user = await ResolveUserAsync(userEmail);
        var voteType = isUpvote ? VoteType.Upvote : VoteType.Downvote;

        var existing = await _unitOfWork.Votes
            .Query()
            .FirstOrDefaultAsync(v => v.QuestionId == questionId && v.UserId == user.Id);

        if (existing != null)
        {
            if (existing.VoteType == voteType)
            {
                _unitOfWork.Votes.Delete(existing);
            }
            else
            {
                existing.VoteType = voteType;
                _unitOfWork.Votes.Update(existing);
            }
        }
        else
        {
            await _unitOfWork.Votes.AddAsync(new Vote
            {
                QuestionId = questionId,
                UserId = user.Id,
                VoteType = voteType,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task VoteAnswerAsync(int answerId, string userEmail, bool isUpvote)
    {
        var user = await ResolveUserAsync(userEmail);
        var voteType = isUpvote ? VoteType.Upvote : VoteType.Downvote;

        var existing = await _unitOfWork.Votes
            .Query()
            .FirstOrDefaultAsync(v => v.AnswerId == answerId && v.UserId == user.Id);

        if (existing != null)
        {
            if (existing.VoteType == voteType)
            {
                _unitOfWork.Votes.Delete(existing);
            }
            else
            {
                existing.VoteType = voteType;
                _unitOfWork.Votes.Update(existing);
            }
        }
        else
        {
            await _unitOfWork.Votes.AddAsync(new Vote
            {
                AnswerId = answerId,
                UserId = user.Id,
                VoteType = voteType,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<ApplicationUser> ResolveUserAsync(string userEmail)
    {
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
        return user;
    }
}