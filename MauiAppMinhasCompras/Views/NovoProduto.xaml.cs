using MauiAppMinhasCompras.Models;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace MauiAppMinhasCompras.Views
{
    public partial class NovoProduto : ContentPage
    {
        public List<string> Categorias { get; set; } = new List<string>
        {
            "Alimentos",
            "Bebidas",
            "Limpeza",
            "Higiene",
            "Eletrônicos",
            "Roupas",
            "Outros"
        };

        public NovoProduto()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(txt_descricao.Text) ||
                    !double.TryParse(txt_quantidade.Text, out double quantidade) ||
                    !double.TryParse(txt_preco.Text, out double preco) ||
                    pickerCategoria.SelectedItem == null)
                {
                    await DisplayAlert("Erro", "Preencha todos os campos corretamente", "OK");
                    return;
                }

                Produto p = new Produto
                {
                    Descricao = txt_descricao.Text,
                    Quantidade = quantidade,
                    Preco = preco,
                    Categoria = pickerCategoria.SelectedItem.ToString()
                };

                await App.Db.Insert(p);
                await DisplayAlert("Sucesso!", "Produto cadastrado", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }


    }
    }

