using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Q_A.BLL.Interface;
using Q_A.Contract;
using Q_A.DAL.Interface;
using Q_A.Domain;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public CommentService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task AddCommentAsync(int answerId, string userEmail, string content, int? parentCommentId = null)
    {
        if (string.IsNullOrWhiteSpace(userEmail))
            throw new ArgumentException("User email is required.", nameof(userEmail));
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content is required.", nameof(content));

        var user = await ResolveUserAsync(userEmail);

        var comment = new Comment
        {
            AnswerId = answerId,
            UserId = user.Id,
            ParentCommentId = parentCommentId,
            Content = content.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Comments.AddAsync(comment);
        await _unitOfWork.SaveChangesAsync();
    }

  

   public async Task<IReadOnlyList<CommentDTO>> GetCommentsTreeForAnswerAsync(int answerId)
    {
        var comments = await _unitOfWork.Comments
            .Query()
            .Include(c => c.User)
            .Where(c => c.AnswerId == answerId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();

        var byParent = comments
            .GroupBy(c => c.ParentCommentId ?? -1)
            .ToDictionary(g => g.Key, g => g.ToList());

        return BuildTree(byParent, -1);

        static List<CommentDTO> BuildTree(Dictionary<int, List<Comment>> byParent, int parentKey)
        {
            if (!byParent.TryGetValue(parentKey, out var list))
                return new List<CommentDTO>();

            return list.Select(c => new CommentDTO
            {
                CommentId = c.CommentId,
                Content = c.Content,
                AuthorName = c.User?.UserName ?? c.UserId, // UserName from Identity
                CreatedAt = c.CreatedAt,
                Replies = BuildTree(byParent, c.CommentId)
            }).ToList();
        }
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