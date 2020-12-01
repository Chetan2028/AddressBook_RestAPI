using AddressBook_RestAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        [TestMethod]
        public void GivenContact_OnPost_ShouldReturnMultipleContacts()
        {
            //Adding multiple contacts to list
            List<AddressBook> multipleContactList = new List<AddressBook>();
            multipleContactList.Add(new AddressBook { FirstName = "Sandy", LastName = "Pujeri", Zip = "590016", PhoneNumber = "89514236445", Address = "Hampi Nagar", City = "Bangalore", State = "Karnataka", Type = "Friend" });
            multipleContactList.Add(new AddressBook { FirstName = "Ash", LastName = "Pise", Zip = "590486", PhoneNumber = "7412569841", Address = " WhiteField", City = "Bangalore", State = "Karnataka", Type = "Friend" });
            multipleContactList.Add(new AddressBook { FirstName = "Daya", LastName = "Karagi", Zip = "590176", PhoneNumber = "89519876445", Address = "Maple Beach", City = "Karwar", State = "Karnataka", Type = "Family" });

            multipleContactList.ForEach(contactData =>
            {
                //Arrange
                RestRequest request = new RestRequest("/addressBook", Method.POST);
                JObject jObjectBody = new JObject();
                jObjectBody.Add("FirstName", contactData.FirstName);
                jObjectBody.Add("LastName", contactData.LastName);
                jObjectBody.Add("Zip", contactData.Zip);
                jObjectBody.Add("Address", contactData.Address);
                jObjectBody.Add("City", contactData.City);
                jObjectBody.Add("State", contactData.State);
                jObjectBody.Add("PhoneNumber", contactData.PhoneNumber);
                jObjectBody.Add("Type", contactData.Type);

                request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

                //Act
                IRestResponse response = client.Execute(request);

                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                AddressBook dataResponse = JsonConvert.DeserializeObject<AddressBook>(response.Content);
                Assert.AreEqual(contactData.FirstName, dataResponse.FirstName);
                Assert.AreEqual(contactData.LastName, dataResponse.LastName);
                Console.WriteLine(response.Content);
            });
        }
    }
}
