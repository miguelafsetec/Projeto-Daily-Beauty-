using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using LEGAL2.Database;
using LEGAL2.Operations;
using LEGAL2.Models;

namespace LEGAL2
{
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
