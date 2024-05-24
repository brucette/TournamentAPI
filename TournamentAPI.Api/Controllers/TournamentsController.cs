using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data.Data;
using TournamentAPI.Core.Entities;
using TournamentAPI.Data.Repositories;
using TournamentAPI.Core.Repositories;
using AutoMapper;
using TournamentAPI.Core.Dto;

namespace TournamentAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TournamentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Tournaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournaments()
        {
            var tournaments = await _unitOfWork.TournamentRepo.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<TournamentDto>>(tournaments));
        }

        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto?>> GetTournament(int id)
        {
            var tournament = await _unitOfWork.TournamentRepo.GetAsync(id);

            if (tournament == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TournamentDto>(tournament));
        }

        // PUT: api/Tournaments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, TournamentDto tournament)
        {
            if (id != tournament.Id)
            {
                return BadRequest();
            }

            // get the tournament as entity from db
            var tournamentEntity = await _unitOfWork.TournamentRepo.GetAsync(id);

            if (tournamentEntity == null) return NotFound();

            // map from dto to entity
            var finalTournament = _mapper.Map(tournament, tournamentEntity);

            _unitOfWork.TournamentRepo.Update(finalTournament);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.TournamentRepo.AnyAsync(id))
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

        // POST: api/Tournaments
        [HttpPost]
        public async Task<ActionResult<Tournament>> PostTournament(TournamentForCreationDto tournament)
        {
            var finalTournament = _mapper.Map<Tournament>(tournament);

            _unitOfWork.TournamentRepo.Add(finalTournament);

            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("GetTournament", new { id = finalTournament.Id }, tournament);
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _unitOfWork.TournamentRepo.GetAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            _unitOfWork.TournamentRepo.Remove(tournament);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
