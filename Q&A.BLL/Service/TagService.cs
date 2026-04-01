using Q_A.BLL.Interface;
using Q_A.Contract;
using Q_A.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.BLL.Service
{
    public class TagService(IUnitOfWork unitOfWork) : ITagService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IReadOnlyList<TagDTO>> GetAllAsync()
        {
            var tags = await _unitOfWork.Tags.GetAllAsync();
            return tags.Select(t => new TagDTO
            {
                TagId = t.TagId,
                Name = t.Name,
                Slug = t.Slug
            }).ToList();
        }
    }

}
