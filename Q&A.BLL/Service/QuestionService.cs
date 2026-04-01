using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Q_A.BLL.Interface;
using Q_A.Contract;
using Q_A.DAL.Interface;
using Q_A.Domain;

public class QuestionService : IQuestionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVoteService _voteService;
    private readonly UserManager<ApplicationUser> _userManager;

    public QuestionService(IUnitOfWork unitOfWork, IVoteService voteService, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _voteService = voteService;
        _userManager = userManager;
    }


   

   

    public async Task CreateQuestionAsync(string userEmail, string title, string description, int categoryId, IEnumerable<int> tagIds, IEnumerable<string> newTagNames)
    {
        if (string.IsNullOrWhiteSpace(userEmail))
            throw new ArgumentException("User email is required.", nameof(userEmail));

        var user = await ResolveUserAsync(userEmail);

        var question = new Question
        {
            Title = title,
            Description = description,
            CategoryId = categoryId,
            UserId = user.Id,
            ViewCount = 0,
            CreatedAt = DateTime.UtcNow
        };
        await _unitOfWork.Questions.AddAsync(question);
        await _unitOfWork.SaveChangesAsync();

        // Process tags
        var distinctTagIds = tagIds.Distinct().ToList();
        var cleanedNewNames = newTagNames
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Select(n => n.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (cleanedNewNames.Count > 0)
        {
            var allTags = await _unitOfWork.Tags.GetAllAsync();
            foreach (var name in cleanedNewNames)
            {
                var existing = allTags.FirstOrDefault(t => string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase));
                if (existing != null)
                {
                    distinctTagIds.Add(existing.TagId);
                    continue;
                }
                var slug = ToSlug(name);
                var slugExists = allTags.Any(t => string.Equals(t.Slug, slug, StringComparison.OrdinalIgnoreCase));
                var finalSlug = slugExists ? slug + "-" + DateTime.UtcNow.Ticks : slug;
                var newTag = new Tag
                {
                    Name = name,
                    Slug = finalSlug,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.Tags.AddAsync(newTag);
                await _unitOfWork.SaveChangesAsync();
                allTags = await _unitOfWork.Tags.GetAllAsync();
                distinctTagIds.Add(newTag.TagId);
            }
        }

        distinctTagIds = distinctTagIds.Distinct().ToList();
        foreach (var tagId in distinctTagIds)
        {
            await _unitOfWork.QuestionTags.AddAsync(new QuestionTag
            {
                QuestionId = question.QuestionId,
                TagId = tagId
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

    private static string ToSlug(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;
        text = text.Trim().ToLowerInvariant();
        var parts = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var slug = string.Join("-", parts);
        var allowed = new char[slug.Length];
        int j = 0;
        for (int i = 0; i < slug.Length; i++)
        {
            var c = slug[i];
            if (char.IsLetterOrDigit(c) || c == '-') allowed[j++] = c;
        }
        return new string(allowed, 0, j).Trim('-') switch { { Length: 0 } s => "tag", var s => s };
    }

   public async Task<QuestionDetailsDTO?> GetQuestionDetailsAsync(int questionId)
    {
        var query = _unitOfWork.Questions
            .Query()
            .Include(q => q.Category)
            .Include(q => q.User)
            .Include(q => q.QuestionTags).ThenInclude(qt => qt.Tag)
            .Include(q => q.Answers).ThenInclude(a => a.User);

        var question = await query.FirstOrDefaultAsync(q => q.QuestionId == questionId);
        if (question == null)
            return null;

        question.ViewCount++;
        _unitOfWork.Questions.Update(question);
        await _unitOfWork.SaveChangesAsync();

        var answerList = (question.Answers ?? new List<Answer>()).OrderBy(a => a.CreatedAt).ToList();
        var answerDtos = new List<AnswerDTO>();
        foreach (var a in answerList)
        {
            var score = await _voteService.GetAnswerScoreAsync(a.AnswerId);
            answerDtos.Add(new AnswerDTO
            {
                AnswerId = a.AnswerId,
                Content = a.Content,
                AuthorName = a.User?.UserName ?? "",
                CreatedAt = a.CreatedAt,
                Score = score
            });
        }

        var questionScore = await _voteService.GetQuestionScoreAsync(question.QuestionId);

        return new QuestionDetailsDTO
        {
            QuestionId = question.QuestionId,
            Title = question.Title,
            Description = question.Description,
            CategoryName = question.Category?.Name ?? "",
            AuthorName = question.User?.UserName ?? "",
            CreatedAt = question.CreatedAt,
            ViewCount = question.ViewCount,
            QuestionScore = questionScore,
            TagNames = question.QuestionTags?.Select(qt => qt.Tag.Name).ToList() ?? new List<string>(),
            Answers = answerDtos
        };
    }

   public async Task<IReadOnlyList<QuestionListDTO>> GetLatestQuestionsAsync(int take)
    {
        var queryable = _unitOfWork.Questions
           .Query()
           .Include(q => q.Category)
           .Include(q => q.Answers)
           .Include(q => q.QuestionTags).ThenInclude(qt => qt.Tag);

        var questions = await queryable
            .OrderByDescending(q => q.CreatedAt)
            .Take(take)
            .ToListAsync();

        return questions.Select(q => new QuestionListDTO
        {
            QuestionId = q.QuestionId,
            Title = q.Title,
            CategoryId = q.CategoryId,
            CategoryName = q.Category?.Name ?? "",
            TagIds = q.QuestionTags?.Select(qt => qt.TagId).Distinct().ToList() ?? new List<int>(),
            CreatedAt = q.CreatedAt,
            AnswerCount = q.Answers?.Count ?? 0,
            ViewCount = q.ViewCount
        }).ToList();
    }

    public async Task<CreateQuestionDTO> GetCreatePageDataAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        var tags = await _unitOfWork.Tags.GetAllAsync();
        return new CreateQuestionDTO
        {
            Categories = categories.Select(c => new SelectDTO { Value = c.CategoryId.ToString(), Text = c.Name }).ToList(),
            Tags = tags.Select(t => new SelectDTO { Value = t.TagId.ToString(), Text = t.Name }).ToList()
        };
    }
}