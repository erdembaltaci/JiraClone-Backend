using AutoMapper;
using JiraProject.Business.Dtos;
using JiraProject.Entities;

namespace JiraProject.Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // --- OKUMA YÖNÜ: VERİTABANINDAN EKRANA (Entity -> DTO) ---

            CreateMap<User, UserSummaryDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<Issue, IssueDto>()
                // Kural: Enum'ları metin karşılıklarına çevir.
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))

                // Kural: İlişkili entity'den bir alanı al (Flattening).
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))

                // Kural: İlişkili User entity'sini UserSummaryDto'ya çevir.
                .ForMember(dest => dest.Assignee, opt => opt.MapFrom(src => src.Assignee != null
                    ? new UserSummaryDto { Id = src.Assignee.Id, FullName = $"{src.Assignee.FirstName} {src.Assignee.LastName}" }
                    : null))

                .ForMember(dest => dest.Reporter, opt => opt.MapFrom(src =>
                    new UserSummaryDto { Id = src.Reporter.Id, FullName = $"{src.Reporter.FirstName} {src.Reporter.LastName}" }));

            // Kural: Project entity'sini ProjectDto'ya çevirirken, içindeki Team'in adını da al.
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.Name));

            // Kural: User entity'sini UserDto'ya çevirirken Role enum'ını metne çevir.
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            CreateMap<Team, TeamDto>();

            // --- YAZMA YÖNÜ: EKRANDAN VERİTABANINA (DTO -> Entity) ---

            CreateMap<IssueCreateDto, Issue>();
            CreateMap<IssueUpdateDto, Issue>();

            CreateMap<ProjectCreateDto, Project>();
            CreateMap<ProjectUpdateDto, Project>();

            // Kural: UserCreateDto'dan User'a map'lerken PasswordHash alanını Yoksay.
            // Çünkü bu işlemi UserManager'da elle ve güvenli bir şekilde yapıyoruz.
            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<UserUpdateDto, User>();

            CreateMap<TeamCreateDto, Team>();
        }
    }
}