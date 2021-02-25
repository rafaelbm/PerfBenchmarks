using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using Bogus;
using Newtonsoft.Json;

namespace PerfBenchmarks
{
    [Config(typeof(BenchConfig))]
    public class JsonDeserialization
    {
        private ArraySegment<byte> _arraySegment;

        [Params(1, 100)]
        public int CustomersQuantity { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            Randomizer.Seed = new Random(42);

            var addressFaker = new Faker<Address>();
            addressFaker.RuleFor(c => c.City, (f, c) => f.Address.City());
            addressFaker.RuleFor(c => c.Region, (f, c) => f.Address.State());
            addressFaker.RuleFor(c => c.PostalCode, (f, c) => f.Address.ZipCode());
            addressFaker.RuleFor(c => c.Country, (f, c) => f.Address.Country());
            addressFaker.RuleFor(c => c.Phone, (f, c) => f.Phone.PhoneNumber());
            addressFaker.RuleFor(c => c.Fax, (f, c) => f.Phone.PhoneNumber());

            var customerFaker = new Faker<Customer>();
            customerFaker.RuleFor(c => c.CustomerID, (f, c) => f.Random.AlphaNumeric(20));
            customerFaker.RuleFor(c => c.CompanyName, (f, c) => f.Company.CompanyName());
            customerFaker.RuleFor(c => c.ContactName, (f, c) => f.Person.FirstName);
            customerFaker.RuleFor(c => c.ContactTitle, (f, c) => f.Person.UserName);
            customerFaker.RuleFor(c => c.Addresses, (f, c) => addressFaker.Generate(100).ToList());

            var customers = customerFaker.Generate(CustomersQuantity);
            var customerAsJson = JsonConvert.SerializeObject(customers);

            _arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(customerAsJson));
        }

        [Benchmark(Baseline = true)]
        public Customer[] NewtonJsonDeserializeFromJsonString()
        {
            var array = _arraySegment.ToArray();
            var json = Encoding.UTF8.GetString(array);

            return JsonConvert.DeserializeObject<Customer[]>(json);
        }

        [Benchmark]
        public Customer[] JsonSerializerDeserializeFromJsonString()
        {
            var array = _arraySegment.ToArray();
            var json = Encoding.UTF8.GetString(array);

            return System.Text.Json.JsonSerializer.Deserialize<Customer[]>(json);
        }

        [Benchmark]
        public Customer[] NewtonJsonDeserializeFromJsonTextReader()
        {
            using var memoryStream = new MemoryStream(_arraySegment.Array, _arraySegment.Offset, _arraySegment.Count);
            using var streamReader = new StreamReader(memoryStream, Encoding.UTF8);
            using var jsonReader = new JsonTextReader(streamReader);

            var serializer = JsonSerializer.Create();
            return serializer.Deserialize<Customer[]>(jsonReader);
        }


        [Benchmark]
        public Customer[] JsonSerializerDeserializeFromArraySegment()
        {
            return System.Text.Json.JsonSerializer.Deserialize<Customer[]>(_arraySegment);
        }


        [Benchmark]
        public Customer[] JsonSerializerDeserializeFromArraySegmentFromArray()
        {
            return System.Text.Json.JsonSerializer.Deserialize<Customer[]>(_arraySegment.Array);
        }
    }

    public class Customer
    {
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public List<Address> Addresses { get; set; }

    }

    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}
