namespace VarzeaFootballManager.Domain.Jogadores
{
    public class Jogador : Core.AggregateRoot
    {
        public string Nome { get; set; }
        public int Idade { get; set; }
        public Nivel Nivel { get; set; }
        public Posicao Posicao { get; set; }
    }
}
