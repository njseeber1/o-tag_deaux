using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Xunit;
using Moq;
using Newtonsoft.Json;

namespace o_tag_deaux0.Tests
{
    public class TestClearKeys
    {

        GenericResponse response;
        private void SetupMocks()
        {
            

           
        }
        [Fact]
        public void AssertReponseOK()
        {
            DAL.ClearRoomKeys = (x) =>
            {
                return JsonConvert.DeserializeObject<GenericResponse>("{\"result\":\"OK\",\"test\":\"tester\"}");
            };
            response = DAL.ClearRoomKeys("123456");
            Assert.Equal("OK",response.result);
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
