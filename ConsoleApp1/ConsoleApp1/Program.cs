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

                Console.WriteLine($"Nome: {nome}, CPF: {cpf}, Cargo: {cargo}, Salário: {salario:C}");
                Console.WriteLine($"INSS: {inss:C}, FGTS: {fgts:C}, Tempo de Casa: {tempoDeCasa} anos");

                string updateQuery = "UPDATE Funcionario SET DataDemissao = @dataDemissao WHERE id_funcionario = @id";
                cmd = new MySqlCommand(updateQuery, connection);
                cmd.Parameters.AddWithValue("@dataDemissao", DateTime.Now);
                cmd.Parameters.AddWithValue("@id", idFuncionario);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Funcionário demitido e atualizado no sistema.");
            }
            else
            {
                reader.Close();
                Console.WriteLine("Funcionário não encontrado.");
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
            string connectionString = "server=localhost;uid=root;pwd=emigab;database=db_Daily";
            DatabaseConnection dbConnection = new DatabaseConnection(connectionString);

            try
            {
                dbConnection.OpenConnection();
                Console.WriteLine("Conexão estabelecida");

                RHOperations rhOps = new RHOperations(dbConnection);
                EstoqueOperations estoqueOps = new EstoqueOperations(dbConnection);

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("=== MENU PRINCIPAL ===");
                    Console.WriteLine("1. Listar Funcionários");
                    Console.WriteLine("2. Calcular INSS e FGTS de Funcionários");
                    Console.WriteLine("3. Demitir Funcionário");
                    Console.WriteLine("4. Listar Produtos");
                    Console.WriteLine("5. Adicionar Produto");
                    Console.WriteLine("6. Remover Produto");
                    Console.WriteLine("0. Sair");
                    Console.Write("Escolha uma opção: ");

                    string opcao = Console.ReadLine();

                    switch (opcao)
                    {
                        case "1":
                            ListarFuncionarios(rhOps);
                            break;
                        case "2":
                            CalcularInssFgts(rhOps);
                            break;
                        case "3":
                            DemitirFuncionario(rhOps);
                            break;
                        case "4":
                            ListarProdutos(estoqueOps);
                            break;
                        case "5":
                            AdicionarProduto(estoqueOps);
                            break;
                        case "6":
                            RemoverProduto(estoqueOps);
                            break;
                        case "0":
                            Console.WriteLine("Saindo do sistema...");
                            return;
                        default:
                            Console.WriteLine("Opção inválida, tente novamente.");
                            break;
                    }

                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
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
                if (dbConnection.GetConnection() != null && dbConnection.GetConnection().State == System.Data.ConnectionState.Open)
                {
                    dbConnection.CloseConnection();
                    Console.WriteLine("Conexão fechada.");
                }
            }
        }

        static void ListarFuncionarios(RHOperations rhOps)
        {
            Console.WriteLine("=== Lista de Funcionários ===");
            var funcionarios = rhOps.GetFuncionarios();

            foreach (var funcionario in funcionarios)
            {
                string status = funcionario.DataDemissao.HasValue ? "Demitido" : "Ativo";
                Console.WriteLine($"ID: {funcionario.Id}, Nome: {funcionario.Nome}, CPF: {funcionario.CPF}, Cargo: {funcionario.Cargo}, Salário: {funcionario.Salario:C}, Status: {status}");
            }
        }

        static void CalcularInssFgts(RHOperations rhOps)
        {
            Console.WriteLine("Digite o ID do funcionário para calcular INSS e FGTS:");
            int idFuncionario = int.Parse(Console.ReadLine());
            var funcionarios = rhOps.GetFuncionarios();

            var funcionario = funcionarios.Find(f => f.Id == idFuncionario);
            if (funcionario != null)
            {
                decimal inss = rhOps.CalcularINSS(funcionario.Salario);
                decimal fgts = rhOps.CalcularFGTS(funcionario.Salario);
                Console.WriteLine($"Funcionário: {funcionario.Nome}, INSS: {inss:C}, FGTS: {fgts:C}");
            }
            else
            {
                Console.WriteLine("Funcionário não encontrado.");
            }
        }

        static void DemitirFuncionario(RHOperations rhOps)
        {
            Console.WriteLine("Digite o ID do funcionário que deseja demitir:");
            int idFuncionario = int.Parse(Console.ReadLine());
            rhOps.DemitirFuncionario(idFuncionario);
        }

        static void ListarProdutos(EstoqueOperations estoqueOps)
        {
            Console.WriteLine("=== Lista de Produtos ===");
            var produtos = estoqueOps.GetProdutos();

            foreach (var produto in produtos)
            {
                Console.WriteLine($"ID: {produto.Id}, Nome: {produto.Nome}, Lote: {produto.Lote}, Custo Unitário: {produto.CustoUnitario:C}, Unidade: {produto.Unidade}");
            }
        }

        static void AdicionarProduto(EstoqueOperations estoqueOps)
        {
            Produto novoProduto = new Produto();

            Console.WriteLine("Digite o nome do produto:");
            novoProduto.Nome = Console.ReadLine();

            Console.WriteLine("Digite o número do lote:");
            novoProduto.Lote = int.Parse(Console.ReadLine());

            Console.WriteLine("Digite o custo unitário:");
            novoProduto.CustoUnitario = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Digite a quantidade de unidades:");
            novoProduto.Unidade = int.Parse(Console.ReadLine());

            estoqueOps.AdicionarProduto(novoProduto);
        }

        static void RemoverProduto(EstoqueOperations estoqueOps)
        {
            Console.WriteLine("Digite o ID do produto que deseja remover:");
            int idProduto = int.Parse(Console.ReadLine());
            estoqueOps.RemoverProduto(idProduto);
        }
    }
}
