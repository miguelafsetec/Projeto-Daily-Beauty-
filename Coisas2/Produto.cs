﻿namespace Vai_porfavor
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Lote { get; set; }
        public decimal CustoUnitario { get; set; }
        public int Unidade { get; set; }
        public DateTime DataValidade { get; set; }
    }
}
