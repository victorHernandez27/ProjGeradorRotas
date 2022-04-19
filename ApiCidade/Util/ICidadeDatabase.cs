namespace ApiCidade.Util
{
    public interface ICidadeDatabase
    {
        string CidadeCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
