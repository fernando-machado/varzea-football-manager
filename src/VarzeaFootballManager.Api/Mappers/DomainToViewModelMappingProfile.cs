using AutoMapper;
using System.Collections.Generic;
using VarzeaFootballManager.Api.ViewModels.Jogadores;
using VarzeaFootballManager.Domain.Jogadores;

namespace VarzeaFootballManager.Api.Mappers
{
    /// <summary>
    /// Map view models to domain classes
    /// </summary>
    public class DomainToViewModelMappingProfile : Profile
    {
        /// <summary>
        /// Profile Name
        /// </summary>
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        /// <summary>
        /// Constructor of <see cref="DomainToViewModelMappingProfile"/>
        /// </summary>
        public DomainToViewModelMappingProfile()
        {
            this.CreateMap<Jogador, JogadorGetAllDetailsViewModel>();
            this.CreateMap<Jogador, JogadorGetSingleViewModel>();
            this.CreateMap<IEnumerable<Jogador>, JogadorGetAllViewModel>()
                .ConstructUsing(source => new JogadorGetAllViewModel { Jogadores = Mapper.Map<IEnumerable<JogadorGetAllDetailsViewModel>>(source) });
        }
    }
}
