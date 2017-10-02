using AutoMapper;

namespace VarzeaFootballManager.Api.Mappers
{
    /// <summary>
    /// Automapper config
    /// </summary>
    public class AutomapperConfig
    {
        /// <summary>
        /// Register mappings
        /// </summary>
        public static void RegisterMappings()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDomainMappingProfile>();
            });
        }
    }
}
