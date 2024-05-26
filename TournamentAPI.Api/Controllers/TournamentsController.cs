
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
        public async Task<ActionResult> GetTournaments(bool includeGames = false)
        {
            var tournaments = await _unitOfWork.TournamentRepo.GetAllAsync(includeGames);

            if (includeGames) 
            {
                return Ok(_mapper.Map<IEnumerable<TournamentWithGamesDto>>(tournaments));
            }

            return Ok(_mapper.Map<IEnumerable<TournamentDto>>(tournaments));
        }

        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto?>> GetTournament(int id)
        {
            var tournament = await _unitOfWork.TournamentRepo.GetAsync(id);
            if (tournament == null)
            {
                return NotFound($"Tournament with ID {id} not found.");
            }

            return Ok(_mapper.Map<TournamentDto>(tournament));
        }

        // PUT: api/Tournaments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, TournamentForUpdateDto tournament)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // get the tournament as entity from db
            var tournamentEntity = await _unitOfWork.TournamentRepo.GetAsync(id);

            if (tournamentEntity == null) return NotFound($"Tournament with ID {id} not found.");

            // map from dto to entity
            var finalTournament = _mapper.Map(tournament, tournamentEntity);
            _unitOfWork.TournamentRepo.Update(finalTournament);

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

        // PATCH: api/Tournaments/5
        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchTournament(
            int id, JsonPatchDocument<TournamentForUpdateDto> patchDocument)
        {
            // get the tournament as entity from db
            var tournamentEntity = await _unitOfWork.TournamentRepo.GetAsync(id);
            if (tournamentEntity == null) return NotFound($"Tournament with ID {id} not found.");

            // patch document works on a TournamentForUpdateDto so we need to transorm the entity 
            var tournamentToPatch = _mapper.Map<TournamentForUpdateDto>(tournamentEntity);

            // then apply patch doc, passing in the ModelState will make it invalid if there are any errors, e.g. the GameForUpdateDto contains properties that don't even exist
            patchDocument.ApplyTo(tournamentToPatch, ModelState);

            // checks json patch doc
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // validates the dto itself as per the annotations on it
            if (!TryValidateModel(tournamentToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(tournamentToPatch, tournamentEntity);

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // POST: api/Tournaments
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournament(TournamentForCreationDto tournament)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var finalTournament = _mapper.Map<Tournament>(tournament);

           _unitOfWork.TournamentRepo.Add(finalTournament);
            await _unitOfWork.CompleteAsync();

            //map to the TournamentDto that the GetTournament method returns
            var tournamentDto = _mapper.Map<TournamentDto>(finalTournament);

            return CreatedAtAction(
                "GetTournament", 
                new { id = finalTournament.Id },
                tournamentDto);
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _unitOfWork.TournamentRepo.GetAsync(id);
            if (tournament == null)
            {
                return NotFound($"Tournament with ID {id} not found.");
            }

            _unitOfWork.TournamentRepo.Remove(tournament);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
