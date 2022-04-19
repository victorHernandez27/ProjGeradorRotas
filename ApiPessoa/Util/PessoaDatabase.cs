namespace ApiPessoa.Util
{
    public class PessoaDatabase : IPessoaDatabase
    {
        public string PessoaCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
