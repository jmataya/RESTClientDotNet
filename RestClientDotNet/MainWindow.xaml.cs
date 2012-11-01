using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Net;
using System.IO;

namespace RestClientDotNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubmitRequest_Click(object sender, RoutedEventArgs e)
        {
            // First, clear the current response.
            this.ResultTextBox.Text = String.Empty;

            // Second, get all of the different parameters.
            String requestType = this.RequestType.Text;
            String contentType = this.ContentType.Text;
            String baseUrl = this.UrlTextBox.Text;
            String pathUrl = this.PathTextBox.Text;
            String parameters = this.ParametersTextBox.Text;

            // Third, craft the request.
            Uri baseUri = new Uri(baseUrl);
            Uri requestUrl = new Uri(baseUri, pathUrl);
            HttpWebRequest request = WebRequest.CreateHttp(requestUrl);
            request.Method = requestType;

            if (contentType == "JSON")
            {
                request.ContentType = "application/json";
            }

            if (requestType != "GET")
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(parameters);
                request.ContentLength = data.Length;
                Stream contentStream = request.GetRequestStream();
                contentStream.Write(data, 0, data.Length);
                contentStream.Close();
            }

            try
            {
                // Execute the request.
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                String responseContent = reader.ReadToEnd();
                this.ResultTextBox.Text = responseContent;
            }
            catch (Exception exp)
            {
                this.ResultTextBox.Text = exp.Message + exp.StackTrace;
            }
        }
    }
}
