using Businesses.Domain.Dtos;
using Businesses.Domain.Entities;

namespace Businesses.UnitTests.Fixtures;

public class CategoryTestFixture
{
    public CategoryTestFixture()
    {

    }

    public List<Category> GetCategoryList() =>
        new()
        {
            GetCategory()
        };

    public List<CategoryDto> GetCategoryDtoList() =>
      new()
      {
            GetCategoryDto()
      }; 

    public Category GetCategory() =>
        new()
        {
            Id = 1,
            Name = "Categoria teste",            
        };

    public CategoryDto GetCategoryDto() =>
      new()
      {
          Id = 1,
          Name = "Categoria teste",          
      };
   
    public CreateCategoryDto GetCreateCategoryDto() =>
        new()
        {
            Name = "Categoria teste",            
        };
}
