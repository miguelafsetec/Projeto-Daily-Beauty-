create database Projeto;
use projeto;
drop table Estoque;


select Nome_do_estoque as nome_estoque from Estoque;
CREATE TABLE Estoque (
  id_Estoque INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  Nome_do_estoque VARCHAR(45) NOT NULL,
  PRIMARY KEY(id_Estoque)
);

drop table CEP;

select*from CEP;

CREATE TABLE CEP (
  id_CEP INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  Estado VARCHAR(45) NOT NULL,
  Cidade VARCHAR(45) NOT NULL,
  Municipio VARCHAR(45) NOT NULL,
  Bairro VARCHAR(45) NOT NULL,
  Rua VARCHAR(45) NOT NULL,
  Numero INT NOT NULL,
  PRIMARY KEY(id_CEP)
);
drop table Produto;

CREATE TABLE Produto (
  id_Produto INTEGER UNSIGNED NOT NULL,
  Estoque_idEstoque INTEGER UNSIGNED NOT NULL,
  Nome_do_Produto VARCHAR(45) NOT NULL,
  Nº_do_lote INT NOT NULL,
  Custo_Unitario decimal NOT NULL,
  Unidade INT NOT NULL,
  PRIMARY KEY(idProduto),
  foreign key (Estoque_idEstoque) references  Estoque (id_estoque)
);
drop table Funcionario;

CREATE TABLE funcionario (
  id_funcionario INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  CEP_idCEP INTEGER UNSIGNED NOT NULL,
  Nome  VARCHAR(100) NOT NULL,
  CEP VARCHAR(11) NOT NULL,
  CPF VARCHAR(14) NOT NULL,
  Email VARCHAR(50) NOT NULL,
  celular  VARCHAR(10) NOT NULL,
  Salario decimal NOT NULL,
  Cargo VARCHAR(45) NOT NULL,
  Departamento VARCHAR(45) NOT NULL,
  PRIMARY KEY(id_funcionario),
  foreign key(CEP_idCEP)references cep (id_cep)
);

