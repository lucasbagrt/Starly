using Businesses.Domain.Dtos;

namespace Businesses.API.IntegrationTests.API.MockModels;

public static class CategoryMock
{
    public static CreateCategoryDto GetCategoryDto()
    {
        return new CreateCategoryDto
        {
            Name = "Categoria teste"
        };
    }
}
