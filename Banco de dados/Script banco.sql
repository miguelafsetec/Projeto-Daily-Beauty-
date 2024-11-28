	create database db_Daily;
	use db_Daily;

	CREATE TABLE Produto (
	  id_Produto int primary key auto_increment,
	  Nome_do_Produto VARCHAR(45) NOT NULL,
	  Nº_do_lote INT NOT NULL,
	  Custo_Unitario decimal NOT NULL,
	  Unidade INT NOT NULL
	);

	create table fornecedores(
	Id_Fornecedor int primary key auto_increment,
	nome varchar (47) not null,
	endereço varchar (100) not null,
	CNPJ varchar (18) not null,
	email varchar (100) not null,
	Telefone varchar(100) not null,
	quantos_anos_de_contrato int not null
	);


	CREATE TABLE funcionario (
	  id_funcionario int primary key auto_increment,
	  Nome  VARCHAR(100) NOT NULL,
	  CEP VARCHAR(11) NOT NULL,
	  CPF VARCHAR(14) NOT NULL,
	  Email VARCHAR(50) NOT NULL,
	  celular  VARCHAR(10) NOT NULL,
	  Salario decimal NOT NULL,
	  Cargo VARCHAR(45) NOT NULL,
	  Departamento VARCHAR(45) NOT NULL,
	  DataAdmissao DATE NOT NULL,
	  DataDemissao DATE
	);

	INSERT INTO Produto (Nome_do_Produto, Nº_do_lote, Custo_Unitario, Unidade) VALUES
	('Parafuso', 1001, 0.25, 500),
	('Porca', 1002, 0.15, 1000),
	('Arruela', 1003, 0.10, 2000),
	('Chave de Fenda', 1004, 5.50, 150),
	('Martelo', 1005, 10.00, 100);

	INSERT INTO funcionario (Nome, CEP, CPF, Email, celular, Salario, Cargo, Departamento, DataAdmissao, DataDemissao) VALUES
	('João Silva', '12345-678', '123.456.789-00', 'joao.silva@example.com', '999123456', 2500.00, 'Operador', 'Produção', '2022-01-15', NULL),
	('Maria Oliveira', '23456-789', '234.567.890-01', 'maria.oliveira@example.com', '999234567', 3200.00, 'Analista', 'Logística', '2021-03-10', NULL),
	('Carlos Pereira', '34567-890', '345.678.901-02', 'carlos.pereira@example.com', '999345678', 2800.00, 'Supervisor', 'Manutenção', '2019-07-22', '2023-11-01'),
	('Ana Souza', '45678-901', '456.789.012-03', 'ana.souza@example.com', '999456789', 4500.00, 'Gerente', 'RH', '2018-05-18', NULL),
	('Lucas Costa', '56789-012', '567.890.123-04', 'lucas.costa@example.com', '999567890', 3000.00, 'Técnico', 'TI', '2020-11-25', NULL);


	INSERT INTO fornecedores (nome, endereco, CNPJ, email, telefone, quantos_anos_de_contrato) VALUES ('Gabriel','teste','4325232','gabriel@tetse.com','11953422',1)