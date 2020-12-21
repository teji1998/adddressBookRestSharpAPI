using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace AdddressBookRestSharpMSTest
{
    [TestClass]
    public class RestSharpTest
    {
        RestClient client;

        [TestInitialize]
        public void setUp()
        {
            client = new RestClient(" http://localhost:4000");
        }

        /// <summary>
        /// Gets the address book list.
        /// </summary>
        /// <returns></returns>
        private IRestResponse getPersonList()
        {
            //arrange
            RestRequest request = new RestRequest("/Person", Method.GET);
            //act
            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// Givens the request when calling the get API should return the person.
        /// </summary>
        [TestMethod]
        public void givenRequest_WhenCallingTheGetAPI_shouldReturnThePerson()
        {
            IRestResponse response = getPersonList();
            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            //deserialize the response
            List<Person> dataResponse = JsonConvert.DeserializeObject<List<Person>>(response.Content);
            Assert.AreEqual(1, dataResponse.Count);
        }
    }
}

