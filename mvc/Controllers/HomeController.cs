using Microsoft.AspNetCore.Mvc;

using Models.IndexModels;
using Models.ReportQueryModels.InverterModels;
using mvc.Models;
using mvc.Models.IndexModels;
using mvc.Models.ReportQueryModels;
using System.Diagnostics;


namespace mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBmanager _dbManager; // Add a private instance of DBmanager
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _dbManager = new DBmanager(); // Initialize the DBmanager instance
        }


        [HttpPost]
        public ActionResult AttensionneededTable(string selectedCheckboxes)
        {

            //DBmanager dbmanager = new DBmanager();
            //List<AttensionNeededModel> attensionNeededModels = dbmanager.GetAttensionNeededs());
            //ViewBag.ConverterModels = attensionNeededModels;
            NowDBmanager nowdbmanager = new NowDBmanager();

            object? selectedModel = null;

            selectedModel = nowdbmanager.GetAttensionNeededs(selectedCheckboxes);

            return Json(selectedModel);
        }


        [HttpPost]
        public IActionResult AlarmHistoryTable(DateTime startDate, DateTime endDate, string selectedCheckboxes)
        {
            DBmanager dbmanager = new DBmanager();

            object? selectedModel = null;

            selectedModel = dbmanager.GetAlarmHistoryDatas(startDate, endDate, selectedCheckboxes);


            return Json(selectedModel);
        }


        [HttpPost]
        public ActionResult TableRefresh(DateTime startDate, DateTime endDate, string selectedCheckboxes)
        {
            object? selectedModel = null;

                    selectedModel = _dbManager.GetHistoryDatas(startDate, endDate, selectedCheckboxes);

            return Json(selectedModel);
        }



        //public IActionResult LoadMainContent(string id)
        //{
        //    string viewPath
        //}

    
        public IActionResult ReportQuery()
        {
            NowDBmanager nowdbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = nowdbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;
            DBmanager dbmanager = new DBmanager();
            ////List<AttensionNeededModel> attensionNeededModels = dbmanager.GetAttensionNeededs();
            //ViewBag.ConverterModels = attensionNeededModels;
            return View();
        }

        //public IActionResult Home()
        //{

        //    return View();
        //}


        public ActionResult LoadInvDetail(string InvPath)
        {
            NowDBmanager dbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = dbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;
            ViewBag.Units = dbmanager.Units;
            ViewBag.DataNames = dbmanager.DataNames;
            List<List<string>> dataKeys = dbmanager.GetDataKeys(converterInfoModels[0].invCount);
            ViewBag.DataKeys = dataKeys;
            return PartialView(InvPath);
        }
        public ActionResult LoadPESDetail(string PESPath)
        {
            NowDBmanager dbmanager = new NowDBmanager();
            List<PowerEquipmentStatusModel> powerEquipmentModels = dbmanager.GetPowerEquipmentModels();
            ViewBag.PESModels = powerEquipmentModels;
            ViewBag.Units = dbmanager.PMUnits;
            ViewBag.DataNames = dbmanager.PMDataNames;
            ViewBag.PMNamesAndData = dbmanager.pmNamesAndData;

            ViewBag.DCUnits = dbmanager.DCPMUnits;
            ViewBag.DCDataNames = dbmanager.DCPMDataNames;
            


            return PartialView(PESPath);
        }

        public ActionResult LoadIndexDetail()
        {
            CalculatedDataDBmanager calculatedDataDBmanager = new CalculatedDataDBmanager();
            List<CalIndexModel> calIndexModels = calculatedDataDBmanager.GetCalIndexDatas();
            ViewBag.CalIndexModels = calIndexModels;


            NowDBmanager nowdbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = nowdbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;
         
            List<IndexModel> indexmodels = nowdbmanager.GetIndexDatas();
            ViewBag.IndexModels = indexmodels;

            return PartialView("Index/_Index");
        }
        public IActionResult Index()
        {
            //ConverterDBmanager Converterdbmanager = new ConverterDBmanager();
            //List<ConverterModel> converterModels = Converterdbmanager.GetConverterModels();
            //ViewBag.ConverterModels = converterModels;
            CalculatedDataDBmanager calculatedDataDBmanager = new CalculatedDataDBmanager();
            //List<CalIndexModel> calIndexModels = calculatedDataDBmanager.GetCalIndexDatas();
            //ViewBag.CalIndexModels = calIndexModels;

            NowDBmanager dbmanager = new NowDBmanager();
            List<IndexModel> indexmodels = dbmanager.GetIndexDatas();
            ViewBag.IndexModels = indexmodels;





            return View("~/Views/Home/Index/Index.cshtml");
        }

      
        public IActionResult Privacy()
        {
            //ConverterDBmanager Converterdbmanager = new ConverterDBmanager();
            //List<ConverterModel> converterModels = Converterdbmanager.GetConverterModels();
            //ViewBag.ConverterModels = converterModels;

            //DBmanager dbmanager = new DBmanager();
            //List<AttensionNeededModel> attensionNeededModels = dbmanager.GetAttensionNeededs();
            //ViewBag.ANModels = attensionNeededModels;





            return View("~/Views/Home/TBD/privacy.cshtml");
        }

        public IActionResult ACPM()
        {
            NowDBmanager dbmanager = new NowDBmanager();
            List<PowerEquipmentStatusModel> powerEquipmentModels = dbmanager.GetPowerEquipmentModels();
            ViewBag.PESModels = powerEquipmentModels;


            return View("~/Views/Home/ACPM/AC_MainPage.cshtml");
        }
        public IActionResult DCPM()
        {
            NowDBmanager dbmanager = new NowDBmanager();
            List<PowerEquipmentStatusModel> powerEquipmentModels = dbmanager.GetPowerEquipmentModels();
            ViewBag.PESModels = powerEquipmentModels;


            return View("~/Views/Home/DCPM/DC_MainPage.cshtml");
        }

        public IActionResult WeatherAndTemp()
        {
            


            return View("~/Views/Home/WeatherAndTemp/Extras.cshtml");
        }











        public IActionResult ConverterInfo()
        {
            NowDBmanager dbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = dbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;

            return View("~/Views/Home/ConverterInfo/ConverterInfo.cshtml");
        }





        public ActionResult LoadConverterDetail(string ConverterPath)
        {
            NowDBmanager dbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = dbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;
        
            ViewBag.Units = dbmanager.Units;
            ViewBag.DataNames = dbmanager.DataNames;
            List<List<string>> dataKeys = dbmanager.GetDataKeys(converterInfoModels[0].invCount);
            ViewBag.DataKeys = dataKeys;
            return PartialView(ConverterPath);
        }
        public ActionResult LoadWeatherTempDetail(string WeatherPath)
        {
            NowDBmanager dbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = dbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;
            List<PowerEquipmentStatusModel> powerEquipmentModels = dbmanager.GetPowerEquipmentModels();
            ViewBag.PESModels = powerEquipmentModels;
            List<WeatherTempModel> weatherTempModels = dbmanager.GetWeatherTempModels();
            ViewBag.WeatherTempModels = weatherTempModels;
            ViewBag.Units = dbmanager.Units;
            ViewBag.DataNames = dbmanager.DataNames;

            return PartialView(WeatherPath);
        }
        public IActionResult Converter()
        {
            NowDBmanager dbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = dbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;



            return View("~/Views/Home/ConverterOverview/Converter.cshtml");
        }

        public IActionResult Singlelinediagram()
        {
            NowDBmanager dbmanager = new NowDBmanager();
           
       

            List<PowerEquipmentStatusModel> powerEquipmentModels = dbmanager.GetPowerEquipmentModels();
            ViewBag.PESModels = powerEquipmentModels;

            List<ConverterInfoModel> converterInfoModels = dbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;

            return View();
        }

        [HttpGet]
        public ActionResult ChartRefresh(DateTime datetimefrom, DateTime datetimeto, List<string> name)
        {

            List<ChartModels> newChartData = _dbManager.GetChartDatas(datetimefrom, datetimeto, name);

            return Json(newChartData);
        }
        public IActionResult Chartquery(DateTime datetimefrom, DateTime datetimeto, string name)
        {

            NowDBmanager nowdbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = nowdbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;
            ViewBag.SelectedColumn = name; // Pass the selected column name to the view
            ViewBag.SelectedDatetimeFrom = datetimefrom;
            ViewBag.SelectedDatetimeTo = datetimeto;
            return View();
        }
        public IActionResult Attensionneeded()
        {
            NowDBmanager nowdbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = nowdbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;
            return View();
        }

        public IActionResult AlarmHistory()
        {
            NowDBmanager nowdbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = nowdbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;
            return View();
        }


        public IActionResult Sectorlayout()
        {
            return View();
        }



        public IActionResult ConverterInfo2()
        {
            NowDBmanager dbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = dbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;
            return View("~/Views/Home/ConverterInfo/ConverterInfo2.cshtml");
        }

        public IActionResult ConverterInfo3()
        {
            NowDBmanager dbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = dbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;
            return View("~/Views/Home/ConverterInfo/ConverterInfo3.cshtml");
        }

        public IActionResult ConverterInfo4()
        {
            NowDBmanager dbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = dbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;
            return View("~/Views/Home/ConverterInfo/ConverterInfo4.cshtml");
        }
        public IActionResult ConverterInfo5()
        {
            NowDBmanager dbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = dbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;
            return View("~/Views/Home/ConverterInfo/ConverterInfo5.cshtml");
        }

        public IActionResult ConverterInfo6()
        {
            NowDBmanager dbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = dbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;
            return View("~/Views/Home/ConverterInfo/ConverterInfo6.cshtml");
        }

        public IActionResult ConverterInfo7()
        {
            NowDBmanager dbmanager = new NowDBmanager();
            List<ConverterInfoModel> converterInfoModels = dbmanager.GetConverterInfoDatas();
            ViewBag.ConverterInfoModels = converterInfoModels;
            return View("~/Views/Home/ConverterInfo/ConverterInfo7.cshtml");
        }

        public IActionResult ErrorPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}