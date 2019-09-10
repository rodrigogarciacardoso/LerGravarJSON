using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace CSharp_Json
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo d = new DirectoryInfo(@"C:\Users\Fulltime\Desktop\Parceiros");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.json"); //Getting Text files
            foreach (FileInfo file in Files)
            {
                LerArquivoJson($@"C:\Users\Fulltime\Desktop\Parceiros\{file.Name}");
            }
        }

        static void LerArquivoJson(string arquivo)
        {
            using (StreamReader r = new StreamReader(arquivo))
            {
                string json = r.ReadToEnd();
                if (!json.Equals("Gerando pessoas, por favor aguarde..."))
                {
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
                            CNPJ_CPF = RemoverCaracteresEsepeciais(item.cpf),
                            Pessoa = EnumTipoPessoa.Fisica,
                            RG = RemoverCaracteresEsepeciais(item.rg),
                            Nome = item.nome,
                            Situacao = EnumSituacao.Ativo,
                            EnderecoPrincipal = new ParceiroViewModel.EnderecoSaveViewModel
                            {
                                UF = item.estado,
                                Cidade = item.cidade,
                                CodigoIBGE = ExecutarApi.ExecutarApiJSON<retEndereco>(Metodo.GET, null, headers, parameters, "https://viacep.com.br/ws/", $"{RemoverCaracteresEsepeciais(item.cep)}/json/").Data.ibge,
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
                            EmpresaId = 170
                        };

                        var result = ExecutarApi.ExecutarApiJSON<object>(Metodo.POST, parceiro, headers, parameters, apiURLWS, "Parceiro");
                        System.Console.WriteLine(item.nome);
                    }
                }
            }
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
