using System.Collections.Generic;

namespace CSharp_Json
{
    public class RetornoApi
    {
        public RetornoApi()
        {
            Erros = new List<ErrorRetornoViewModel>();
        }
        public string Id { get; set; }
        public bool Successo { get; set; }
        public string ErroCodigo { get; set; }
        public string ErroDescricao { get; set; }
        public TokenViewModel Entidade { get; set; }
        public IList<ErrorRetornoViewModel> Erros { get; set; }
    }

    public class ErrorRetornoViewModel
    {
        public string descricao { get; set; }
    }
}
