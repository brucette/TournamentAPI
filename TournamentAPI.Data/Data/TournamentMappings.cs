
namespace TournamentAPI.Data.Data
{
    public class TournamentMappings : Profile
    {
        public TournamentMappings()
        {
            // Tournaments
            CreateMap<Core.Entities.Tournament, Core.Dto.TournamentDto>();
            CreateMap<Core.Entities.Tournament, Core.Dto.TournamentDto>().ReverseMap();
            CreateMap<Core.Entities.Tournament, Core.Dto.TournamentWithGamesDto>();
            CreateMap<Core.Dto.TournamentForUpdateDto, Core.Entities.Tournament>();
            CreateMap<Core.Dto.TournamentForUpdateDto, Core.Entities.Tournament>().ReverseMap();
            CreateMap<Core.Dto.TournamentForCreationDto, Core.Entities.Tournament>();
            CreateMap<Core.Dto.TournamentForCreationDto, Core.Entities.Tournament>().ReverseMap();
            CreateMap<Core.Dto.TournamentForCreationDto, Core.Dto.TournamentDto>();

            // Games
            CreateMap<Core.Entities.Game, Core.Dto.GameDto>();
            CreateMap<Core.Entities.Game, Core.Dto.GameDto>().ReverseMap();
            CreateMap<Core.Dto.GameForUpdateDto, Core.Entities.Game>();
            CreateMap<Core.Dto.GameForUpdateDto, Core.Entities.Game>().ReverseMap();
            CreateMap<Core.Dto.GameForCreationDto, Core.Entities.Game>();
            CreateMap<Core.Dto.GameForCreationDto, Core.Dto.GameDto>();
        }
    }
}
