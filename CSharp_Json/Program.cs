using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CSharp_Json
{
    class Program
    {
        static void Main(string[] args)
        {
            LerArquivoJson(@"C:\Users\Fulltime\Desktop\data.json");
        }

        static void LerArquivoJson(string arquivo)
        {
            using (StreamReader r = new StreamReader(arquivo))
            {
                string json = r.ReadToEnd();
                var lista = JsonConvert.DeserializeObject<List<json>>(json);

                var apiURLWS = "http://localhost:5000/";
                var headers = new List<KeyValuePair<string, string>>();
                var parameters = new List<KeyValuePair<string, string>>();

                ParceiroViewModel.ParceiroSaveViewModel parceiro;

                foreach (var item in lista)
                {
                    headers.Clear();
                    parameters.Clear();

                    parceiro = new ParceiroViewModel.ParceiroSaveViewModel
                    {
                        CNPJ_CPF = item.cpf
                    };

                    var result = ExecutarApi.ExecutarApiJSON<object>(Metodo.POST, item, headers, parameters, apiURLWS, "Parceiro");
                }
            }
        }

    }

}
