using SQLite;
using System;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        private string _descricao;
        private double _quantidade;
        private double _preco;
        private string _categoria;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao
        {
            get => _descricao;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Por favor, preencha a descrição.");

                _descricao = value;
            }
        }

        public double Quantidade
        {
            get => _quantidade;
            set
            {
                if (value < 0)
                    throw new Exception("A quantidade não pode ser negativa.");

                _quantidade = value;
            }
        }

        public double Preco
        {
            get => _preco;
            set
            {
                if (value < 0)
                    throw new Exception("O preço não pode ser negativo.");

                _preco = value;
            }
        }

        public string Categoria
        {
            get => string.IsNullOrWhiteSpace(_categoria) ? "Outros" : _categoria;
            set => _categoria = value;
        }

        public double Total => Quantidade * Preco;
    }
}
