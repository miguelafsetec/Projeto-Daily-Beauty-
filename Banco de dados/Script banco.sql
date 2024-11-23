create database Projeto;
use projeto;

drop table Produto;

CREATE TABLE Produto (
  id_Produto INTEGER UNSIGNED NOT NULL,
  Nome_do_Produto VARCHAR(45) NOT NULL,
  Nº_do_lote INT NOT NULL,
  Custo_Unitario decimal NOT NULL,
  Unidade INT NOT NULL,
  PRIMARY KEY(idProduto)
);
drop table Funcionario;

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