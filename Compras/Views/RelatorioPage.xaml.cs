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
            // 1. Pega as datas selecionadas pelo usu�rio nos DatePickers.
            DateTime dataInicio = pckDataInicio.Date;
            DateTime dataFim = pckDataFim.Date;

            if (dataInicio > dataFim)
            {
                await DisplayAlert("Aten��o", "A data de in�cio n�o pode ser maior que a data final.", "OK");
                return;
            }

            // 2. Chama o novo m�todo do banco de dados para buscar os produtos.
            List<Produto> lista_produtos = await App.Db.GetProdutosByDateRange(dataInicio, dataFim);

            // 3. Verifica se algum produto foi encontrado.
            if (lista_produtos.Any())
            {
                // 4. Exibe a lista de produtos na CollectionView.
                cv_produtos.ItemsSource = lista_produtos;
            }
            else
            {
                // Limpa resultados anteriores e avisa o usu�rio.
                cv_produtos.ItemsSource = null;
                await DisplayAlert("Nenhum resultado", "N�o foram encontrados produtos para o per�odo selecionado.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}