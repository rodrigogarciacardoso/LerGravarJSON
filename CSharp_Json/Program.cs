using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace CSharp_Json
{
    class Program
    {
        static void Main(string[] args)
        {
            LerArquivoJson(@"C:\Users\Fulltime\Desktop\data.json");
        }

        static void LerStringJson()
        {
            JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            string json = @"{ ""nome"" : ""Jose Carlos"", ""sobrenome"" : ""Macoratti"", ""email"": ""macoratti@yahoo.com"" }";

            dynamic resultado = serializer.DeserializeObject(json);

            Console.WriteLine("  ==  Resultado da leitura do arquivo JSON  == ");
            Console.WriteLine("");

            foreach (KeyValuePair<string, object> entry in resultado)
            {
                var key = entry.Key;
                var value = entry.Value as string;
                Console.WriteLine(String.Format("{0} : {1}", key, value));
            }

        }

        static void LerArquivoJson(string arquivo)
        {
            using (StreamReader r = new StreamReader(arquivo))
            {
                string json = r.ReadToEnd();
                var obj = JsonConvert.DeserializeObject<json>(json);

                var apiURLWS = "http://localhost:5000/";

                var headers = new List<KeyValuePair<string, string>>();
                var parameters = new List<KeyValuePair<string, string>>();
                headers.Clear();
                parameters.Clear();

                var result = ExecutarApi.ExecutarApiJSON<object>(Metodo.POST, obj, headers, parameters, apiURLWS, "Parceiro");
            }
        }

    }

}
