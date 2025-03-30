using MauiAppMinhasCompras.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace MauiAppMinhasCompras.Views
{
    public partial class ListaProduto : ContentPage
    {
        ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

        public ListaProduto()
        {
            InitializeComponent();
            lst_produtos.ItemsSource = lista;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                lista.Clear();

                List<Produto> tmp = await App.Db.GetAll();
                tmp.ForEach(i => lista.Add(i));

                AtualizarCategorias(tmp);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private void AtualizarCategorias(List<Produto> produtos)
        {
            var categorias = produtos.Select(p => p.Categoria)
                                     .Where(c => !string.IsNullOrEmpty(c))
                                     .Distinct()
                                     .ToList();
            pickerCategoria.ItemsSource = categorias;
            pickerCategoria.SelectedIndex = -1;
        }

        private async void ToolbarItem_Novo_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new NovoProduto());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string q = e.NewTextValue;
                lista.Clear();
                List<Produto> tmp = await App.Db.Search(q);
                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void PickerCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pickerCategoria.SelectedIndex == -1)
                    return;

                string categoriaSelecionada = pickerCategoria.SelectedItem.ToString();
                lista.Clear();

                List<Produto> tmp = await App.Db.SearchByCategory(categoriaSelecionada);
                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void ToolbarItem_Total_Clicked(object sender, EventArgs e)
        {
            try
            {
                double soma = lista.Sum(i => i.Total);
                string msg = $"O total é: {soma:C}";
                await DisplayAlert("Total dos Produtos", msg, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is MenuItem menuItem && menuItem.BindingContext is Produto produtoSelecionado)
                {
                    bool confirmar = await DisplayAlert("Remover", $"Deseja remover {produtoSelecionado.Descricao}?", "Sim", "Não");

                    if (confirmar)
                    {
                        await App.Db.Delete(produtoSelecionado.Id);
                        lista.Remove(produtoSelecionado);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    

        private async void ToolbarItem_Relatorio_Clicked(object sender, EventArgs e)
        {
            try
            {
                var todosProdutos = await App.Db.GetAll();
                var grupoCategoria = todosProdutos
                    .GroupBy(p => p.Categoria)
                    .Select(g => new
                    {
                        Categoria = g.Key,
                        TotalCategoria = g.Sum(x => x.Total)
                    }).ToList();

                string relatorio = "";
                foreach (var grupo in grupoCategoria)
                {
                    relatorio += $"Categoria: {grupo.Categoria} - Total: {grupo.TotalCategoria:C}\n";
                }

                await DisplayAlert("Relatório por Categoria", relatorio, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null)
                    return;

                Produto p = e.SelectedItem as Produto;
                await Navigation.PushAsync(new EditarProduto
                {
                    BindingContext = p,
                });
                lst_produtos.SelectedItem = null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}
