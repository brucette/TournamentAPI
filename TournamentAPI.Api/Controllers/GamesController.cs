using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data.Data;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using AutoMapper;
using TournamentAPI.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace TournamentAPI.Api.Controllers
{
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GamesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/tournaments/5/games
        [HttpGet("api/tournaments/{tournamentId}/games")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(int tournamentId)
        {
            // check that the tournament the game is connected to exists
            if (!await TournamentExists(tournamentId))
            {
                return NotFound($"Tournament with ID {tournamentId} not found.");
            }

            var games = await _unitOfWork.GameRepo.GetAllAsync(tournamentId);

            return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
        }

        // GET: api/tournaments/5/games/title/Champions Battle
        [HttpGet("api/tournaments/{tournamentId}/games/title/{title}")]
        public async Task<ActionResult<GameDto?>> GetGame(int tournamentId, string title)
        {
            if (!await TournamentExists(tournamentId))
            {
                return NotFound($"Tournament with ID {tournamentId} not found.");
            }

            var game = await _unitOfWork.GameRepo.GetByTitleAsync(tournamentId, title);
            if (game == null)
            {
                return NotFound($"Game with the title {title} not found.");
            }

            return Ok(_mapper.Map<GameDto>(game));
        }

        // PUT: api/tournaments/5/games/4
        [HttpPut("api/tournaments/{tournamentId}/games/{gameId}")]
        public async Task<IActionResult> PutGame(int tournamentId, int gameId, GameForUpdateDto game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await TournamentExists(tournamentId))
            {
                return NotFound($"Tournament with ID {tournamentId} not found.");
            }

            // Get the game from db
            var gameEntity = await _unitOfWork.GameRepo.GetAsync(tournamentId, gameId);
            if (gameEntity == null) return NotFound($"Game with ID {gameId} not found.");

            var finalGame = _mapper.Map(game, gameEntity);

            _unitOfWork.GameRepo.Update(finalGame);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Concurrency conflict: The tournament was modified by another user.");
            }

            return NoContent();
        }

        // PATCH: api/tournaments/5/games/4
        [HttpPatch("api/tournaments/{tournamentId}/games/{gameId}")]
        public async Task<ActionResult> PatchGame(
            int tournamentId, int gameId, JsonPatchDocument<GameForUpdateDto> patchDocument)
        {
            if (!await TournamentExists(tournamentId))
            {
                return NotFound($"Tournament with ID {tournamentId} not found.");
            }

            // Get the game from db
            var gameEntity = await _unitOfWork.GameRepo.GetAsync(tournamentId, gameId);
            if (gameEntity == null) return NotFound($"Game with ID {gameId} not found.");

            // patch document works on a GameForUpdateDto so we need to transorm the entity 
            var gameToPatch = _mapper.Map<GameForUpdateDto>(gameEntity);

            // then apply patch doc, passing in the ModelState will make it invalid if there are any errors, e.g. the GameForUpdateDto contains properties that don't even exist
            patchDocument.ApplyTo(gameToPatch, ModelState);

            // checks json patch doc
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // validates the dto itself as per the annotations on it
            if (!TryValidateModel(gameToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(gameToPatch, gameEntity);

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // POST: api/tournaments/5/games
        [HttpPost("api/tournaments/{tournamentId}/games")]
        public async Task<ActionResult<GameDto>> PostGame(int tournamentId, GameForCreationDto game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await TournamentExists(tournamentId))
            {
                return NotFound($"Tournament with ID {tournamentId} not found.");
            }

            var finalGame = _mapper.Map<Game>(game);
            finalGame.TournamentId = tournamentId;

            _unitOfWork.GameRepo.Add(finalGame);
            await _unitOfWork.CompleteAsync();

            // map to the GameDto that the GetGame method returns
            var gameDto = _mapper.Map<GameDto>(finalGame);

            return CreatedAtAction("GetGame", new { tournamentId = finalGame.TournamentId, title = finalGame.Title }, gameDto);
        }

        // DELETE: api/tournaments/5/games/4
        [HttpDelete("api/tournaments/{tournamentId}/games/{gameId}")]
        public async Task<IActionResult> DeleteGame(int tournamentId, int gameId)
        {
            if (!await TournamentExists(tournamentId))
            {
                return NotFound($"Tournament with ID {tournamentId} not found.");
            }

            var game = await _unitOfWork.GameRepo.GetAsync(tournamentId, gameId);
            if (game == null)
            {
                 return NotFound($"Game with ID {gameId} not found.");
            }

            _unitOfWork.GameRepo.Remove(game);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // GET: api/games
        [HttpGet("api/games")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAllExistingGames()
        {
            var games = await _unitOfWork.GameRepo.GetAllGamesAsync();
            return Ok(games);
        }

        private async Task<bool> TournamentExists(int tournamentId)
        {
            return await _unitOfWork.TournamentRepo.AnyAsync(tournamentId);
        }
    }
}
