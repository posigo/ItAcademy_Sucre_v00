using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sucre_Core.DTOs;
using Sucre_Models;
using Sucre_MVC.Models;
using Sucre_Services.Interfaces;
using Sucre_Utility;

namespace Sucre_MVC.Controllers
{
    [Authorize(Roles = $"{WC.AdminRole},{WC.UserRole},{WC.SupervisorRole}")]
    public partial class MeasurementController : Controller
    {
        private readonly IPointService _pointService;
        private readonly IMeasurementService _measurementService;
        private static List<SelectListItem> _pointListItem = new List<SelectListItem>();
        public MeasurementController(IPointService pointService,
            IMeasurementService measurementService)
        {
            _pointService = pointService;
            _measurementService = measurementService;
        }
        public IActionResult Index()
        {
            return View();
        }

        //получение списка отчётов
        private async Task<List<SelectListItem>> GetPointSelectListItem (IEnumerable<PointDto> pointsDto)
        {
            List<SelectListItem> tmpPoints = new List<SelectListItem>();
            tmpPoints.Add(new SelectListItem
            {
                Text = "--Select from the list--",
                Value = 0.ToString(),
                Selected = true,
                Disabled = true
            });
            foreach (var point in pointsDto)
            {
                tmpPoints.Add(new SelectListItem
                {
                    Text = point.Name,
                    Value = point.Id.ToString()
                });
            }
            return tmpPoints;
        }

        [HttpGet]
        public async Task<IActionResult> ValueHour()
        {
            var pointsDto = await _pointService.GetListPointsAsync();
            _pointListItem = await GetPointSelectListItem(pointsDto);
            ConsValueHourM consValueHourM = new ConsValueHourM();            
            consValueHourM.PointNameSelectList = _pointListItem;

            return View(consValueHourM);
        }

        /// <summary>
        /// проверка даты
        /// </summary>
        /// <param name="dd"></param>
        /// <param name="mm"></param>
        /// <param name="yyyy"></param>
        /// <returns></returns>
        private string GetQueryString(int dd, int mm, int yyyy)
        {
            if (dd != 32 && mm != 13 && yyyy !=2034)
            {
                var strDate = $"{dd}/{mm}/{yyyy}";
                var ddate = Convert.ToDateTime(strDate);
                if (ddate.Date > DateTime.Now.Date)
                {
                    return "error";
                    
                }
                else
                {
                    return strDate;
                }
            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ValueHour")]
        public async Task<IActionResult> ValueHourPost(ConsValueHourM consValueHour, List<SelectListItem> lst)
        {
            var sds = _pointListItem;
            if (consValueHour.PointId == 0)
            {
                ModelState.AddModelError("PointNameSelectList", "Not choise point");
            }
            if (ModelState.IsValid)
            {
                string queryDate = GetQueryString(consValueHour.NDay, consValueHour.NMonth, consValueHour.NYear);
                if (queryDate == "error")
                {
                    ModelState.AddModelError("NYear", "Error date");
                }
                if (consValueHour.NHour != 24 && queryDate != null && queryDate != "error")
                {
                    DateTime validateDate = Convert.ToDateTime($"{queryDate} {consValueHour.NHour}:00:00");
                    if (DateTime.Now<validateDate)
                    {
                        ModelState.AddModelError("NHour", "Error hour");
                    }
                }
                if (ModelState.IsValid)
                {
                    //var valueh = (consValueHour.NHour == 24) ?
                    //    await _measurementService.GetHourValueByPoint(consValueHour.PointId, queryDate, null) :
                    //    await _measurementService.GetHourValueByPoint(consValueHour.PointId, queryDate, consValueHour.NHour);
                    _pointListItem = null;
                    ValuesHourByIdPointDto valH = new ValuesHourByIdPointDto();
                    ValuesHourByIdPointM viewResult = new ValuesHourByIdPointM();
                    if (consValueHour.NHour == 24)
                    {

                        valH = await _measurementService.GetValuesHourByPointId(
                            consValueHour.PointId,
                            queryDate,
                            null);
                                                
                        //return RedirectToAction(nameof(GetValuesHourByPointId), new
                        //{
                        //    pointId = consValueHour.PointId,
                        //    queryDate = queryDate
                        //});
                    }
                    else
                    {
                        valH = await _measurementService.GetValuesHourByPointId(
                            consValueHour.PointId,
                            queryDate,
                            consValueHour.NHour);

                        //return RedirectToAction(nameof(GetValuesHourByPointId), new
                        //{
                        //    pointId = consValueHour.PointId,
                        //    queryDate = queryDate,
                        //    queryHour = consValueHour.NHour,
                        //});
                    }
                    viewResult.ValuesHourPoint = valH.ValuesHourPoint;
                    viewResult.Heading = valH.Heading;
                    viewResult.Columns = valH.Columns;
                    viewResult.ColumnsName = valH.ColumnsName;
                    viewResult.Rows = valH.Rows;
                    viewResult.TableDict = valH.TableDict;

                    return View("GetValuesHourByPointId",viewResult);
                    
                }
                else
                {
                    consValueHour.PointNameSelectList = _pointListItem;
                    return View(consValueHour);
                }
                
            }


            return RedirectToAction(nameof(ValueHour));
            //return View();
        }

        //public class ValuesHourByIdPointM
        //{
        //    public MeasurementPointDto ValuesHourPoint { get; set; }
        //    public string Heading { get; set; }
        //    public int Columns { get; set; }
        //    //public List<string>[] ColumnsName { get; set; }
        //    public (int, List<string>)[] ColumnsName { get; set; }
        //    public int Rows { get; set; }
        //    public Dictionary<int, decimal[]>  TableDict { get; set; }
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetValuesHourByPointId(int pointId, string? queryDate, int? queryHour)
        //{
        //    var valueh = await _measurementService.GetHourValueByPoint(pointId, queryDate, queryHour);
        //    ValuesHourByIdPointM valuesHourByIdPointM = new ValuesHourByIdPointM();
        //    valuesHourByIdPointM.ValuesHourPoint = valueh;
        //    var heading = $"Отчёт: {Environment.NewLine} {valueh.PointTableDto.PointDto.Name} ";
        //    if (queryDate != null)
        //    {
        //        var tmpHead = $"{Environment.NewLine} за {queryDate.Replace("/", ".")}";
        //        heading += tmpHead ;
        //    }
        //    valuesHourByIdPointM.Heading = heading ;
        //    if (valueh.ChannalesFullDto.Count == 0)
        //    {
        //        valuesHourByIdPointM.Columns = 0;
        //        valuesHourByIdPointM.ColumnsName = null;
        //    }
        //    else
        //    {
        //        valuesHourByIdPointM.Columns = valueh.ChannalesFullDto.Count + 1;
        //        (int, List<string>) tupleNameCol;
        //        var strNameColl = new List<(int, List<string>)>();
        //        tupleNameCol = (0, new List<string>() { "Час" });
        //        strNameColl.Add(tupleNameCol);
        //        foreach (var cnl in valueh.ChannalesFullDto)
        //        {
        //            tupleNameCol = (cnl.CannaleDto.Id, new List<string>()
        //            {
        //                $"{cnl.CannaleDto.Name}",
        //                $"{cnl.ParameterTypeDto.Mnemo}, {cnl.ParameterTypeDto.UnitMeas}"
        //            });
        //            strNameColl.Add(tupleNameCol);
        //        }
                    
                


        //        //var strNameColl = new List<List<string>>();

        //        //strNameColl.Add(new List<string>() { "Час" });
        //        //foreach (var c in valueh.ChannalesFullDto)
        //        //    strNameColl.Add (new List<string>()
        //        //    {
        //        //        $"{c.CannaleDto.Name}",
        //        //        $"{c.ParameterTypeDto.Mnemo}, {c.ParameterTypeDto.UnitMeas}"
        //        //    });
        //        valuesHourByIdPointM.ColumnsName = strNameColl.ToArray();                


        //    }
        //    valuesHourByIdPointM.Rows = valueh.ValuesHourDto.Count;

        //    if (valueh.ValuesHourDto.Count > 0)
        //    {
        //        var tableDic = new Dictionary<int, decimal[]>();
        //        var decimalMas = new List<decimal>();
        //        //if (queryHour != null)
        //        //{
        //            for (var hour = ((queryHour == null) ? 0 : queryHour.Value);
        //                hour <= ((queryHour == null) ? 23 : queryHour.Value);
        //                hour++)
        //            {
        //                foreach (var cnl in valueh.ChannalesFullDto)
        //                {
        //                    var valueHour = valueh.ValuesHourDto
        //                        .Where(value => value.Id == cnl.CannaleDto.Id && value.Hour == hour)
        //                        .Select(v => v.Value)
        //                        .FirstOrDefault();
        //                    if (valueHour != null)
        //                        decimalMas.Add(valueHour);
        //                    else
        //                        decimalMas.Add(0);
        //                }
        //                tableDic.Add(hour, decimalMas.ToArray());
        //                decimalMas.Clear();
        //            }
        //        //}
        //        valuesHourByIdPointM.TableDict = tableDic;
        //    }
        //    else
        //    {
        //        valuesHourByIdPointM.TableDict = null;
        //    }
            

        //    return View(valuesHourByIdPointM);
        //}

        [HttpGet]
        public async Task<IActionResult> ValueDay()
        {
            var pointsDto = await _pointService.GetListPointsAsync();
            _pointListItem = await GetPointSelectListItem(pointsDto);
            ConsValueDayM consValueDayM = new ConsValueDayM();
            consValueDayM.PointNameSelectList = _pointListItem;

            return View(consValueDayM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ValueDay")]
        public async Task<IActionResult> ValueDayPost(ConsValueDayM consValueDayM)
        {
            //var sds = _pointListItem;
            //проверка на наличие Id т.у.
            if (consValueDayM.PointId == 0)
            {
                ModelState.AddModelError("PointNameSelectList", "Not choise point");
            }
            if (ModelState.IsValid)
            {
                //проверка нач даты
                string queryDate = GetQueryString(consValueDayM.NDay, consValueDayM.NMonth, consValueDayM.NYear);
                if (queryDate == "error")
                {
                    ModelState.AddModelError("NYear", "Error date");
                }
                //проверка кон даты
                string queryDateEnd;
                if (consValueDayM.NDayE.HasValue &&
                    consValueDayM.NMonthE.HasValue &&
                    consValueDayM.NYearE.HasValue)
                {
                    queryDateEnd = GetQueryString(consValueDayM.NDayE.Value, consValueDayM.NMonthE.Value, consValueDayM.NYearE.Value);
                    if (queryDateEnd == "error")
                    {
                        ModelState.AddModelError("NYearE", "Error date");
                    }
                    //нач дата>кон даты
                    if (queryDate!="error" &&
                        queryDateEnd!="error" &&
                        DateTime.Parse(queryDate)> DateTime.Parse(queryDateEnd))
                    {
                        ModelState.AddModelError("NYearE", "Error date");
                    }
                }
                else
                {
                    queryDateEnd = string.Empty;
                }
                
                if (ModelState.IsValid)
                {                    
                    _pointListItem = null;
                    //возможно для релизации отчёта здесь
                    //MeasurementPointHDto result;
                    //result = await _measurementService.GetDayValueByPoint(
                    //    consValueDayM.PointId,
                    //    queryDate,
                    //    queryDateEnd == string.Empty ? null : queryDateEnd);

                    //получить класс отчёта
                    ValuesDayByIdPointDto viewRes = new ValuesDayByIdPointDto();
                    if (queryDateEnd == string.Empty)
                    {
                        viewRes = await _measurementService.GetValuesDayByPointId(
                            consValueDayM.PointId,
                            queryDate,
                            null);
                        //return View("GetValuesDayByPointId", viewres);
                        //return RedirectToAction(
                        //    nameof(GetValuesDayByPointId), new
                        //{
                        //    consValueDayM.PointId,
                        //    queryDate                            
                        //});
                    }
                    else
                    {
                        
                        viewRes = await _measurementService.GetValuesDayByPointId(
                            consValueDayM.PointId,
                            queryDate,
                            queryDateEnd);
                        //return View("GetValuesDayByPointId", viewres);
                        //передаётся значение queryDateEnd в GetValuesDayByPointId как null почему???
                        //return RedirectToAction(
                        //    nameof(GetValuesDayByPointId), new
                        //{
                        //    consValueDayM.PointId,
                        //    queryDate,
                        //    queryDateEnd
                        //});
                    }
                    return View("GetValuesDayByPointId", viewRes);


                }
                else
                {
                    //если модель не валидна
                    consValueDayM.PointNameSelectList = _pointListItem;
                    return View(consValueDayM);
                }

            }
            else
            {
                //если модель не валидна
                return RedirectToAction(nameof(ValueDay));
            } 
                

            
            //return View();
        }

        //public class ValuesDayByIdPointM
        //{
        //    /// <summary>
        //    /// данные по точке учёта
        //    /// </summary>
        //    public MeasurementPointHDto ValuesDayPoint { get; set; }
        //    /// <summary>
        //    /// заголовок
        //    /// </summary>
        //    public string Heading { get; set; }
        //    /// <summary>
        //    /// количество колонок
        //    /// </summary>
        //    public int Columns { get; set; }
        //    //public List<string>[] ColumnsName { get; set; }
        //    /// <summary>
        //    /// номер колонки и название колонки
        //    /// </summary>
        //    public (int, List<string>)[] ColumnsName { get; set; }
        //    /// <summary>
        //    /// количество строк
        //    /// </summary>
        //    public int Rows { get; set; }
        //    /// <summary>
        //    /// словарь: дата строка значений
        //    /// </summary>
        //    public Dictionary<DateTime, decimal[]> TableDict { get; set; }
        //}


        //ValuesDayByIdPointM класс отчёта
        //[HttpGet]
        //public async Task<ValuesDayByIdPointM> GetValuesDayByPointId(
        //    int pointId, 
        //    string? queryDate, 
        //    string? queryDateE)
        //{
        //    //получить данные с дд1 по дд2
        //    var valued = await _measurementService.GetDayValueByPoint(pointId, queryDate, queryDateE);
        //    //отчёт для отображения
        //    ValuesDayByIdPointM valuesDayByIdPointM = new ValuesDayByIdPointM();
        //    valuesDayByIdPointM.ValuesDayPoint = valued;
        //    //заголовок
        //    var heading = $"Отчёт: {Environment.NewLine} {valued.PointTableDto.PointDto.Name} ";
        //    if (queryDate != null)
        //    {
        //        if (queryDateE != null) 
        //        {
        //            var tmpHead = $"{Environment.NewLine} c {queryDate.Replace("/", ".")} по {queryDateE.Replace("/", ".")}";
        //            heading += tmpHead;
        //        }
        //        else
        //        {
        //            var tmpHead = $"{Environment.NewLine} за {queryDate.Replace("/", ".")}";
        //            heading += tmpHead;
        //        }
                
        //    }
        //    valuesDayByIdPointM.Heading = heading;
        //    //список каналов
        //    if (valued.ChannalesFullDto.Count == 0)
        //    {
        //        valuesDayByIdPointM.Columns = 0;
        //        valuesDayByIdPointM.ColumnsName = null;
        //    }
        //    else
        //    {
        //        valuesDayByIdPointM.Columns = valued.ChannalesFullDto.Count + 1;
        //        //названия колонок
        //        (int, List<string>) tupleNameCol;
        //        var strNameColl = new List<(int, List<string>)>();
        //        //первая колонка
        //        tupleNameCol = (0, new List<string>() { "Дата" });
        //        strNameColl.Add(tupleNameCol);
        //        foreach (var cnl in valued.ChannalesFullDto)
        //        {
        //            tupleNameCol = (cnl.CannaleDto.Id, new List<string>()
        //            {
        //                $"{cnl.CannaleDto.Name}",
        //                $"{cnl.ParameterTypeDto.Mnemo}, {cnl.ParameterTypeDto.UnitMeas}"
        //            });
        //            strNameColl.Add(tupleNameCol);
        //        }

        //        valuesDayByIdPointM.ColumnsName = strNameColl.ToArray();


        //    }
        //    valuesDayByIdPointM.Rows = valued.ValuesDayDto.Count;
        //    //значения строк по колонкам
        //    if (valued.ValuesDayDto.Count > 0)
        //    {
        //        var tableDic = new Dictionary<DateTime, decimal[]>();
        //        var decimalMas = new List<decimal>();
        //        var ddateE = queryDateE == null ? 
        //            DateTime.Parse(queryDate) :
        //            DateTime.Parse(queryDateE);
        //        for (var ddate = DateTime.Parse(queryDate);
        //            ddate <= ddateE; ddate = ddate.AddDays(1))
        //        {
        //            foreach (var cnl in valued.ChannalesFullDto)
        //            {
        //                var valueDay = valued.ValuesDayDto
        //                    .Where(value => value.Id == cnl.CannaleDto.Id && value.Date == ddate)
        //                    .Select(v => v.Value)
        //                    .FirstOrDefault();
        //                if (valueDay != null)
        //                    decimalMas.Add(valueDay);
        //                else
        //                    decimalMas.Add(0);
        //            }
        //            tableDic.Add(ddate.Date, decimalMas.ToArray());
        //            decimalMas.Clear();
        //        }
        //        //}
        //        valuesDayByIdPointM.TableDict = tableDic;
        //    }
        //    else
        //    {
        //        valuesDayByIdPointM.TableDict = null;
        //    }

        //    return valuesDayByIdPointM;
        //    //return View(valuesDayByIdPointM);
        //}

    }

    
}
