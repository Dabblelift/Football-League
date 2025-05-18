using Football_League.Common;
using Football_League.Data.DTOs;
using Football_League.Data.Models;
using Football_League.Repositories.Interfaces;
using Football_League.Services.MatchServices;
using Football_League.Services.ResultProcessingServices.Interfaces;
using Moq;
using Match = Football_League.Data.Models.Match;

namespace Football_League_Tests
{
    [TestFixture]
    class MatchServiceTests
    {
        private Mock<IMatchRepository> mockMatchRepository;
        private Mock<ITeamRepository> mockTeamRepository;
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IResultProcessor> mockResultProcessor;
        private MatchService matchService;

        [SetUp]
        public void Setup()
        {
            mockMatchRepository = new Mock<IMatchRepository>();
            mockTeamRepository = new Mock<ITeamRepository>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockResultProcessor = new Mock<IResultProcessor>();
            matchService = new MatchService(mockUnitOfWork.Object, mockResultProcessor.Object);
            
            mockUnitOfWork.Setup(u => u.Matches).Returns(mockMatchRepository.Object);
        }

        [Test]
        public void AddMatchAsync_ShouldThrow_WhenTeamIdsMatch()
        {
            var dto = new AddMatchDTO { HomeTeamId = 1, AwayTeamId = 1 };

            var ex = Assert.ThrowsAsync<ArgumentException>(() => matchService.AddMatchAsync(dto));
            Assert.That(ex.Message, Is.EqualTo(ErrorMessages.MatchSameTeams));
        }

        [Test]
        public async Task AddMatchAsync_ShouldCallApplyAndSave()
        {
            var dto = new AddMatchDTO
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamGoals = 2,
                AwayTeamGoals = 1
            };

            mockUnitOfWork.Setup(x => x.Teams.GetByIdAsync(1)).ReturnsAsync(new Team { Id = 1, Name = "Liverpool" });
            mockUnitOfWork.Setup(x => x.Teams.GetByIdAsync(2)).ReturnsAsync(new Team { Id = 2, Name = "Manchester City" });

            await matchService.AddMatchAsync(dto);

            mockResultProcessor.Verify(x => x.Apply(It.IsAny<Match>()), Times.Once);
            mockUnitOfWork.Verify(x => x.Matches.AddAsync(It.IsAny<Match>()), Times.Once);
            mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task AddMatchAsync_ShouldAdd_WhenTeamsAreDifferent()
        {
            var dto = new AddMatchDTO { HomeTeamId = 1, AwayTeamId = 2, HomeTeamGoals = 2, AwayTeamGoals = 1 };
            var homeTeam = new Team { Id = 1, Name = "Arsenal" };
            var awayTeam = new Team { Id = 2, Name = "Newcastle" };

            mockUnitOfWork.Setup(u => u.Matches).Returns(mockMatchRepository.Object);
            mockUnitOfWork.Setup(u => u.Teams).Returns(mockTeamRepository.Object);

            mockTeamRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(homeTeam);
            mockTeamRepository.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(awayTeam);

            await matchService.AddMatchAsync(dto);

            mockMatchRepository.Verify(r => r.AddAsync(It.IsAny<Match>()), Times.Once);
            mockResultProcessor.Verify(p => p.Apply(It.IsAny<Match>()), Times.Once);
            mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public void AddMatchAsync_ShouldThrow_WhenSameTeamIds()
        {
            var dto = new AddMatchDTO { HomeTeamId = 1, AwayTeamId = 1 };

            var ex = Assert.ThrowsAsync<ArgumentException>(() => matchService.AddMatchAsync(dto));
            Assert.That(ex.Message, Is.EqualTo(ErrorMessages.MatchSameTeams));
        }

        [Test]
        public async Task UpdateMatchAsync_ShouldUpdate_WhenValid()
        {
            var dto = new UpdateMatchDTO { Id = 1, HomeTeamId = 1, AwayTeamId = 2, HomeTeamGoals = 1, AwayTeamGoals = 1 };
            var homeTeam = new Team { Id = 1, Name = "Arsenal" };
            var awayTeam = new Team { Id = 2, Name = "Newcastle" };
            var match = new Match
            {
                Id = 1,
                HomeTeamId = homeTeam.Id,
                HomeTeam = homeTeam,
                AwayTeamId = awayTeam.Id,
                AwayTeam = awayTeam
            };

            mockUnitOfWork.Setup(u => u.Matches).Returns(mockMatchRepository.Object);
            mockUnitOfWork.Setup(u => u.Teams).Returns(mockTeamRepository.Object);

            mockMatchRepository.Setup(r => r.GetMatchByIdWithTeamsAsync(1)).ReturnsAsync(match);
            mockTeamRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(homeTeam);
            mockTeamRepository.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(awayTeam);

            await matchService.UpdateMatchAsync(dto);

            mockResultProcessor.Verify(p => p.Revert(It.IsAny<Match>()), Times.Once);
            mockResultProcessor.Verify(p => p.Apply(It.IsAny<Match>()), Times.Once);
            mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteMatchAsync_ShouldDelete_WhenMatchExists()
        {
            var homeTeam = new Team { Id = 1, Name = "Arsenal" };
            var awayTeam = new Team { Id = 2, Name = "Newcastle" };
            var match = new Match
            {
                Id = 1,
                HomeTeamId = homeTeam.Id,
                HomeTeam = homeTeam,
                AwayTeamId = awayTeam.Id,
                AwayTeam = awayTeam
            };

            mockMatchRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(match);

            await matchService.DeleteMatchAsync(1);

            mockMatchRepository.Verify(r => r.Delete(match), Times.Once);
            mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public void DeleteMatchAsync_ShouldThrow_WhenMatchNotFound()
        {
            int nonExistentMatchId = 99;

            mockMatchRepository
                .Setup(r => r.GetByIdAsync(nonExistentMatchId))
                .ThrowsAsync(new ArgumentException(string.Format(ErrorMessages.EntityNotFound, nameof(Match), nonExistentMatchId)));

            var ex = Assert.ThrowsAsync<ArgumentException>(() => matchService.DeleteMatchAsync(nonExistentMatchId));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ErrorMessages.EntityNotFound, nameof(Match), nonExistentMatchId)));
        }
    }
}
