using MySql.Data.MySqlClient;
using System;

public class DatabaseConnection
{
    private string _connectionString;
    private MySqlConnection _connection;

    // Construtor que define a string de conexão
    public DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
        _connection = new MySqlConnection(_connectionString);
    }

    // Método para abrir a conexão
    public void OpenConnection()
    {
        try
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
                Console.WriteLine("Conexão aberta com sucesso.");
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Erro ao abrir a conexão: " + ex.Message);
        }
    }

    // Método para fechar a conexão
    public void CloseConnection()
    {
        try
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
                Console.WriteLine("Conexão fechada com sucesso.");
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Erro ao fechar a conexão: " + ex.Message);
        }
    }

    // Método para obter a conexão aberta
    public MySqlConnection GetConnection()
    {
        return _connection;
    }

    // Método para verificar se a tabela existe
    public bool TabelaExiste(string nomeTabela)
    {
        OpenConnection();  // Abre a conexão
        using (MySqlCommand cmd = new MySqlCommand($"SHOW TABLES LIKE '{nomeTabela}'", _connection))
        {
            var result = cmd.ExecuteScalar();
            CloseConnection();  // Fecha a conexão
            return result != null;  // Se o resultado for nulo, a tabela não existe
        }
    }
}
