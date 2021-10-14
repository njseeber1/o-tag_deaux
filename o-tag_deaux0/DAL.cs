using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace o_tag_deaux0
{
    public static class DAL
    {
        static string atlantis_baseURL = "http://91.75.74.44:4480/v2_api/";
        //static string baseURL = "http://10.215.0.250:80/v2_api/";
        static string baseURL = "http://dev1.vantagelabs.co/v2_api/";
        static HttpClient client = new HttpClient();

        public static Family CreateFamily(Reservation reservation)
        {
            Family family = new Family();
            // Create the http request
            string Url = baseURL + "atlantis/createFamily.php?resid=" + reservation.confirmationid;
            var webRequest = WebRequest.Create(Url);

            // Send the http request and wait for the response
            var responseStream = webRequest.GetResponse().GetResponseStream();

            // Displays the response stream text
            if (responseStream != null)
            {
                using (var streamReader = new StreamReader(responseStream))
                {
                    // Return next available character or -1 if there are no characters to be read
                    string obj = String.Empty;
                    while (streamReader.Peek() > -1)
                    {
                        obj = (streamReader.ReadLine());
                    }
                    family = JsonConvert.DeserializeObject<Family>(obj);
                }
            }
            return family;
        }

        public static Func<string, GenericResponse> ClearRoomKeys = (confirmationNumber) =>
       {
           GenericResponse response = new GenericResponse();
           string url = baseURL + "atlantis/clearRoomKeys.php?"
              + "&confirmationid=" + confirmationNumber
              ;


           var webRequest = WebRequest.Create(url);


           var responseStream = webRequest.GetResponse().GetResponseStream();
           if (responseStream != null)
           {
               using (var streamReader = new StreamReader(responseStream))
               {
                   string obj = String.Empty;
                   while (streamReader.Peek() > -1)
                   {
                       obj = (streamReader.ReadLine());
                   }
                   response = JsonConvert.DeserializeObject<GenericResponse>(obj);
               }

           }
           return response;
       };

        public static GenericResponse CreateHotelReservation(Reservation reservation)
        {
            HotelReservation hres = new HotelReservation(reservation);

            hres.outdate = hres.outdate.Replace("-", "");
            hres.indate = hres.indate.Replace("-", "");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL + "atlantis/createHotelReservation.php");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";


            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(hres);

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string result = String.Empty;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<GenericResponse>(result);
        }
        public static GenericResponse ActivateKey(string mediacode, string confirmationNumber)
        {

            string url = baseURL + "atlantis/assignRoomKey.php?"
                + "mediacode=" + Sanitize(mediacode)
                + "&confirmationid=" + confirmationNumber
                ;


            var webRequest = WebRequest.Create(url);


            GenericResponse response = new GenericResponse();
            var responseStream = webRequest.GetResponse().GetResponseStream();
            if (responseStream != null)
            {
                using (var streamReader = new StreamReader(responseStream))
                {
                    string obj = String.Empty;
                    while (streamReader.Peek() > -1)
                    {
                        obj = (streamReader.ReadLine());
                    }
                    response = JsonConvert.DeserializeObject<GenericResponse>(obj);
                }

            }
            return response;
        }
        private static string Sanitize(string unclean)
        {
            return Regex.Replace(unclean, @"\s+", "").Trim().ToUpper();
        }
        public static User LogIn(string serialNum)
        {
            string url = baseURL + "atlantis/staffLogin.php?staffcard=" + serialNum + "&logintype=C";
            User response = new User();
            var webRequest = WebRequest.Create(url);
            var responseStream = webRequest.GetResponse().GetResponseStream();
            if (responseStream != null)
            {
                using (var streamReader = new StreamReader(responseStream))
                {
                    string obj = String.Empty;
                    while (streamReader.Peek() > -1)
                    {
                        obj = (streamReader.ReadLine());
                    }
                    response = JsonConvert.DeserializeObject<User>(obj);
                }

            }
            return response;
        }
        public static Reservation GetReservation(string reservationID)
        {
            Reservation reservation = new Reservation();

            // Create the http request
            string Url = atlantis_baseURL + "atlantis/getReservation.php?resid=" + reservationID;

            var webRequest = WebRequest.Create(Url);
            string Url_passport = "http://10.102.100.53:8091/api/VantageService/FetchReservationDetails?confirmationNo=" + reservation.confirmationid;

            var webRequest_passport = WebRequest.Create(Url_passport);
            webRequest_passport.Method = "POST";
            
            var values = new Dictionary<string, string>
            {
                { "thing1", "hello" },
                { "thing2", "world" }
            };

            var json = JsonConvert.SerializeObject(values);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            try
            {
                var reqStream = webRequest_passport.GetRequestStream();
                reqStream.Write(bytes, 0, bytes.Length);

            }
            catch (Exception)
            {

                throw;
            }
           

            var resp = webRequest_passport.GetResponse();

            var t = ((HttpWebResponse)resp).StatusDescription;

            var respStream = resp.GetResponseStream();
            var reader = new StreamReader(respStream);
            string data = reader.ReadToEnd();


    

           // var responseString = response.Content

            // Send the http request and wait for the response
            var responseStream = webRequest.GetResponse().GetResponseStream();

            // Displays the response stream text
            if (responseStream != null)
            {
                using (var streamReader = new StreamReader(responseStream))
                {
                    // Return next available character or -1 if there are no characters to be read
                    string obj = String.Empty;
                    while (streamReader.Peek() > -1)
                    {
                        obj = (streamReader.ReadLine());
                    }


                    reservation = JsonConvert.DeserializeObject<Reservation>(obj);
                    if (!reservation.result.Equals("FAIL"))
                    {
                        reservation.dateIndate = DateTime.Parse(reservation.indate);
                        reservation.dateOutdate = DateTime.Parse(reservation.outdate);

                        reservation.confirmationid = reservationID;
                    }
                }
            }

            //var data = GetPassportDataAsync(reservation).Result;

            return reservation;
        }

        private static async System.Threading.Tasks.Task<Reservation> GetPassportDataAsync(Reservation reservation)
        {
            string Url = atlantis_baseURL + "atlantis/getReservation.php?resid=";

            HttpClient client = new HttpClient();

            using (client)
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), Url))
                {
                    request.Content = new StringContent("text.txt");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
                    var response = await client.SendAsync(request);
                }
            }
            var webRequest = WebRequest.Create(Url);
            //webRequest.Method = "POST";
            // Send the http request and wait for the response
            var responseStream = webRequest.GetResponse().GetResponseStream();

            // Displays the response stream text
            if (responseStream != null)
            {
                using (var streamReader = new StreamReader(responseStream))
                {
                    // Return next available character or -1 if there are no characters to be read
                    string obj = String.Empty;
                    while (streamReader.Peek() > -1)
                    {
                        obj = (streamReader.ReadLine());
                    }


                    reservation = JsonConvert.DeserializeObject<Reservation>(obj);
                    if (!reservation.result.Equals("FAIL"))
                    {
                        //reservation.dateIndate = DateTime.Parse(reservation.indate);
                        //reservation.dateOutdate = DateTime.Parse(reservation.outdate);

                        //reservation.confirmationid = reservationID;
                    }
                }
            }

            return reservation;
        }

    }
}