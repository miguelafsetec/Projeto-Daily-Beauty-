create database Projeto;
use projeto;

drop table Produto;
select*from Produto;
CREATE TABLE Produto (
  id_Produto INTEGER UNSIGNED NOT NULL,
  Nome_do_Produto VARCHAR(45) NOT NULL,
  Nº_do_lote INT NOT NULL,
  Custo_Unitario decimal NOT NULL,	
  Unidade INT NOT NULL,
  PRIMARY KEY(id_Produto)
);
drop table Funcionario;
select*from funcionario;
CREATE TABLE funcionario (
  id_funcionario INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  Nome  VARCHAR(100) NOT NULL,
  CEP VARCHAR(11) NOT NULL,
  CPF VARCHAR(14) NOT NULL,
  Email VARCHAR(50) NOT NULL,
  celular  VARCHAR(10) NOT NULL,
  Salario decimal NOT NULL,
  Cargo VARCHAR(45) NOT NULL,
  Departamento VARCHAR(45) NOT NULL,
  DataAdmissao DATE NOT NULL,
  DataDemissao DATE,
  PRIMARY KEY(id_funcionario)
);
drop table fornecedores;
select*from fornecedores;
		create table fornecedores(
		Id_Fornecedor int not null,
		nome varchar (47) not null,
		endereco varchar (100) not null,
		CNPJ varchar (18) not null,
		email varchar (100) not null,
		Telefone varchar(100) not null,
		quantos_anos_de_contrato int not null,
		primary key (Id_Fornecedor)
		);


