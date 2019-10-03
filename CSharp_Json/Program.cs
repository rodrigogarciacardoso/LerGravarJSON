using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace CSharp_Json
{
    class Program
    {
        public static int contador = 1;
        public static RetornoAPIViewModel<object> result;
        public static string apiURLWS = "http://homologacao.fullcontrol.com.br:8077/";
        //public static string apiURLWS = "http://localhost:5000/";
        public static List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>();
        public static List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

        static void Main(string[] args)
        {
            ObterTokenAsync();

            DirectoryInfo d = new DirectoryInfo(@"C:\Users\Fulltime\Desktop\Parceiros");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.json"); //Getting Text files
            foreach (FileInfo file in Files)
            {
                LerArquivoJson($@"C:\Users\Fulltime\Desktop\Parceiros\{file.Name}");
                File.Delete($@"C:\Users\Fulltime\Desktop\Parceiros\{file.Name}");
            }
        }

        static void LerArquivoJson(string arquivo)
        {
            using (StreamReader r = new StreamReader(arquivo))
            {
                string json = r.ReadToEnd();
                if (json != "Gerando pessoas, por favor aguarde...")
                {
                    var lista = JsonConvert.DeserializeObject<List<json>>(json);
                    ParceiroViewModel.ParceiroSaveViewModel parceiro;

                    foreach (var item in lista)
                    {
                        parceiro = new ParceiroViewModel.ParceiroSaveViewModel
                        {
                            CNPJ_CPF = RemoverCaracteresEsepeciais(item.cpf),
                            Pessoa = EnumTipoPessoa.Fisica,
                            RG = RemoverCaracteresEsepeciais(item.rg),
                            Nome = item.nome,
                            Situacao = EnumSituacao.Ativo,
                            EnderecoPrincipal = new ParceiroViewModel.EnderecoSaveViewModel
                            {
                                UF = item.estado,
                                Cidade = item.cidade,
                                //CodigoIBGE = ExecutarApi.ExecutarApiJSON<retEndereco>(Metodo.GET, null, headers, parameters, "https://viacep.com.br/ws/", $"{RemoverCaracteresEsepeciais(item.cep)}/json/").Data.ibge,
                                Cep = RemoverCaracteresEsepeciais(item.cep),
                                Endereco = item.endereco,
                                Numero = item.numero.ToString(),
                                Bairro = item.bairro
                            },
                            ContatoPrincipal = new ParceiroViewModel.ContatoSaveViewModel
                            {
                                Email = item.email,
                                Telefone = new ParceiroViewModel.PhoneContato
                                {
                                    DDI = "55",
                                    DDD = RemoverCaracteresEsepeciais(item.telefone_fixo).Substring(0, 2),
                                    Phone = RemoverCaracteresEsepeciais(item.telefone_fixo).Substring(3, 8)
                                },
                                Celular = new ParceiroViewModel.PhoneContato
                                {
                                    DDI = "55",
                                    DDD = RemoverCaracteresEsepeciais(item.celular).Substring(0, 2),
                                    Phone = RemoverCaracteresEsepeciais(item.celular).Substring(3, 9)
                                }
                            },
                            EmpresaId = 347
                        };

                        CallApi(parceiro);
                    }
                }

            }
        }

        static void CallApi(ParceiroViewModel.ParceiroSaveViewModel parceiro)
        {
            result = ExecutarApi.ExecutarApiJSON<object>(Metodo.POST, parceiro, headers, parameters, apiURLWS, "Parceiro");

            if (result.Status == 200)
                Console.WriteLine($"{parceiro.Nome} - {contador++} - ({DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")})");

            else if (result.Status == 401)
            {
                ObterTokenAsync();
                CallApi(parceiro);
            }
        }

        static void ObterTokenAsync()
        {
            headers.Clear();
            parameters.Clear();

            parameters.Add(new KeyValuePair<string, string>("DDI", "55"));
            parameters.Add(new KeyValuePair<string, string>("Telefone", "14991912061"));
            parameters.Add(new KeyValuePair<string, string>("CodigoAtivacao", "39414")); //Homolog
            //parameters.Add(new KeyValuePair<string, string>("CodigoAtivacao", "29803")); //Local

            var ret = ExecutarApi.ExecutarApiJSON<RetornoApi>(Metodo.GET, null, headers, parameters, apiURLWS, "Account/Token");
            headers.Add(new KeyValuePair<string, string>("Authorization", $"Bearer {ret.Data.Entidade.token}"));
        }

        static string RemoverCaracteresEsepeciais(string cpfCnpj)
        {
            string pattern = @"(?i)[^0-9a-záéíóúàèìòùâêîôûãõç\s]";
            string replacement = "";
            Regex rgx = new Regex(pattern);
            return rgx.Replace(cpfCnpj, replacement);
        }

    }

}
