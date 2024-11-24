using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Vai_porfavor
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
            string query = "SELECT id_Produto, Nome_do_Produto, Numero_do_Lote, Custo_Unitario, Unidade, DataValidade FROM Produto ORDER BY DataValidade ASC";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Produto produto = new Produto
                {
                    Id = Convert.ToInt32(reader["id_Produto"]),
                    Nome = reader["Nome_do_Produto"].ToString(),
                    Lote = Convert.ToInt32(reader["Numero_do_Lote"]),
                    CustoUnitario = Convert.ToDecimal(reader["Custo_Unitario"]),
                    Unidade = Convert.ToInt32(reader["Unidade"]),
                    DataValidade = Convert.ToDateTime(reader["DataValidade"])
                };
                produtos.Add(produto);
            }
            reader.Close();

            produtos.RemoveAll(prod => prod.DataValidade < DateTime.Now);
            return produtos;
        }

        public decimal CalcularCustoTotal(Produto produto)
        {
            return produto.CustoUnitario * produto.Unidade;
        }
    }
}
