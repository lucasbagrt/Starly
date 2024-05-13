using Businesses.Domain.Dtos;
using Businesses.Domain.Filters;
using Starly.Domain.Dtos.Default;

namespace Businesses.Domain.Interfaces.Services;

public interface ICategoryService
{
    Task<ICollection<CategoryDto>> GetAllAsync(CategoryFilter filter);
    Task<CategoryDto> GetById(int id);
    Task<DefaultServiceResponseDto> Create(CreateCategoryDto categoryDto);
    Task<DefaultServiceResponseDto> Delete(int id);
    Task<DefaultServiceResponseDto> Update(CategoryDto categoryDto);
}
