using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Vai_porfavor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MySqlConnection conexao = null;

            try
            {
                
                string stringconexao = "server=localhost;uid=root;pwd=1234;database=Projeto";
                DatabaseConnection dbConnection = new DatabaseConnection(stringconexao);

                dbConnection.OpenConnection();  

                
                RHOperations rhOps = new RHOperations(dbConnection);
                EstoqueOperations estoqueOps = new EstoqueOperations(dbConnection);

                
                List<Funcionario> funcionarios = rhOps.GetFuncionarios();
                foreach (var func in funcionarios)
                {
                    
                    decimal inss = rhOps.CalcularINSS(func.Salário);
                    decimal fgts = rhOps.CalcularFGTS(func.Salário);
                    int tempoDeCasa = rhOps.CalcularTempoDeCasa(func.DataAdmissao);

                    
                    Console.WriteLine($"Nome: {func.Nome}, CPF: {func.CPF}, Cargo: {func.Cargo}, Salário: {func.Salário:C}, INSS: {inss:C}, FGTS: {fgts:C}, Tempo de Casa: {tempoDeCasa} anos");
                }

                
                List<Produto> produtos = estoqueOps.GetProdutos();
                foreach (var produto in produtos)
                {
                    
                    decimal custoTotal = estoqueOps.CalcularCustoTotal(produto);
                    Console.WriteLine($"Produto: {produto.Nome}, Lote: {produto.Lote}, Custo Unitário: {produto.CustoUnitario:C}, Custo Total: {custoTotal:C}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
            finally
            {
                
                if (conexao != null && conexao.State == System.Data.ConnectionState.Open)
                {
                    conexao.Close();
                    Console.WriteLine("Conexão fechada.");
                }
            }

            Console.ReadKey();
        }
    }
}
