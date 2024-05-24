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
    [Route("api/tournaments/{tournamentId}/games")]
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(int tournamentId)
        {
            var games = await _unitOfWork.GameRepo.GetAllAsync(tournamentId);

            return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
        }

        // GET: api/tournaments/5/games/4
        [HttpGet("{gameId}")]
        public async Task<ActionResult<GameDto?>> GetGame(int tournamentId, int gameId)
        {
            var game = await _unitOfWork.GameRepo.GetAsync(tournamentId, gameId);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GameDto>(game));
        }

        // PUT: api/tournaments/5/games/4
        [HttpPut("{gameId}")]
        public async Task<IActionResult> PutGame(int tournamentId, int gameId, GameForUpdateDto game)
        {
            // Get the game from db
            var gameEntity = await _unitOfWork.GameRepo.GetAsync(tournamentId, gameId);
            if (gameEntity == null) return NotFound();

            var finalGame = _mapper.Map(game, gameEntity);

            _unitOfWork.GameRepo.Update(finalGame);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.GameRepo.AnyAsync(tournamentId, gameId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/tournaments/5/games
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(int tournamentId, GameForCreationDto game)
        {
            // check that the tournament the game is connected to exists
            //if (await _unitOfWork.TournamentRepo.GetAsync(tournamentId) == null)
            //    {
            //        return BadRequest("Tournament does not exist");
            //    }

            var finalGame = _mapper.Map<Game>(game);
            finalGame.TournamentId = tournamentId;

            _unitOfWork.GameRepo.Add(finalGame);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("GetGame", new { tournamentId = finalGame.TournamentId, gameId = finalGame.Id }, game);
        }

        // DELETE: api/tournaments/5/games/4
        [HttpDelete("{gameId}")]
        public async Task<IActionResult> DeleteGame(int tournamentId, int gameId)
        {
            var game = await _unitOfWork.GameRepo.GetAsync(tournamentId, gameId);
            if (game == null)
            {
                return NotFound();
            }

            _unitOfWork.GameRepo.Remove(game);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
