using Football_League.Common;
using Football_League.Data.DTOs;
using Football_League.Data.Models;
using Football_League.Repositories.Interfaces;
using Football_League.Services.TeamServices;
using Moq;

namespace Football_League_Tests
{
    [TestFixture]
    public class TeamServiceTests
    {
        private Mock<ITeamRepository> mockTeamRepository;
        private Mock<IUnitOfWork> mockUnitOfWork;
        private TeamService teamService;

        [SetUp]
        public void Setup() 
        {
            mockTeamRepository = new Mock<ITeamRepository>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
            teamService = new TeamService(mockUnitOfWork.Object);

            mockUnitOfWork.Setup(u => u.Teams).Returns(mockTeamRepository.Object);
        }

        [Test]
        public void AddTeamAsync_ShouldThrow_WhenTeamAlreadyExists()
        {
            var dto = new AddTeamDTO { TeamName = "Manchester United" };
            mockTeamRepository.Setup(r => r.CheckIfTeamExistsAsync(dto.TeamName)).ReturnsAsync(true);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => teamService.AddTeamAsync(dto));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ErrorMessages.TeamAlreadyExists, dto.TeamName)));
        }

        [Test]
        public async Task AddTeamAsync_ShouldAdd_WhenTeamDoesNotExist()
        {
            var dto = new AddTeamDTO { TeamName = "Chelsea" };
            mockTeamRepository.Setup(r => r.CheckIfTeamExistsAsync(dto.TeamName)).ReturnsAsync(false);

            await teamService.AddTeamAsync(dto);

            mockTeamRepository.Verify(r => r.AddAsync(It.Is<Team>(t => t.Name == "Chelsea")), Times.Once);
            mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateTeamAsync_ShouldUpdate_WhenTeamExists()
        {
            var dto = new UpdateTeamDTO { Id = 1, TeamName = "Chelsea", Points = 3 };
            var existing = new Team { Id = 1, Name = "Chelsea", Points = 0 };
            mockTeamRepository.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(existing);
            mockTeamRepository.Setup(r => r.CheckIfTeamExistsAsync(dto.TeamName)).ReturnsAsync(false);

            await teamService.UpdateTeamAsync(dto);

            Assert.That(existing.Name, Is.EqualTo("Chelsea"));
            Assert.That(existing.Points, Is.EqualTo(3));
            mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteTeamAsync_ShouldDelete_WhenTeamExists()
        {
            var existing = new Team { Id = 1, Name = "Chelsea" };
            mockTeamRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            await teamService.DeleteTeamAsync(1);

            mockTeamRepository.Verify(r => r.Delete(existing), Times.Once);
            mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public void DeleteTeamAsync_ShouldThrow_WhenTeamNotFound()
        {
            int nonExistentTeamId = 99;

            mockTeamRepository.Setup(r => r.GetByIdAsync(nonExistentTeamId))
                .ThrowsAsync(new ArgumentException(string.Format(ErrorMessages.EntityNotFound, nameof(Team), nonExistentTeamId)));

            var ex = Assert.ThrowsAsync<ArgumentException>(() => teamService.DeleteTeamAsync(nonExistentTeamId));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ErrorMessages.EntityNotFound, nameof(Team), nonExistentTeamId)));
        }
    }
}