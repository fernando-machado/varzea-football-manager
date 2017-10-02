using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VarzeaFootballManager.Api.ViewModels.Jogadores;
using VarzeaFootballManager.Domain.Core;
using VarzeaFootballManager.Domain.Jogadores;
using AutoMapper;

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
        private readonly IRepositoryAsync<Jogador> _repositorioJogador;

        /// <summary>
        /// Constructor of <see cref="JogadoresController"/>
        /// </summary>
        /// <param name="repositorioJogador">Repositorio de Jogador</param>
        public JogadoresController(IRepositoryAsync<Jogador> repositorioJogador)
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
            var jogadores = await _repositorioJogador.FindAllAsync();

            var result = Mapper.Map<JogadorGetAllViewModel>(jogadores);
            
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

            var jogador = await _repositorioJogador.GetAsync(id);

            var result = Mapper.Map<JogadorGetSingleViewModel>(jogador);

            //var result = new JogadorGetSingleViewModel
            //{
            //    Id = jogador.Id,
            //    CreatedAt = jogador.CreatedAt,
            //    ModifiedAt = jogador.ModifiedAt,
            //    Nome = jogador.Nome,
            //    Idade = jogador.Idade,
            //    Posicao = jogador.Posicao,
            //    Nivel = jogador.Nivel,
            //};

            return Ok(result);
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
            
            var jogador = Mapper.Map<Jogador>(viewModel);

            await _repositorioJogador.InsertAsync(jogador);

            var result = Mapper.Map<JogadorGetSingleViewModel>(jogador);
            
            return base.CreatedAtRoute("GetJogadorById", new { id = jogador.Id }, result);
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

            var jogador = await _repositorioJogador.GetAsync(id);

            if (jogador == null)
                return NotFound();

            await _repositorioJogador.UpdateAsync(jogador,
                upd => upd.Set(j => j.Nome, viewModel.Nome),
                upd => upd.Set(j => j.Idade, viewModel.Idade),
                upd => upd.Set(j => j.Nivel, viewModel.Nivel),
                upd => upd.Set(j => j.Posicao, viewModel.Posicao)
            );

            await _repositorioJogador.UpdateAsync(jogador, new[]
            {
                _repositorioJogador.Updater.Set(j => j.Nome, viewModel.Nome),
                _repositorioJogador.Updater.Set(j => j.Idade, viewModel.Idade),
                _repositorioJogador.Updater.Set(j => j.Nivel, viewModel.Nivel),
                _repositorioJogador.Updater.Set(j => j.Posicao, viewModel.Posicao)
            });

            var result = Mapper.Map<JogadorGetSingleViewModel>(jogador);

            return Ok(result);
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

            await _repositorioJogador.DeleteAsync(id);

            return Ok($"Jogador {id} removido com sucesso!");
        }
    }
}
