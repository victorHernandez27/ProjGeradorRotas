﻿namespace ApiUsuario.Util
{
    public class UsuarioDatabase : IUsuarioDatabase
    {
        public string UsuarioCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
