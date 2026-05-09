using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoPreco.UnitTests.Builders
{
    public class CustomerBuilder
    {
        public static Customer Build()
        {
            return new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Guilherme",
                Email = "gui@email.com"
            };
        }
    }
}
