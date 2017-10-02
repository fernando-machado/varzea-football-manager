using System.Collections.Generic;

namespace VarzeaFootballManager.Api.ViewModels.Jogadores
{
    /// <summary>
    /// 
    /// </summary>
    public class JogadorGetAllViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<JogadorGetAllDetailsViewModel> Jogadores { get; set; }
    }
}