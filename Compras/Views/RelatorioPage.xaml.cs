using Compras.Models;

namespace Compras.Views;

public partial class RelatorioPage : ContentPage
{
    public RelatorioPage()
    {
        InitializeComponent();
    }

    private async void btnGerarRelatorio_Clicked(object sender, EventArgs e)
    {
        try
        {
            // 1. Pega as datas selecionadas pelo usuário nos DatePickers.
            DateTime dataInicio = pckDataInicio.Date;
            DateTime dataFim = pckDataFim.Date;

            if (dataInicio > dataFim)
            {
                await DisplayAlert("Atenção", "A data de início não pode ser maior que a data final.", "OK");
                return;
            }

            // 2. Chama o novo método do banco de dados para buscar os produtos.
            List<Produto> lista_produtos = await App.Db.GetProdutosByDateRange(dataInicio, dataFim);

            // 3. Verifica se algum produto foi encontrado.
            if (lista_produtos.Any())
            {
                // 4. Exibe a lista de produtos na CollectionView.
                cv_produtos.ItemsSource = lista_produtos;
            }
            else
            {
                // Limpa resultados anteriores e avisa o usuário.
                cv_produtos.ItemsSource = null;
                await DisplayAlert("Nenhum resultado", "Não foram encontrados produtos para o período selecionado.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}