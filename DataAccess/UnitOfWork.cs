using Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata;

namespace DataAccess
{
    public class UnitOfWork
    {
        public IEnumerable<Client> Clients { get; }
        public IEnumerable<Comment> Comments { get; }
        public IEnumerable<Photo> Photos { get; }
    }
}
