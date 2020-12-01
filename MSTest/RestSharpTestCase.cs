using AddressBook_RestAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace MSTest
{
    [TestClass]
    public class RestSharpTestCase
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }

        /// <summary>
        /// Gets the employee list.
        /// </summary>
        /// <returns></returns>
        private IRestResponse GetContactList()
        {
            //arrange
            RestRequest request = new RestRequest("/addressBook", Method.GET);

            //act
            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// Called when [calling list return employee list].
        /// </summary>
        [TestMethod]
        public void OnCallingList_ReturnAddressBookList()
        {
            IRestResponse response = GetContactList();

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<AddressBook> dataResponse = JsonConvert.DeserializeObject<List<AddressBook>>(response.Content);
            int dataCount = 4;
            Assert.AreEqual(dataCount, dataResponse.Count);

            foreach (AddressBook address in dataResponse)
            {
                Console.WriteLine("id : " + address.id + " FirstName : " + address.FirstName + " LastName : " + address.LastName + 
                    " Address : " + address.Address + " City : " + address.City + " State : " + address.State + "  Zip :" + address.Zip
                    +" PhoneNumber : " + address.PhoneNumber + "  Type : " + address.Type);
            }
        }
    }
}
