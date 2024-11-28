using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Vai_porfavor
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
            string query = "SELECT id_Funcionario, Nome, CPF, Cargo, Salário, DataAdmissao FROM Funcionario";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Funcionario funcionario = new Funcionario
                {
                    Id = Convert.ToInt32(reader["id_Funcionario"]),
                    Nome = reader["Nome"].ToString(),
                    CPF = reader["CPF"].ToString(),
                    Cargo = reader["Cargo"].ToString(),
                    Salário = Convert.ToDecimal(reader["Salário"]),
                    DataAdmissao = Convert.ToDateTime(reader["DataAdmissao"])
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

        public int CalcularTempoDeCasa(DateTime dataAdmissao)
        {
            return DateTime.Now.Year - dataAdmissao.Year;
        }

        public void DemitirFuncionario(int idFuncionario)
        {
            string query = "SELECT Nome, CPF, Cargo, Salário, DataAdmissao FROM Funcionario WHERE id_Funcionario = @id";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", idFuncionario);
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string nome = reader["Nome"].ToString();
                string cpf = reader["CPF"].ToString();
                string cargo = reader["Cargo"].ToString();
                decimal salario = Convert.ToDecimal(reader["Salário"]);
                DateTime dataAdmissao = Convert.ToDateTime(reader["DataAdmissao"]);
                reader.Close();

                decimal inss = CalcularINSS(salario);
                decimal fgts = CalcularFGTS(salario);
                int tempoDeCasa = CalcularTempoDeCasa(dataAdmissao);

                Console.WriteLine($"Nome: {nome}, CPF: {cpf}, Cargo: {cargo}, Salário: {salario:C}, INSS: {inss:C}, FGTS: {fgts:C}, Tempo de Casa: {tempoDeCasa} anos");
                Console.WriteLine($"Total de INSS: {inss:C}");
                Console.WriteLine($"Total de FGTS: {fgts:C}");

                string deleteQuery = "DELETE FROM Funcionario WHERE id_Funcionario = @id";
                cmd = new MySqlCommand(deleteQuery, connection);
                cmd.Parameters.AddWithValue("@id", idFuncionario);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Funcionário demitido e removido do sistema.");
            }
            else
            {
                reader.Close();
                Console.WriteLine("Funcionário não encontrado.");
            }
        }
    }
}
