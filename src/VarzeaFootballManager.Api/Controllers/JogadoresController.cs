using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VarzeaFootballManager.Api.ViewModels.Jogadores;
using VarzeaFootballManager.Domain.Core;
using VarzeaFootballManager.Domain.Jogadores;

namespace VarzeaFootballManager.Api.Controllers
{
    /// <summary>
    /// Jogadores Controller
    /// </summary>
    [Route("api/v1/jogadores")]
    public class JogadoresController : Controller
    {
        /// <summary>
        /// Repositorio de Jogador
        /// </summary>
        private readonly IRepository<Jogador> _repositorioJogador;

        /// <summary>
        /// Constructor of <see cref="JogadoresController"/>
        /// </summary>
        /// <param name="repositorioJogador">Repositorio de Jogador</param>
        public JogadoresController(IRepository<Jogador> repositorioJogador)
        {
            _repositorioJogador = repositorioJogador;
        }

        /// <summary>
        /// Get Jogadores
        /// </summary>
        [HttpGet("", Name = "GetAllJogadores")]
        [ProducesResponseType(typeof(IEnumerable<JogadorGetAllDetailsViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var jogadores = await Task.Run(() => _repositorioJogador.FindAll().ToList());

            var result = jogadores.Select(jogador => new JogadorGetAllDetailsViewModel
            {
                Id = jogador.Id,
                Nome = jogador.Nome,
                Idade = jogador.Idade,
                Nivel = jogador.Nivel,
                Posicao = jogador.Posicao
            }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Get Jogador by Id
        /// </summary>
        /// <param name="id">Identifier</param>
        [HttpGet("{id}", Name = "GetJogadorById")]
        [ProducesResponseType(typeof(JogadorGetSingleViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { message = $"O id='{id}' é inválido!" });

            var jogador = await Task.Run(() => _repositorioJogador.Get(id));

            return Ok(new JogadorGetSingleViewModel
            {
                Id = jogador.Id,
                CreatedAt = jogador.CreatedAt,
                ModifiedAt = jogador.ModifiedAt,
                Nome = jogador.Nome,
                Idade = jogador.Idade,
                Posicao = jogador.Posicao,
                Nivel = jogador.Nivel
            });
        }

        /// <summary>
        /// Save Jogador
        /// </summary>
        /// <param name="viewModel">Data to save</param>
        [HttpPost("")]
        [ProducesResponseType(typeof(JogadorGetSingleViewModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody]JogadorPostViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var jogador = new Jogador
            {
                Nome = viewModel.Nome,
                Idade = viewModel.Idade,
                Nivel = viewModel.Nivel,
                Posicao = viewModel.Posicao
            };

            await Task.Run(() => _repositorioJogador.Insert(jogador));

            return CreatedAtRoute("GetJogadorById", new { id = jogador.Id }, new JogadorGetSingleViewModel
            {
                Id = jogador.Id,
                CreatedAt = jogador.CreatedAt,
                ModifiedAt = jogador.ModifiedAt,
                Nome = jogador.Nome,
                Idade = jogador.Idade,
                Posicao = jogador.Posicao,
                Nivel = jogador.Nivel,
            });
        }

        /// <summary>
        /// Update specific Jogador
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="viewModel">Data to update</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(JogadorGetSingleViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Put(string id, [FromBody]JogadorPutViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { message = $"O id='{id}' é inválido!" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var jogador = await Task.Run(() => _repositorioJogador.Get(id));

            if (jogador == null)
                return NotFound();

            jogador.Nome = viewModel.Nome;
            jogador.Idade = viewModel.Idade;
            jogador.Nivel = viewModel.Nivel;
            jogador.Posicao = viewModel.Posicao;

            _repositorioJogador.Replace(jogador);
            
            return Ok(new JogadorGetSingleViewModel
            {
                Id = jogador.Id,
                CreatedAt = jogador.CreatedAt,
                ModifiedAt = jogador.ModifiedAt,
                Nome = jogador.Nome,
                Idade = jogador.Idade,
                Posicao = jogador.Posicao,
                Nivel = jogador.Nivel,
            });
        }

        /// <summary>
        /// Delete specific Jogador
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Returns a value of type <see cref="string"/></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { message = $"O id='{id}' é inválido!" });

            await Task.Run(() => _repositorioJogador.Delete(id));

            return Ok($"Jogador {id} removido com sucesso!");
        }
    }
}
