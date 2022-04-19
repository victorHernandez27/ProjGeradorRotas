namespace ApiCidade.Util
{
    public class CidadeDatabase : ICidadeDatabase
    {
        public string CidadeCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
