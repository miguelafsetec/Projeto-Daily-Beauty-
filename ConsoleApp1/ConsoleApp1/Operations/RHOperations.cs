using LEGAL2.Database;
using LEGAL2.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LEGAL2.Operations
{
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
}
