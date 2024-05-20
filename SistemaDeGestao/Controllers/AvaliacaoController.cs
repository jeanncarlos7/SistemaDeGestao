﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SistemaDeGestao.Models;
using SistemaDeGestao.Repositorios;
using SistemaDeGestao.Repositorios.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaDeGestao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacaoRepositorio _avaliacaoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public AvaliacaoController(IAvaliacaoRepositorio avaliacaoRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            _avaliacaoRepositorio = avaliacaoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<AvaliacaoModel>>> BuscarAvaliacoes()
        {
            List<AvaliacaoModel> avaliacoes = await _avaliacaoRepositorio.BuscarTodasAvaliacao();
            return Ok(avaliacoes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AvaliacaoModel>> BuscarPorId(int id)
        {
            AvaliacaoModel avaliacao = await _avaliacaoRepositorio.BuscarPorId(id);
            if (avaliacao == null)
            {
                return NotFound();
            }
            return Ok(avaliacao);
        }

        [HttpPost]
        public async Task<ActionResult<AvaliacaoModel>> Cadastrar([FromBody] AvaliacaoModelInsert avaliacaoModelInsert)
        {
            try
            {
                if (avaliacaoModelInsert == null)
                    return BadRequest();

                var usuarioExistente = await _usuarioRepositorio.ObterPorId(avaliacaoModelInsert.UsuarioId);
                if (usuarioExistente == null)
                    return BadRequest("O usuário fornecido não existe.");

                var avaliacaoModel = new AvaliacaoModel
                {
                    Nome = avaliacaoModelInsert.Nome,
                    Descricao = avaliacaoModelInsert.Descricao,
                    Status = avaliacaoModelInsert.Status,
                    Email = avaliacaoModelInsert.Email,
                    UsuarioId = usuarioExistente.Id,
                };

                AvaliacaoModel avaliacao = await _avaliacaoRepositorio.Adicionar(avaliacaoModel);

                return CreatedAtAction(nameof(BuscarPorId), new { id = avaliacao.Id }, avaliacao);
            }
            catch(Exception ex) {
                if (ex.InnerException is DbUpdateException dbUpdateException)
                {
                    var sqlException = dbUpdateException.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        var number = sqlException.Number;
                        var message = sqlException.Message;

                        return BadRequest($"Erro ao salvar as alterações no banco de dados: {message}");
                    }
                }
                return BadRequest(ex.Message); 
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AvaliacaoModel>> Atualizar(int id, [FromBody] AvaliacaoModel avaliacaoModel)
        {
            if (avaliacaoModel == null || avaliacaoModel.Id != id)
            {
                return BadRequest();
            }

            AvaliacaoModel avaliacaoAtualizada = await _avaliacaoRepositorio.Atualizar(avaliacaoModel, id);
            if (avaliacaoAtualizada == null)
            {
                return NotFound();
            }
            return Ok(avaliacaoAtualizada);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Apagar(int id)
        {
            bool apagado = await _avaliacaoRepositorio.Apagar(id);
            if (!apagado)
            {
                return NotFound();
            }
            return Ok(apagado);
        }
    }
}
