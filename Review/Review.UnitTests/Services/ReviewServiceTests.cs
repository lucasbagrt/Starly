using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using Review.Domain.Dtos;
using Review.Domain.Filters;
using Review.Domain.Interfaces.Integration;
using Review.Domain.Interfaces.Repositories;
using Review.Service.Services;
using Review.UnitTests.Fixtures;
using Starly.CrossCutting.Notifications;

namespace Review.UnitTests.Services;

public class ReviewServiceTests : IClassFixture<ReviewTestFixture>
{
    private readonly ReviewTestFixture _reviewTestFixture;
    private readonly Mock<IReviewRepository> _reviewRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IConfiguration> _configurationMock;    
    private readonly Mock<IBusinessIntegration> _businessIntegrationMock;    
    private readonly Mock<IUserIntegration> _userIntegration;    
    private readonly Mock<NotificationContext> _notificationContextMock;
    private readonly ReviewService _reviewServiceMock;    

    public ReviewServiceTests(ReviewTestFixture reviewTestFixture)
    {
        _reviewTestFixture = reviewTestFixture;

        _notificationContextMock = new Mock<NotificationContext>();
        _reviewRepositoryMock = new Mock<IReviewRepository>();
        _mapperMock = new Mock<IMapper>();
        _configurationMock = new Mock<IConfiguration>();
        _businessIntegrationMock = new Mock<IBusinessIntegration>();
        _userIntegration = new Mock<IUserIntegration>();

        _reviewServiceMock = new ReviewService(
            _notificationContextMock.Object,
            _reviewRepositoryMock.Object,
            _mapperMock.Object,
            _configurationMock.Object,
            _businessIntegrationMock.Object,
            _userIntegration.Object);
    }

    [Fact]
    public async Task GetCountAndRatingByBusiness_ShouldReturnTuple()
    {
        // Arrange
        int businessId = 1;
        var expectedTuple = Tuple.Create(10L, 4.5d);
        _reviewRepositoryMock.Setup(x => x.GetReviewCountAndRatingAsync(businessId))
                          .ReturnsAsync(expectedTuple);

        // Act
        var result = await _reviewServiceMock.GetCountAndRatingByBusiness(businessId);

        // Assert
        Assert.Equal(result.Item1, expectedTuple.Item1);;
        Assert.Equal(result.Item2, expectedTuple.Item2);;
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnCollection()
    {
        // Arrange
        var filter = new ReviewFilter();        
        var expectedCollection = _reviewTestFixture.GetReviewList();
        var reviewListDto = _reviewTestFixture.GetReviewListDto();
        _reviewRepositoryMock.Setup(repo => repo.GetQueryable()).Returns(expectedCollection.AsQueryable());
        _mapperMock.Setup(mapper => mapper.Map<List<ReviewDto>>(expectedCollection)).Returns(reviewListDto);

        // Act
        var result = await _reviewServiceMock.GetAllAsync(filter, string.Empty);

        // Assert
        Assert.True(expectedCollection.Count == 1);        
    }

    [Fact]
    public async Task GetById_ShouldReturnReviewDto()
    {
        // Arrange
        int reviewId = 1;        
        var expectedReview = _reviewTestFixture.GetReview();
        var expectedReviewDto = _reviewTestFixture.GetReviewDto();
        _reviewRepositoryMock.Setup(x => x.SelectAsync(reviewId)).ReturnsAsync(expectedReview);
        _mapperMock.Setup(mapper => mapper.Map<ReviewDto>(expectedReview)).Returns(expectedReviewDto);

        // Act
        var result = await _reviewServiceMock.GetById(reviewId, string.Empty);

        // Assert
        Assert.Equal(expectedReviewDto.Id, result.Id);
        Assert.Equal(expectedReviewDto.Comment, result.Comment);
    }

    [Fact]
    public async Task Create_ShouldReturnDefaultServiceResponseDto()
    {
        // Arrange
        var createReviewDto = _reviewTestFixture.GetCreateReviewDto();
        var review = _reviewTestFixture.GetReview();        
        
        int userId = 1;        
        _reviewRepositoryMock.Setup(x => x.InsertAsync(review)).Returns(Task.CompletedTask);
        _mapperMock.Setup(mapper => mapper.Map<Domain.Entities.Review>(createReviewDto)).Returns(review);
        _businessIntegrationMock.Setup(businessIntegration => businessIntegration.ExistsById(review.BusinessId, It.IsAny<string>()))
                                .ReturnsAsync(true);

        // Act
        var result = await _reviewServiceMock.Create(createReviewDto, userId, string.Empty);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(result.Message, StaticNotifications.ReviewSuccess.Message);
    }
}
