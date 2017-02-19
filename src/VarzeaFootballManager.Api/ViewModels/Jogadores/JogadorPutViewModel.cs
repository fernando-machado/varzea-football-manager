using VarzeaFootballManager.Domain.Jogadores;

namespace VarzeaFootballManager.Api.ViewModels.Jogadores
{
    /// <summary>
    /// 
    /// </summary>
    public class JogadorPutViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Idade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nivel Nivel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Posicao Posicao { get; set; }
    }
}