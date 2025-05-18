using Football_League.Common;
using Football_League.Data.DTOs;
using Football_League.Data.Models;
using Football_League.Services.MatchServices.Interfaces;
using Football_League.Shared.APIResponses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Football_League.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly ILogger<MatchController> logger;
        private readonly IMatchService matchServices;

        public MatchController(ILogger<MatchController> logger, IMatchService matchServices)
        {
            this.logger = logger;
            this.matchServices = matchServices;
        }

        /// <summary>
        /// Gets all matches including info for the teams.
        /// </summary>
        /// <response code="200">Matches has been retrieved successfully.</response>
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllMatches()
        {
            var result = await matchServices.GetAllMatchesAsync();
            var message = SuccessMessages.DataRetrieved;
            return Ok(new BaseAPIResponse<IEnumerable<MatchDTO>>(true, message, result));
        }

        /// <summary>
        /// Creates a match object using AddMatchDTO data.
        /// </summary>
        /// <param name="match">DTO object representing the match to be created.</param>
        /// <response code="201">Match has been created successfully.</response>
        /// <response code="400">Error on match creation.</response>
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMatch(AddMatchDTO match)
        {
            await matchServices.AddMatchAsync(match);
            var message = string.Format(SuccessMessages.EntityAdded, nameof(Match));
            return StatusCode(StatusCodes.Status201Created, new BaseAPIResponse(true, message));
        }

        /// <summary>
        /// Updates an existing match object using UpdateMatchDTO data.
        /// </summary>
        /// <param name="match">DTO object representing the match to be updated.</param>
        /// <response code="200">Match has been updated successfully.</response>
        /// <response code="400">Error on match update.</response>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateMatch(UpdateMatchDTO match)
        {
            await matchServices.UpdateMatchAsync(match);
            var message = string.Format(SuccessMessages.EntityUpdated, nameof(Match), match.Id);
            return Ok(new BaseAPIResponse(true, message));
        }

        /// <summary>
        /// Deletes a match object by match id.
        /// </summary>
        /// <param name="id">Represents the id of the match to be deleted.</param>
        /// <response code="200">Match has been deleted successfully.</response>
        /// <response code="400">Error on match deletion.</response>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            await matchServices.DeleteMatchAsync(id);
            var message = string.Format(SuccessMessages.EntityDeleted, nameof(Match), id);
            return Ok(new BaseAPIResponse(true, message));
        }
    }
}
