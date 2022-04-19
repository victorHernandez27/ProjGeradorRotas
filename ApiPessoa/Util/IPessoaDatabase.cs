namespace ApiPessoa.Util
{
    public interface IPessoaDatabase
    {
        string PessoaCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
