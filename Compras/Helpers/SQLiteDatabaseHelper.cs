using Compras.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq; // Adicionado para usar o .Where() na lista
using System.Threading.Tasks;

namespace Compras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> InsertProduto(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        public Task<List<Produto>> UpdateProduto(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao = ?, Quantidade = ?, Preco = ? WHERE Id = ?";
            _conn.QueryAsync<Produto>(sql, p.Descricao, p.Quantidade, p.Preco, p.Id);
            return null;
        }

        public Task<int> DeleteProduto(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE '%" + q + "%'";
            return _conn.QueryAsync<Produto>(sql);
        }


        // --- MÉTODO CORRIGIDO ---
        public async Task<List<Produto>> GetProdutosByDateRange(DateTime dataInicio, DateTime dataFim)
        {
            // 1. Pega TODOS os produtos do banco de dados e traz para a memória.
            var todosOsProdutos = await _conn.Table<Produto>().ToListAsync();

            // 2. Agora, com a lista em memória, filtra usando o LINQ do C#, onde o .Date funciona perfeitamente.
            var produtosFiltrados = todosOsProdutos
                                    .Where(p => p.DataCadastro.Date >= dataInicio.Date && p.DataCadastro.Date <= dataFim.Date)
                                    .ToList();

            return produtosFiltrados;
        }
    }
}