using Domain;
using GestaoPreco.UnitTests.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoPreco.UnitTests.Mocks
{
    public static class CustomerMocks
    {
        public static List<Customer> GetList()
        {
            return new List<Customer>
        {
            CustomerBuilder.Build()
        };
        }
    }
}
