using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace PerfBenchmarks
{
    [Config(typeof(BenchConfig))]
    [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
    public class LinqWhereTests
    {
        private HashSet<string> _verifiedCustomerNames;
        private HashSet<string> _cerifiedCustomerNames;
        private HashSet<string> _aListCustomerNames;
        private Customer[] _customers;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _verifiedCustomerNames = new HashSet<string> { "AAA", "DDD", "EEE" };
            _cerifiedCustomerNames = new HashSet<string> { "FFF", "GGG", "HHH" };
            _aListCustomerNames = new HashSet<string> { "III", "JJJ", "KKK" };

            _customers = new[]
            {
               new Customer("aaa", "Laaaa"),
               new Customer("bbb", "Lbbbb"),
               new Customer("ccc", "Lcccc"),
            };
        }

        [Benchmark(Baseline = true)]
        public List<Customer> GetUsingMultipleToUpper()
        {
            return _customers
                .Where(c =>
                    _verifiedCustomerNames.Contains(c.FirstName.ToUpper())
                    || _cerifiedCustomerNames.Contains(c.FirstName.ToUpper())
                    || _aListCustomerNames.Contains(c.FirstName.ToUpper()))
                .ToList();
        }

        [Benchmark]
        public List<Customer> GetUsingMultipleEqualityComparer()
        {
            return _customers
                .Where(c =>
                    _verifiedCustomerNames.Contains(c.FirstName.ToUpper(), StringComparer.OrdinalIgnoreCase)
                    || _cerifiedCustomerNames.Contains(c.FirstName.ToUpper(), StringComparer.OrdinalIgnoreCase)
                    || _aListCustomerNames.Contains(c.FirstName.ToUpper(), StringComparer.OrdinalIgnoreCase))
                .ToList();
        }

        [Benchmark]
        public List<Customer> GetUsingMergeSetsAndSingleToUpper()
        {
            _verifiedCustomerNames.UnionWith(_cerifiedCustomerNames);
            _verifiedCustomerNames.UnionWith(_aListCustomerNames);

            return _customers
                .Where(c => _verifiedCustomerNames.Contains(c.FirstName.ToUpper()))
                .ToList();
        }

        [Benchmark]
        public List<Customer> GetUsingConcatAndMultipleToUpper()
        {
            return _customers.Where(c => _verifiedCustomerNames
                    .Concat(_cerifiedCustomerNames)
                    .Concat(_aListCustomerNames)
                    .Contains(c.FirstName.ToUpper()))
                .ToList();
        }

        [Benchmark]
        public List<Customer> GetUsingQueryExpressionLetAndOneToUpper()
        {
            return (from customer in _customers
                    let uppercaseFirstName = customer.FirstName.ToUpper()
                    where _verifiedCustomerNames.Contains(uppercaseFirstName)
                        || _cerifiedCustomerNames.Contains(uppercaseFirstName)
                        || _aListCustomerNames.Contains(uppercaseFirstName)
                    select customer).ToList();
        }

        [Benchmark]
        public List<Customer> GetUsingLocalvariableAssignmentForToUpper()
        {
            return (
                _customers
                    .Where(c =>
                    {
                        var firstNameUppered = c.FirstName.ToUpper();
                        return _verifiedCustomerNames.Contains(firstNameUppered)
                        || _cerifiedCustomerNames.Contains(firstNameUppered)
                        || _aListCustomerNames.Contains(firstNameUppered);
                    })).ToList();
        }

        [Benchmark]
        public List<Customer> GetWithouhLINQ()
        {
            var matchingCustomers = new List<Customer>();

            foreach (var customer in _customers)
            {
                var firstNameUppered = customer.FirstName.ToUpper();
                if(_verifiedCustomerNames.Contains(firstNameUppered)
                    || _cerifiedCustomerNames.Contains(firstNameUppered)
                    || _aListCustomerNames.Contains(firstNameUppered))
                {
                    matchingCustomers.Add(customer);
                }
            }

            return matchingCustomers;
        }

        [Benchmark]
        public List<Customer> GetWithouLINQNoToUpperForLoop()
        {
            var matchingCustomers = new List<Customer>();

            for (int i = 0; i < _customers.Length; i++)
            {
                var customer = _customers[i];
                if (_verifiedCustomerNames.Contains(customer.FirstName)
                    || _cerifiedCustomerNames.Contains(customer.FirstName)
                    || _aListCustomerNames.Contains(customer.FirstName))
                    matchingCustomers.Add(customer);
            }

            return matchingCustomers;
        }

        public class Customer
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public Customer(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }
        }
    }
}
