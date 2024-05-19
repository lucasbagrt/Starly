using Businesses.Domain.Dtos;
using Businesses.Domain.Entities;
using System.Text.Json;

namespace Businesses.UnitTests.Fixtures;

public class BusinessTestFixture
{
    public BusinessTestFixture()
    {

    }

    public List<Business> GetBusinessList() =>
        new()
        {
            GetBusiness()
        };

    public List<BusinessDto> GetBusinessDtoList() =>
      new()
      {
            GetBusinessDto()
      };

    public BusinessByIdResponseDto GetBusinessById() =>
       new()
       {
           Id = 1,
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
           Categories = new List<BusinessCategoryDto>
           {
                new() { BusinessId = 1, CategoryId = 1 },
           },
           Hours = new List<BusinessHour>
           {
                new() { Id = 1, BusinessId = 1, DayOfWeek = 1, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 2, BusinessId = 1, DayOfWeek = 2, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 3, BusinessId = 1, DayOfWeek = 3, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 4, BusinessId = 1, DayOfWeek = 4, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 5, BusinessId = 1, DayOfWeek = 5, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 6, BusinessId = 1, DayOfWeek = 6, Start = "1900", End = "0200", IsOvernight = true  }
           }
       };

    public Business GetBusiness() =>
        new()
        {
            Id = 1,
            Name = "Restaurante Teste",
            Phone = "(45) 3521-5000",
            Location = JsonSerializer.Serialize(new BusinessLocationDto
            {
                Address = "Av. República Argnetina, 1700",
                City = "Foz do Iguaçu",
                State = "PR",
                Country = "BR",
                ZipCode = "85852-090"
            }),
            Categories = new List<BusinessCategory>
            {
                new() { Id = 1, BusinessId = 1, CategoryId = 1 },
            },
            Hours = new List<BusinessHour>
            {
                new() { Id = 1, BusinessId = 1, DayOfWeek = 1, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 2, BusinessId = 1, DayOfWeek = 2, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 3, BusinessId = 1, DayOfWeek = 3, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 4, BusinessId = 1, DayOfWeek = 4, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 5, BusinessId = 1, DayOfWeek = 5, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 6, BusinessId = 1, DayOfWeek = 6, Start = "1900", End = "0200", IsOvernight = true  }
            }
        };

    public BusinessDto GetBusinessDto() =>
      new()
      {
          Id = 1,
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
          Categories = new List<BusinessCategoryDto>
          {
                new() { BusinessId = 1, CategoryId = 1 },
          }          
      };

    public UpdateBusinessDto GetUpdateBusinessDto() =>
        new()
        {
            Id = 1,
            Name = "Restaurante Teste Atualizado",
            Phone = "(45) 3521-5000",
            Location = new()
            {
                Address = "Av. República Argnetina, 1700",
                City = "Foz do Iguaçu",
                State = "PR",
                Country = "BR",
                ZipCode = "85852-090"
            },
            Categories = new List<UpdateBusinessCategoryDto>
            {
                new() { Id = 1, BusinessId = 1, CategoryId = 1 },
            },
            Hours = new List<BusinessHour>
            {
                new() { Id = 1, BusinessId = 1, DayOfWeek = 1, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 2, BusinessId = 1, DayOfWeek = 2, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 3, BusinessId = 1, DayOfWeek = 3, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 4, BusinessId = 1, DayOfWeek = 4, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 5, BusinessId = 1, DayOfWeek = 5, Start = "1900", End = "2300", IsOvernight = false },
                new() { Id = 6, BusinessId = 1, DayOfWeek = 6, Start = "1900", End = "0200", IsOvernight = true  }
            }
        };

    public CreateBusinessDto GetCreateBusinessDto() =>
        new()
        {
            Name = "Restaurante Teste",
            Phone = "(45) 3521-5000",
            Location = new()
            {
                Address = "Av. República Argnetina, 1700",
                City = "Foz do Iguaçu",
                State = "PR",
                Country = "BR",
                ZipCode = "85852-090"
            },
            Categories = new List<CreateBusinessCategoryDto>
            {
                new() { CategoryId = 1 },
            },
            Hours = new List<CreateBusinessHourDto>
            {
                new() { DayOfWeek = 1, Start = "1900", End = "2300" },
                new() { DayOfWeek = 2, Start = "1900", End = "2300" },
                new() { DayOfWeek = 3, Start = "1900", End = "2300" },
                new() { DayOfWeek = 4, Start = "1900", End = "2300" },
                new() { DayOfWeek = 5, Start = "1900", End = "2300" },
                new() { DayOfWeek = 6, Start = "1900", End = "0200" }
            }
        };
}
