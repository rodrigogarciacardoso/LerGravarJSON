using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace CSharp_Json
{
    class ExecutarApi
    {
        public static RetornoAPIViewModel<T> ExecutarApiJSON<T>(
            Metodo metodo, object body,
            List<KeyValuePair<string, string>> headers,
            List<KeyValuePair<string, string>> parameters,
            string host, string url, bool NaoAdicionarHeaders = false)
        {
            var client = new RestClient(host)
            {
                Timeout = 999999999
            };

            var request = new RestRequest(url, (Method)metodo);
            request.Parameters.Clear();

            if (!NaoAdicionarHeaders)
            {
                request.AddHeader("Accept", "application/json; odata=verbose");
                request.AddHeader("Content-Type", "application/json; odata=verbose");
            }

            request.RequestFormat = DataFormat.Json;

            if (body != null)
            {
                request.AddJsonBody(body);
            }

            // apiKey, secretKey e outros headers opcionais
            if (headers != null && headers.Count > 0)
            {
                foreach (var itemHeader in headers.ToArray())
                {
                    request.AddHeader(itemHeader.Key.ToString(), itemHeader.Value != null ? itemHeader.Value.ToString() : "");
                }
            }

            // contribui para o PUT ou cadeia de consulta URL com base no método
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var itemParameter in parameters.ToArray())
                {
                    request.AddParameter(itemParameter.Key.ToString(), itemParameter.Value != null ? itemParameter.Value.ToString() : ""); // parâmetros diversos, de PUT principalmente.
                }
            }

            IRestResponse response = client.Execute(request); // executar a requisição
            var retorno = new RetornoAPIViewModel<T>
            {
                Status = (int)response.StatusCode,
                Content = response.Content
            };

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                retorno.Success = true;
                try
                {
                    retorno.Data = JsonConvert.DeserializeObject<T>(response.Content);
                }
                catch { }
            }
            else
            {
                retorno.Success = false;
                retorno.Data = default(T);
            }

            return retorno;
        }
    }
}
