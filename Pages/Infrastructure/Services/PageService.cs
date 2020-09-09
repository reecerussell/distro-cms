using Domain.Dtos;
using Domain.Models;
using Shared;
using Shared.Entity;
using Shared.Exceptions;
using System.Globalization;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    internal class PageService : IPageService
    {
        private readonly IRepository<Page> _repository;

        public PageService(IRepository<Page> repository)
        {
            _repository = repository;
        }

        public async Task<string> CreateAsync(CreatePageDto dto, CultureInfo culture)
        {
            var page = Page.Create(dto, culture);
            _repository.Add(page);
            await _repository.SaveChangesAsync();
            return page.Id;
        }

        public async Task UpdateAsync(UpdatePageDto dto, CultureInfo culture)
        {
            var page = await _repository.FindWithImportsAsync(x => x.Id == dto.Id, p => p.Translations);
            if (page == null)
            {
                throw new NotFoundException(ErrorMessages.PageNotFound);
            }

            page.Update(dto, culture);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var page = await _repository.FindByIdAsync(id);
            if (page == null)
            {
                throw new NotFoundException(ErrorMessages.PageNotFound);
            }

            _repository.Remove(page);
            await _repository.SaveChangesAsync();
        }

        public async Task ActivateAsync(string id)
        {
            var page = await _repository.FindByIdAsync(id);
            if (page == null)
            {
                throw new NotFoundException(ErrorMessages.PageNotFound);
            }

            page.Activate();
            await _repository.SaveChangesAsync();
        }

        public async Task DeactivateAsync(string id)
        {
            var page = await _repository.FindByIdAsync(id);
            if (page == null)
            {
                throw new NotFoundException(ErrorMessages.PageNotFound);
            }

            page.Deactivate();
            await _repository.SaveChangesAsync();
        }
    }
}
