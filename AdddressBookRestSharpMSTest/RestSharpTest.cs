using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            Assert.AreEqual(3, dataResponse.Count);
        }

        /// <summary>
        /// Givens the multiple persons when calling the postapi should return ok status.
        /// </summary>
        [TestMethod]
        public void givenMultiplePersons_WhenCallingThePOSTAPI_ShouldReturnOKStatus()
        {
            List<Person> addressBookList = new List<Person>();
            addressBookList.Add(new Person { Firstname = "Virat", Lastname = "Chavan", Address = "CH", City = "Nagpur", State = "Kerala", Zip = 509908, MobileNumber = "9034987656" });
            addressBookList.Add(new Person { Firstname = "Sai", Lastname = "Joshi", Address = "BPL", City = "Gadchiroli", State = "Gos", Zip = 609213, MobileNumber = "9934508976" });
            foreach (Person addressBook in addressBookList)
            {
                //arrange
                RestRequest request = new RestRequest("/Person", Method.POST);
                //creation of json object
                JObject jObjectbody = new JObject();
                jObjectbody.Add("Firstname", addressBook.Firstname);
                jObjectbody.Add("Lastname", addressBook.Lastname);
                jObjectbody.Add("Address", addressBook.Address);
                jObjectbody.Add("City", addressBook.City);
                jObjectbody.Add("State", addressBook.State);
                jObjectbody.Add("Zip", addressBook.Zip);
                jObjectbody.Add("PhoneNumber", addressBook.MobileNumber);
                request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);
                //act
                IRestResponse response = client.Execute(request);
                //assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                //deserializing the response
                Person dataResponse = JsonConvert.DeserializeObject<Person>(response.Content);
                Assert.AreEqual(addressBook.Firstname, dataResponse.Firstname);
            }
        }

        /// <summary>
        /// Givens the details to update when updated using put api should update the details.
        /// </summary>
        [TestMethod]
        public void givenDetailsToUpdate_WhenUpdatedUsingPUTAPI_ShouldUpdateTheDetails()
        {
            RestRequest request = new RestRequest("/Person/3", Method.PUT);
            //arrange
            JObject jObjectbody = new JObject();
            //creating a json object
            jObjectbody.Add("Firstname","Sai");
            jObjectbody.Add("Lastname", "Joshi");
            jObjectbody.Add("Address", "BPL");
            jObjectbody.Add("City", "Nagpur");
            jObjectbody.Add("State", "Goa");
            jObjectbody.Add("Zip", 609213);
            jObjectbody.Add("MobileNumber", "9934508976");
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);
            //act
            IRestResponse response = client.Execute(request);
            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Person dataResponse = JsonConvert.DeserializeObject<Person>(response.Content);
            Assert.AreEqual("Nagpur", dataResponse.City);
            Assert.AreEqual("Goa", dataResponse.State);
        }
    }
}

