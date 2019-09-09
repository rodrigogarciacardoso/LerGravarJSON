using System;

namespace CSharp_Json
{
    public class ParceiroViewModel
    {
        public class ParceiroSaveViewModel
        {
            private string ie;

            public long Id { get; set; }
            public long EmpresaId { get; set; }
            public string CNPJ_CPF { get; set; }
            public EnumTipoPessoa Pessoa { get; set; }
            public string RG { get; set; }
            public string IM { get; set; }
            public string IE
            {
                get
                {
                    return ie;
                }
                set
                {
                    if (value != null)
                        ie = value.ToUpper();
                    else
                        ie = "";
                }
            }
            public string Nome { get; set; }
            public string NomeFantasia { get; set; }
            public string Codigo { get; set; }
            public EnumSituacao Situacao { get; set; }
            public long? RamoAtividadeId { get; set; }
            public long? RegiaoId { get; set; }
            public long? GrupoContatoId { get; set; }
            public DateTime DataCadastro { get; set; }
            public EnderecoSaveViewModel EnderecoPrincipal { get; set; }
            public ContatoSaveViewModel ContatoPrincipal { get; set; }
        }

        public class EnderecoSaveViewModel
        {
            public long Id { get; set; }
            public long ParceiroId { get; set; }
            public EnumTipoEndereco Tipo { get; set; }
            public string DescricaoEnderecoOutros { get; set; }
            public string UF { get; set; }
            public string Cidade { get; set; }
            public string CodigoIBGE { get; set; }
            public string Cep { get; set; }
            public string Endereco { get; set; }
            public string Numero { get; set; }
            public string Complemento { get; set; }
            public string Bairro { get; set; }
            public long? CidadeEstrangeiraId { get; set; }
        }

        public class ContatoSaveViewModel
        {
            public long Id { get; set; }
            public string Nome { get; set; }
            public string Email { get; set; }
            public PhoneContato Telefone { get; set; }
            public PhoneContato Telefone2 { get; set; }
            public PhoneContato Celular { get; set; }
            public EnumTipoContato Tipo { get; set; }
            public long ParceiroId { get; set; }
            public bool Nf { get; set; }
            public bool Boleto { get; set; }
            public bool Favorito { get; set; }
        }

        public class PhoneContato
        {
            public string DDD { get; set; }
            public string DDI { get; set; }
            public string Phone { get; set; }
        }
    }
}
