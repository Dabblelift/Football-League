using Football_League.Common;
using Football_League.Data.DTOs;
using Football_League.Data.Models;
using Football_League.Enums;
using Football_League.Services.TeamServices.Interfaces;
using Football_League.Services.TeamServices.SortingStrategies;
using Football_League.Shared.APIResponses;
using Microsoft.AspNetCore.Mvc;

namespace Football_League.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService teamServices;

        public TeamController(ITeamService teamServices)
        {
            this.teamServices = teamServices;
        }

        /// <summary>
        /// Gets all teams info.
        /// </summary>
        /// <response code="200">Teams has been retrieved successfully.</response>
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllTeams(TeamSortOption option)
        {
            ISortingStrategy sortingStrategy;
            switch (option)
            {
                case TeamSortOption.GoalDifference:
                    sortingStrategy = new SortByGoalDifferenceStrategy();
                    break;
                case TeamSortOption.Wins:
                    sortingStrategy = new SortByWinsStrategy();
                    break;
                default:
                    throw new ArgumentException(string.Format(ErrorMessages.InvalidSortingType, option.ToString()));
            }

            var result = await teamServices.GetAllTeamsAsync(sortingStrategy);
            var message = SuccessMessages.DataRetrieved;
            return Ok(new BaseAPIResponse<IEnumerable<TeamDTO>>(true, message, result));
        }

        /// <summary>
        /// Creates a team object using AddTeamDTO data.
        /// </summary>
        /// <param name="team">DTO object representing the team to be created.</param>
        /// <response code="201">Team has been created successfully.</response>
        /// <response code="400">Error on team creation.</response>
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddTeam(AddTeamDTO team)
        {
            await teamServices.AddTeamAsync(team);
            var message = string.Format(SuccessMessages.EntityAdded, nameof(Team));
            return StatusCode(StatusCodes.Status201Created, new BaseAPIResponse(true, message));
        }

        /// <summary>
        /// Updates an existing team object using UpdateTeamDTO data.
        /// </summary>
        /// <param name="team">DTO object representing the team to be updated.</param>
        /// <response code="200">Team has been updated successfully.</response>
        /// <response code="400">Error on team update.</response>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTeam(UpdateTeamDTO team)
        {
            await teamServices.UpdateTeamAsync(team);
            var message = string.Format(SuccessMessages.EntityUpdated, nameof(Team), team.Id);
            return Ok(new BaseAPIResponse(true, message));
        }

        /// <summary>
        /// Deletes a team object by team id.
        /// </summary>
        /// <param name="id">Represents the id of the team to be deleted.</param>
        /// <response code="200">Team has been deleted successfully.</response>
        /// <response code="400">Error on team deletion.</response>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            await teamServices.DeleteTeamAsync(id);
            var message = string.Format(SuccessMessages.EntityDeleted, nameof(Team), id);
            return Ok(new BaseAPIResponse(true, message));
        }
    }
}
