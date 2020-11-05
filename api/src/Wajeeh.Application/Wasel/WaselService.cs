using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using RestSharp;
using System.Linq;
using System.Web;
using Wajeeh.Wasel.models;
using System.Globalization;
using System.Threading;

namespace Wajeeh.Wasel
{
    public class WaselService : IWaselService
    {
        public static string BaseUrl = "https://wasl-beta.api.elm.sa";

        public string RegTrip(TripRegVM viewModel)
        {
            var client = new RestClient(BaseUrl);
            RestRequest request;
            string param = "";
            request = new RestRequest("/api/eff/v1/trips", Method.POST);
            param = "{\n\"departedWhen\":\"" + DateTime.Parse(viewModel.departedWhen).ToString("yyyy-MM-dd'T'HH:mm:ss")
      + "\",\n\"departureLatitude\":\"" + viewModel.departureLatitude
      + "\",\n\"departureLongitude\":\"" + viewModel.departureLongitude
      + "\",\n\"driverIdentityNumber\":\"" + viewModel.driverIdentityNumber
      + "\",\n\"expectedDestinationLatitude\":\"" + viewModel.expectedDestinationLatitude
      + "\",\n\"expectedDestinationLongitude\":\"" + viewModel.expectedDestinationLongitude
      + "\",\n\"tripNumber\":\"" + viewModel.tripNumber
      + "\",\n\"vehicleSequenceNumber\":\"" + viewModel.vehicleSequenceNumber
      + "\"\n }";


            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("app-id", WaselHeader.appid);
            request.AddHeader("app-key", WaselHeader.appkey);
            request.AddHeader("client-id", WaselHeader.clientid);

            request.AddParameter("text/plain", param, ParameterType.RequestBody);

            IRestResponse<WaselResponse> response = client.Execute<WaselResponse>(request);

            var Data = JsonConvert.DeserializeObject<WaselResponse>(response.Content);

            var result = Data.success;
            return Data.resultCode;
        }

        public bool UpdateTrip(TripUpdateVM viewModel)
        {
            var client = new RestClient(BaseUrl);
            RestRequest request;
            string param = "";

            request = new RestRequest("/api/eff/v1/trips/" + viewModel.tripNumber, Method.PATCH);
            param = "{\n\"actualDestinationLatitude\":\"" + viewModel.actualDestinationLatitude
      + "\",\n\"actualDestinationLongitude\":\"" + viewModel.actualDestinationLongitude
      + "\",\n\"arrivedWhen\":\"" + DateTime.Parse(viewModel.arrivedWhen).ToString("yyyy-MM-dd'T'HH:mm:ss")
      + "\"\n }";


            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("app-id", WaselHeader.appid);
            request.AddHeader("app-key", WaselHeader.appkey);
            request.AddHeader("client-id", WaselHeader.clientid);

            request.AddParameter("text/plain", param, ParameterType.RequestBody);

            IRestResponse<WaselResponse> response = client.Execute<WaselResponse>(request);

            WaselResponse Data = new WaselResponse();
            try
            {
                Data = JsonConvert.DeserializeObject<WaselResponse>(response.Content);
            }
            catch (Exception )
            {
                return false;
            }

            var result = Data.success;
            return result;
        }

        public WaselResponseDTOC RegDriver(RegDriverVM viewModel)
        {
            var client = new RestClient(BaseUrl);
            RestRequest request;
            var param = "";
            request = new RestRequest("/api/eff/v1/drivers", Method.POST);
            param = "{\n\"dateOfBirthGregorian\":\"" + GetGerorgianDate(viewModel.dateOfBirthGregorian)
                      + "\",\n\"dateOfBirthHijri\":\"" + GetHijriDate(viewModel.dateOfBirthHijri)
                      + "\",\n\"email\":\"" + viewModel.email
                      + "\",\n\"identityNumber\":\"" + viewModel.identityNumber
                      + "\",\n\"mobileNumber\":\"" + "+966" + viewModel.mobileNumber
                      + "\"\n }";


            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("app-id", WaselHeader.appid);
            request.AddHeader("app-key", WaselHeader.appkey);
            request.AddHeader("client-id", WaselHeader.clientid);

            request.AddParameter("text/plain", param, ParameterType.RequestBody);

            IRestResponse<WaselResponseDTOC> response = client.Execute<WaselResponseDTOC>(request);

            var Data = JsonConvert.DeserializeObject<WaselResponseDTOC>(response.Content);

            return Data;
        }

        public WaselResponseDTOC RegVechile(VechileRegVm viewModel)
        {
            var client = new RestClient(BaseUrl);
            RestRequest request;
            string param = "";

            request = new RestRequest("/api/eff/v1/vehicles", Method.POST);
            param = "{\n\"plate\":\"" + viewModel.Plate
      + "\",\n\"plateType\":\"" + viewModel.PlateType
      + "\",\n\"sequenceNumber\":\"" + viewModel.SequenceNumber
      + "\"\n }";


            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("app-id", WaselHeader.appid);
            request.AddHeader("app-key", WaselHeader.appkey);
            request.AddHeader("client-id", WaselHeader.clientid);

            request.AddParameter("text/plain", param, ParameterType.RequestBody);

            IRestResponse<WaselResponseDTOC> response = client.Execute<WaselResponseDTOC>(request);

            var Data = JsonConvert.DeserializeObject<WaselResponseDTOC>(response.Content);

            return Data;
        }

        public string Save(FishVm viewModel, bool isAdd)
        {
            var client = new RestClient(BaseUrl);
            RestRequest request;
            string param = "";
            if (isAdd)
            {
                request = new RestRequest("/api/fish/AddFish/", Method.POST);
                param = "{\n\"name\":\"" + viewModel.Name
          + "\",\n\"nameAR\":\"" + viewModel.nameAR
          + "\",\n\"image\":\"" + viewModel.Image
          + "\",\n\"description\":\"" + viewModel.Description
          + "\",\n\"fishId\":\"" + viewModel.FishId
          + "\",\n\"avgPrice\":\"" + viewModel.AvgPrice
          + "\",\n\"fisHFAMILY\":\"" + viewModel.fishFamily
          + "\",\n\"descriptionAR\":\"" + viewModel.descriptionAR
          //     + "\",\n\"Id\":\"" + viewModel.Id
          + "\"\n }";
            }
            else
            {
                request = new RestRequest("/api/fish/UpdateFish/", Method.POST);
                param = "{\n\"name\":\"" + viewModel.Name
                  + "\",\n\"nameAR\":\"" + viewModel.nameAR
                  + "\",\n\"image\":\"" + viewModel.Image
                  + "\",\n\"description\":\"" + viewModel.Description
                  + "\",\n\"fishId\":\"" + viewModel.FishId
                  + "\",\n\"avgPrice\":\"" + viewModel.AvgPrice
                  + "\",\n\"fisHFAMILY\":\"" + viewModel.fishFamily
                  + "\",\n\"descriptionAR\":\"" + viewModel.descriptionAR
                  + "\",\n\"Id\":\"" + viewModel.Id

                  + "\"\n }";
            }

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "bearer " + viewModel.token);

            request.AddParameter("text/plain", param, ParameterType.RequestBody);

            //request.AddJsonBody(viewModel);
            //request.RequestFormat = DataFormat.Json;
            //    Response<FishVm> response = client.Execute<Response<FishVm>>(request).Data;
            IRestResponse<ApiResultVm> response = client.Execute<ApiResultVm>(request);

            var Data = JsonConvert.DeserializeObject<ApiResultVm>(response.Content);

            var result = Data.status;
            return result.severity;


        }
        public FishVm GetById(string id, string token)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("/api/fish/GetFish/", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "bearer " + token);
            request.AddJsonBody(new { ID = id });
            request.RequestFormat = DataFormat.Json;
            Response<FishVm> response = client.Execute<Response<FishVm>>(request).Data;
            return response.Content;
        }

        public List<FishVm> FindAll(FishVm searchModelNotReal, string token)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("/api/fish/GetAllFish/", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "bearer " + token);
            request.RequestFormat = DataFormat.Json;
            Response<List<FishVm>> response = client.Execute<Response<List<FishVm>>>(request).Data;
            return response.Content;
        }
        public string Delete(string id, string token)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("/api/fish/DeleteFish/", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "bearer " + token);
            request.AddJsonBody(new { ID = id });
            request.RequestFormat = DataFormat.Json;
            IRestResponse<ApiResultVm> response = client.Execute<ApiResultVm>(request);
            var Data = JsonConvert.DeserializeObject<ApiResultVm>(response.Content);
            return Data.status.severity;
        }

        private string ConvertDateCalendar(DateTime DateConv, string Calendar, string DateLangCulture)
        {
            System.Globalization.DateTimeFormatInfo DTFormat;
            DateLangCulture = DateLangCulture.ToLower();

            if (Calendar == "Hijri" && DateLangCulture.StartsWith("en-"))
            {
                DateLangCulture = "ar-sa";
            }

            DTFormat = new System.Globalization.CultureInfo(DateLangCulture, false).DateTimeFormat;
            switch (Calendar)
            {
                case "Hijri":
                    DTFormat.Calendar = new System.Globalization.HijriCalendar();
                    break;

                case "Gregorian":
                    DTFormat.Calendar = new System.Globalization.GregorianCalendar();
                    break;

                default:
                    return "";
            }

            DTFormat.ShortDatePattern = "yyyy-MM-dd";
            return (DateConv.Date.ToString("f", DTFormat));
        }

        private string GetGerorgianDate(string date)
        {
            var currentCult = CultureInfo.CurrentCulture.Name;
            if (currentCult == "ar-EG")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            }
            var dateSpilt = Convert.ToDateTime(date).ToString("dd-MM-yyyy").Split('-');
            date = $"{dateSpilt[2]}-{dateSpilt[1]}-{dateSpilt[0]}";
            return date;
        }

        private string GetHijriDate(string date)
        {
            var currentCult = CultureInfo.CurrentCulture.Name;
            if (currentCult == "ar-EG")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            }
            var dateSpilt = Convert.ToDateTime(date).ToString("dd-MM-yyyy").Split('-');
            date = $"{dateSpilt[2]}-{dateSpilt[1]}-{dateSpilt[0]}";
            return date;
        }
        //var dateSpilt = ConvertDateCalendar(Convert.ToDateTime(date).AddDays(-1), "Hijri", "ar-SA").Split('/');
        //date = $"{dateSpilt[2].Split(' ')[0]}-{dateSpilt[1]}-{dateSpilt[0]}";
        //return date;
    }
    }

