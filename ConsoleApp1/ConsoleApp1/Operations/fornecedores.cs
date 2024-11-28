using LEGAL2.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Operations
{
    internal class fornecedores
    {
        public class FornecedorOperations
        {
            private MySqlConnection connection;

            public FornecedorOperations(DatabaseConnection dbConnection)
            {
                connection = dbConnection.GetConnection();
            }

            public class Fornecedor
            {
                public int Id { get; set; }
                public string Nome { get; set; }
                public string Endereco { get; set; }
                public string CNPJ { get; set; }
                public string Email { get; set; }
                public string Telefone { get; set; }
                public int QuantosAnosDeContrato { get; set; }
            }

            public List<Fornecedor> GetFornecedores()
            {
                List<Fornecedor> fornecedores = new List<Fornecedor>();
                string query = "SELECT * FROM Fornecedores";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = null;

                try
                {
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Fornecedor fornecedor = new Fornecedor
                        {
                            Id = Convert.ToInt32(reader["Id_Fornecedor"]),
                            Nome = reader["nome"].ToString(),
                            Endereco = reader["endereço"].ToString(),
                            CNPJ = reader["CNPJ"].ToString(),
                            Email = reader["email"].ToString(),
                            Telefone = reader["Telefone"].ToString(),
                            QuantosAnosDeContrato = Convert.ToInt32(reader["quantos_anos_de_contrato"])
                        };
                        fornecedores.Add(fornecedor);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao obter fornecedores: " + ex.Message);
                }
                finally
                {
                    reader?.Close();
                }

                return fornecedores;
            }

            public void AdicionarFornecedor(Fornecedor fornecedor)
            {
                string query = "INSERT INTO fornecedores (nome, endereço, CNPJ, email, telefone, quantos_anos_de_contrato) VALUES (@nome, @endereco, @cnpj, @email, @telefone, @quantosAnosDeContrato)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@nome", fornecedor.Nome);
                cmd.Parameters.AddWithValue("@endereco", fornecedor.Endereco);
                cmd.Parameters.AddWithValue("@cnpj", fornecedor.CNPJ);
                cmd.Parameters.AddWithValue("@email", fornecedor.Email);
                cmd.Parameters.AddWithValue("@telefone", fornecedor.Telefone);
                cmd.Parameters.AddWithValue("@quantosAnosDeContrato", fornecedor.QuantosAnosDeContrato);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Fornecedor adicionado ao sistema.");
            }
        }
    }
}
