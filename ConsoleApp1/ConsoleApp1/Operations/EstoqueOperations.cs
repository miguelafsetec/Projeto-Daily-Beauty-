using LEGAL2.Database;
using LEGAL2.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LEGAL2.Operations
{
    public class EstoqueOperations
    {
        private MySqlConnection connection;

        public EstoqueOperations(DatabaseConnection dbConnection)
        {
            connection = dbConnection.GetConnection();
        }

        public List<Produto> GetProdutos()
        {
            List<Produto> produtos = new List<Produto>();
            string query = "SELECT id_Produto, Nome_do_Produto, Nº_do_lote, Custo_Unitario, Unidade FROM Produto ORDER BY id_Produto ASC";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Produto produto = new Produto
                {
                    Id = Convert.ToInt32(reader["id_Produto"]),
                    Nome = reader["Nome_do_Produto"].ToString(),
                    Lote = Convert.ToInt32(reader["Nº_do_lote"]),
                    CustoUnitario = Convert.ToDecimal(reader["Custo_Unitario"]),
                    Unidade = Convert.ToInt32(reader["Unidade"])
                };
                produtos.Add(produto);
            }
            reader.Close();

            return produtos;
        }

        public decimal CalcularCustoTotal(Produto produto)
        {
            return produto.CustoUnitario * produto.Unidade;
        }

        public void AdicionarProduto(Produto produto)
        {
            string query = "INSERT INTO Produto (Nome_do_Produto, Nº_do_lote, Custo_Unitario, Unidade) VALUES (@nome, @lote, @custo, @unidade)";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@nome", produto.Nome);
            cmd.Parameters.AddWithValue("@lote", produto.Lote);
            cmd.Parameters.AddWithValue("@custo", produto.CustoUnitario);
            cmd.Parameters.AddWithValue("@unidade", produto.Unidade);
            cmd.ExecuteNonQuery();

            Console.WriteLine("Produto adicionado ao estoque.");
        }

        public void RemoverProduto(int idProduto)
        {
            string query = "DELETE FROM Produto WHERE id_Produto = @id";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", idProduto);
            cmd.ExecuteNonQuery();

            Console.WriteLine("Produto removido do estoque.");
        }
    }
}
