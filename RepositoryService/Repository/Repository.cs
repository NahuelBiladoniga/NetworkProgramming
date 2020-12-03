using System;
using System.Collections.Generic;
using Domain;
using Repositories.Interfaces;

namespace Repositories
{
    public static class Repository
    {
        public static List<User> Users { get; set; } = new List<User>();
    }
}
