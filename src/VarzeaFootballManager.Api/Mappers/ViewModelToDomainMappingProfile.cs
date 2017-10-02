using VarzeaFootballManager.Api.ViewModels.Jogadores;
using VarzeaFootballManager.Domain.Jogadores;

namespace VarzeaFootballManager.Api.Mappers
{
    /// <summary>
    /// Map view models to domain classes
    /// </summary>
    public class ViewModelToDomainMappingProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Profile Name
        /// </summary>
        public override string ProfileName
        {
            get { return "ViewModelToDomainMapping"; }
        }

        /// <summary>
        /// Constructor of <see cref="ViewModelToDomainMappingProfile"/>
        /// </summary>
        public ViewModelToDomainMappingProfile()
        {
            this.CreateMap<JogadorPostViewModel, Jogador>();
            this.CreateMap<JogadorPutViewModel, Jogador>();
        }
    }
}
