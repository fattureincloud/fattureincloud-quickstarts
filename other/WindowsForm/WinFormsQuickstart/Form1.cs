using It.FattureInCloud.Sdk.Api;
using It.FattureInCloud.Sdk.Client;
using It.FattureInCloud.Sdk.Model;
using System;
using System.Windows.Forms;

namespace WinFormsQuickstart
{
    public partial class Form : System.Windows.Forms.Form
    {
        ClientsApi clientsApi;
        int companyId = 12345;
        public Form()
        {
            InitializeComponent();
            Configuration config = new Configuration();
            // IMPORTANT: the access token file must be saved in a secure environment
            // like a db or the AppConfig
            config.AccessToken = "YOUR_ACCESS_TOKEN";
            clientsApi = new ClientsApi(config);
        }

        private void syncButton_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();
                dataGridView1.Columns.Add("Id", "Id");
                dataGridView1.Columns.Add("Name", "Name");
                dataGridView1.Columns.Add("VatNumber", "Vat Number");
                dataGridView1.Columns.Add("TaxCode", "Tax Code");
                dataGridView1.Columns.Add("Country", "Country");

                retrieveAllClients();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void retrieveAllClients(int page = 1)
        {
            var result = clientsApi.ListClientsWithHttpInfo(companyId, page: page, perPage: 5);

            result.Data.Data.ForEach(client =>
            {
                dataGridView1.Rows.Add(new string[] { client.Id.ToString(), client.Name, client.VatNumber, client.TaxCode, client.Country });
            });

            if (result.Data.NextPageUrl != null) retrieveAllClients(++page);
        }

        private void saveClient_Click(object sender, EventArgs e)
        {
            var newClient = new CreateClientRequest(
                data: new ModelClient(
                    name: clientNameTextBox.Text,
                    taxCode: clientTaxCodeTextBox.Text,
                    vatNumber: clientVatNumberTextBox.Text,
                    email: clientEmailTextBox.Text
                )
            );

            try
            {
                var result = clientsApi.CreateClient(companyId, newClient);
                MessageBox.Show("Client save succesfully with id: " + result.Data.Id.ToString());
                clientNameTextBox.Text = String.Empty;
                clientTaxCodeTextBox.Text = String.Empty;
                clientVatNumberTextBox.Text = String.Empty;
                clientEmailTextBox.Text = String.Empty;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
