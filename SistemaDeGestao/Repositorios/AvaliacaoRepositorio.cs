using Microsoft.EntityFrameworkCore;
using SistemaDeGestao.Data;
using SistemaDeGestao.Models;
using SistemaDeGestao.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaDeGestao.Repositorios
{
    public class AvaliacaoRepositorio : IAvaliacaoRepositorio
    {
        private readonly SistemaTarefasDBContext _dbContext;

        public AvaliacaoRepositorio(SistemaTarefasDBContext sistemaTarefasDBContext)
        {
            _dbContext = sistemaTarefasDBContext;
        }

        public async Task<AvaliacaoModel> BuscarPorId(int id)
        {
            return await _dbContext.Avaliacoes
                .Include(x => x.Id)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<AvaliacaoModel>> BuscarTodasAvaliacao()
        {
            return await _dbContext.Avaliacoes
                .ToListAsync();
        }

        public async Task<AvaliacaoModel> Adicionar(AvaliacaoModel avaliacaoModel)
        {
            try { 
                await _dbContext.Avaliacoes.AddAsync(avaliacaoModel);
                await _dbContext.SaveChangesAsync();
                return avaliacaoModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AvaliacaoModel> Atualizar(AvaliacaoModel avaliacao, int id)
        {
            AvaliacaoModel avaliacaoPorId = await BuscarPorId(id);

            if (avaliacaoPorId == null)
                throw new Exception($"Avaliação para o ID: {id} não foi encontrada no banco de dados.");

            avaliacaoPorId.Nome = avaliacao.Nome;
            avaliacaoPorId.Status = avaliacao.Status;
            avaliacaoPorId.Descricao = avaliacao.Descricao;
            avaliacaoPorId.Usuario = avaliacao.Usuario;

            _dbContext.Avaliacoes.Update(avaliacaoPorId);
            await _dbContext.SaveChangesAsync();

            return avaliacaoPorId;
        }

        public async Task<bool> Apagar(int id)
        {
            AvaliacaoModel avaliacaoPorId = await BuscarPorId(id);

            if (avaliacaoPorId == null)
            {
                throw new Exception($"Avaliação para o ID: {id} não foi encontrada no banco de dados.");
            }

            _dbContext.Avaliacoes.Remove(avaliacaoPorId);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
