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

        // GET: api/tournaments/5/games/4
        [HttpGet("api/tournaments/{tournamentId}/games/{gameId}")]
        public async Task<ActionResult<GameDto?>> GetGame(int tournamentId, int gameId)
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
            _mapper.Map<GameDto>(finalGame);

            return CreatedAtAction("GetGame", new { tournamentId = finalGame.TournamentId, gameId = finalGame.Id }, finalGame);
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

            //return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
            return Ok(games);
        }

        private async Task<bool> TournamentExists(int tournamentId)
        {
            return await _unitOfWork.TournamentRepo.AnyAsync(tournamentId);
        }
    }
}
