using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LEGAL2
{
    public class DatabaseConnection
    {
        private MySqlConnection connection;

        public DatabaseConnection(string connectionString)
        {
            connection = new MySqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }

    public class RHOperations
    {
        private MySqlConnection connection;

        public RHOperations(DatabaseConnection dbConnection)
        {
            connection = dbConnection.GetConnection();
        }

        public List<Funcionario> GetFuncionarios()
        {
            List<Funcionario> funcionarios = new List<Funcionario>();
            string query = "SELECT id_funcionario, Nome, CPF, Cargo, Salario, DataAdmissao, DataDemissao FROM Funcionario";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Funcionario funcionario = new Funcionario
                {
                    Id = Convert.ToInt32(reader["id_funcionario"]),
                    Nome = reader["Nome"].ToString(),
                    CPF = reader["CPF"].ToString(),
                    Cargo = reader["Cargo"].ToString(),
                    Salario = Convert.ToDecimal(reader["Salario"]),
                    DataAdmissao = Convert.ToDateTime(reader["DataAdmissao"]),
                    DataDemissao = reader["DataDemissao"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DataDemissao"])
                };
                funcionarios.Add(funcionario);
            }
            reader.Close();
            return funcionarios;
        }

        public decimal CalcularINSS(decimal salario)
        {
            decimal aliquota = 0.11m;
            return salario * aliquota;
        }

        public decimal CalcularFGTS(decimal salario)
        {
            return salario * 0.08m;
        }

        public int CalcularTempoDeCasa(DateTime dataAdmissao, DateTime? dataDemissao)
        {
            DateTime fim = dataDemissao ?? DateTime.Now;
            int tempoDeCasa = fim.Year - dataAdmissao.Year;
            if (fim.Month < dataAdmissao.Month || (fim.Month == dataAdmissao.Month && fim.Day < dataAdmissao.Day))
            {
                tempoDeCasa--;
            }
            return tempoDeCasa;
        }

        public void DemitirFuncionario(int idFuncionario)
        {
            string query = "SELECT Nome, CPF, Cargo, Salario, DataAdmissao FROM Funcionario WHERE id_funcionario = @id";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", idFuncionario);
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string nome = reader["Nome"].ToString();
                string cpf = reader["CPF"].ToString();
                string cargo = reader["Cargo"].ToString();
                decimal salario = Convert.ToDecimal(reader["Salario"]);
                DateTime dataAdmissao = Convert.ToDateTime(reader["DataAdmissao"]);
                reader.Close();

                decimal inss = CalcularINSS(salario);
                decimal fgts = CalcularFGTS(salario);
                int tempoDeCasa = CalcularTempoDeCasa(dataAdmissao, null);

                Console.WriteLine($"Nome: {nome}, CPF: {cpf}, Cargo: {cargo}, Salario: {salario:C}, INSS: {inss:C}, FGTS: {fgts:C}, Tempo de Casa: {tempoDeCasa} anos");
                Console.WriteLine($"Total de INSS: {inss:C}");
                Console.WriteLine($"Total de FGTS: {fgts:C}");

                string updateQuery = "UPDATE Funcionario SET DataDemissao = @dataDemissao WHERE id_funcionario = @id";
                cmd = new MySqlCommand(updateQuery, connection);
                cmd.Parameters.AddWithValue("@dataDemissao", DateTime.Now);
                cmd.Parameters.AddWithValue("@id", idFuncionario);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Funcionario demitido e removido do sistema.");
            }
            else
            {
                reader.Close();
                Console.WriteLine("Funcionario nao encontrado.");
            }
        }
    }

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

    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Cargo { get; set; }
        public decimal Salario { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; }
    }

    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Lote { get; set; }
        public decimal CustoUnitario { get; set; }
        public int Unidade { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "server=localhost;uid=root;pwd=1234;database=Projeto";
            DatabaseConnection dbConnection = new DatabaseConnection(connectionString);

            try
            {
                dbConnection.OpenConnection();
                Console.WriteLine("Conexão estabelecida");

                RHOperations rhOps = new RHOperations(dbConnection);
                EstoqueOperations estoqueOps = new EstoqueOperations(dbConnection);

            
                List<Funcionario> funcionarios = rhOps.GetFuncionarios();
                foreach (var func in funcionarios)
                {
                    decimal inss = rhOps.CalcularINSS(func.Salario);
                    decimal fgts = rhOps.CalcularFGTS(func.Salario);
                    int tempoDeCasa = rhOps.CalcularTempoDeCasa(func.DataAdmissao, func.DataDemissao);
                    Console.WriteLine($"Nome: {func.Nome}, CPF: {func.CPF}, Cargo: {func.Cargo}, Salario: {func.Salario:C}, INSS: {inss:C}, FGTS: {fgts:C}, Tempo de Casa: {tempoDeCasa} anos");
                }

               
                List<Produto> produtos = estoqueOps.GetProdutos();
                foreach (var prod in produtos)
                {
                    decimal custoTotal = estoqueOps.CalcularCustoTotal(prod);
                    Console.WriteLine($"ID Produto: {prod.Id}, Nome: {prod.Nome}, Lote: {prod.Lote}, Custo Unitário: {prod.CustoUnitario:C}, Unidade: {prod.Unidade}, Custo Total: {custoTotal:C}");
                }

            
                rhOps.DemitirFuncionario(1);

                dbConnection.CloseConnection();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1045)
                {
                    Console.WriteLine("Erro de autenticação: verifique as credenciais do banco de dados.");
                }
                else
                {
                    Console.WriteLine("ERRO GERADO: " + ex.Number);
                    Console.WriteLine("Entre em contato com o administrador.");
                }
            }
            finally
            {
                if (dbConnection.GetConnection().State == System.Data