using Businesses.Domain.Dtos;
using Businesses.Domain.Entities;

namespace Businesses.API.IntegrationTests.API.MockModels;

public static class BusinessMock
{
    public static CreateBusinessDto GetBusinessDto()
    {
        return new CreateBusinessDto
        {
            Name = "Restaurante Teste",
            Phone = "(45) 3521-5000",
            Location = new BusinessLocationDto
            {
                Address = "Av. República Argnetina, 1700",
                City = "Foz do Iguaçu",
                State = "PR",
                Country = "BR",
                ZipCode = "85852-090"
            },
            Categories = new List<CreateBusinessCategoryDto>
            {
                new CreateBusinessCategoryDto { CategoryId = 1 },
            },
            Hours = new List<CreateBusinessHourDto>
            {
                new CreateBusinessHourDto { DayOfWeek = 1, Start = "1900", End = "2300" },
                new CreateBusinessHourDto { DayOfWeek = 2, Start = "1900", End = "2300" },
                new CreateBusinessHourDto { DayOfWeek = 3, Start = "1900", End = "2300" },
                new CreateBusinessHourDto { DayOfWeek = 4, Start = "1900", End = "2300" },
                new CreateBusinessHourDto { DayOfWeek = 5, Start = "1900", End = "2300" },
                new CreateBusinessHourDto { DayOfWeek = 6, Start = "1900", End = "0200" }
            }
        };
    }   

    public static CreateBusinessDto GetInvalidBusinessDto()
    {
        return new CreateBusinessDto
        {
            Name = string.Empty,
            Phone = string.Empty,
            Location = null,
            Categories = null,
            Hours = null
        };
    }

    public static UpdateBusinessDto GetUpdateBusinessDto()
    {
        return new UpdateBusinessDto
        {
            Id = 1,
            Name = "Restaurante Teste Atualizado",
            Phone = "(45) 3521-5000",
            Location = new BusinessLocationDto
            {
                Address = "Av. República Argnetina, 1700",
                City = "Foz do Iguaçu",
                State = "PR",
                Country = "BR",
                ZipCode = "85852-090"
            },
            Categories = new List<UpdateBusinessCategoryDto>
            {
                new UpdateBusinessCategoryDto { Id = 1, BusinessId = 1, CategoryId = 1 },
            },
            Hours = new List<BusinessHour>
            {
                new BusinessHour { Id = 1, BusinessId = 1, DayOfWeek = 1, Start = "1900", End = "2300", IsOvernight = false },
                new BusinessHour { Id = 2, BusinessId = 1, DayOfWeek = 2, Start = "1900", End = "2300", IsOvernight = false },
                new BusinessHour { Id = 3, BusinessId = 1, DayOfWeek = 3, Start = "1900", End = "2300", IsOvernight = false },
                new BusinessHour { Id = 4, BusinessId = 1, DayOfWeek = 4, Start = "1900", End = "2300", IsOvernight = false },
                new BusinessHour { Id = 5, BusinessId = 1, DayOfWeek = 5, Start = "1900", End = "2300", IsOvernight = false },
                new BusinessHour { Id = 6, BusinessId = 1, DayOfWeek = 6, Start = "1900", End = "0200", IsOvernight = true  }
            }
        };
    }

    public static UpdateBusinessDto GetNonExistentBusinessDto()
    {
        return new UpdateBusinessDto
        {
            Id = 2,
            Name = "Restaurante Teste Atualizado",
            Phone = "(45) 3521-5000",
            Location = new BusinessLocationDto
            {
                Address = "Av. República Argnetina, 1700",
                City = "Foz do Iguaçu",
                State = "PR",
                Country = "BR",
                ZipCode = "85852-090"
            },
            Categories = new List<UpdateBusinessCategoryDto>
            {
                new UpdateBusinessCategoryDto { Id = 1, BusinessId = 1, CategoryId = 1 },
            },
            Hours = new List<BusinessHour>
            {
                new BusinessHour { Id = 1, BusinessId = 1, DayOfWeek = 1, Start = "1900", End = "2300", IsOvernight = false },
                new BusinessHour { Id = 2, BusinessId = 1, DayOfWeek = 2, Start = "1900", End = "2300", IsOvernight = false },
                new BusinessHour { Id = 3, BusinessId = 1, DayOfWeek = 3, Start = "1900", End = "2300", IsOvernight = false },
                new BusinessHour { Id = 4, BusinessId = 1, DayOfWeek = 4, Start = "1900", End = "2300", IsOvernight = false },
                new BusinessHour { Id = 5, BusinessId = 1, DayOfWeek = 5, Start = "1900", End = "2300", IsOvernight = false },
                new BusinessHour { Id = 6, BusinessId = 1, DayOfWeek = 6, Start = "1900", End = "0200", IsOvernight = true  }
            }
        };
    }

    public static UpdateBusinessDto GetInvalidUpdateBusinessDto()
    {
        return new UpdateBusinessDto
        {
            Id = 1,
            Name = string.Empty,
            Phone = string.Empty,
            Location = null,
            Categories = null,
            Hours = null
        };
    }
}
