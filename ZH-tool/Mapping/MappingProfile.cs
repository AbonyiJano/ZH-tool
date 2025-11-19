using ZH_tool.DTOs;
using ZH_tool.Models;
using AutoMapper;

namespace ZH_tool.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entitás -> Kimeneti DTO (ZH -> ZhResponseDto)
            CreateMap<Zh, ZhResponseDto>();

            // Entitás -> Listázó DTO (ZH -> ZhListDto)
            CreateMap<Zh, ZhListDto>();

            // Bemeneti DTO -> Entitás (CreateZhDto -> ZH)
            // Itt kell a fordított mapping, amikor a Controller fogadja az adatot
            CreateMap<CreateZhDto, Zh>();

            CreateMap<GeneraltZh, GeneraltZhResponseDto>();

            CreateMap<Hallgato, HallgatoDto>().ReverseMap(); // Oda-vissza mapping (Dto -> Entity és Entity -> Dto)

            // Megoldas Mappingok
            CreateMap<MegoldasInputDto, Megoldas>();
            CreateMap<Megoldas, MegoldasResponseDto>();

            CreateMap<Ertekeles, ErtekelesResponseDto>().ReverseMap();
            CreateMap<Feladat, FeladatDto>().ReverseMap();
        }
    }
}
