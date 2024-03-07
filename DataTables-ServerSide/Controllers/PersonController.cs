
using DataTables_ServerSide.DAL;
using DataTables_ServerSide.Models;
using DataTables_ServerSide.Repositories;
using DataTables_ServerSide.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.DynamicData.ModelProviders;
using System.Web.Mvc;

namespace DataTables_ServerSide.Controllers
{
    public class PersonController : Controller
    {

        private IPeopleRepository _peopleRepository;
        private AppDbContext _dbContext;


        public PersonController()
        {
            _dbContext = new AppDbContext();
            _peopleRepository = new PeopleRepository(_dbContext);
        }

        public ActionResult Index()
        {
            return View();
        }

        public class DataTablesResponse
        {
            public int draw { get; set; }
            public int recordsTotal { get; set; }
            public int recordsFiltered { get; set; }
            public List<People> data { get; set; }
        }
        
        public async Task<ActionResult> GetPeople()
        {
            var draw = int.Parse(Request["draw"]);
            var start = int.Parse(Request["start"]);
            var length = int.Parse(Request["length"]);
            var sortColumn = Request["order[0][column]"];
            var sortDirection = Request["order[0][dir]"];

            var columnSearches = Request.Form.AllKeys
                .Where(key => key.StartsWith("columns[") && key.EndsWith("][search][value]"))
                .ToDictionary(key => key, key => Request[key]);

            var people = (await _peopleRepository.GetListAsync()).ToList();

            foreach (var kvp in columnSearches)
            {
                var columnIndex = int.Parse(kvp.Key.Split('[', ']')[1]);
                var search = kvp.Value;
                search = search.Trim('^', '$');

                switch (columnIndex)
                {
                    case 0:
                        people = people.Where(p => p.Id.ToString().Contains(search)).ToList();
                        break;
                    case 1:
                        people = people.Where(p => p.FirstName.Contains(search)).ToList();
                        break;
                    case 2:
                        people = people.Where(p => p.LastName.Contains(search)).ToList();
                        break;
                }
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "0":
                        people = sortDirection == "asc" ? people.OrderBy(p => p.Id).ToList() : people.OrderByDescending(p => p.Id).ToList();
                        break;
                    case "1":
                        people = sortDirection == "asc" ? people.OrderBy(p => p.FirstName).ToList() : people.OrderByDescending(p => p.FirstName).ToList();
                        break;
                    case "2":
                        people = sortDirection == "asc" ? people.OrderBy(p => p.LastName).ToList() : people.OrderByDescending(p => p.LastName).ToList();
                        break;
                }
            }

            var totalRecords = people.Count;

            // Get unique values for each column
            var uniqueValues = new Dictionary<int, IEnumerable<string>>
            {
                { 0, people.Select(p => p.Id.ToString()).Distinct() },
                { 1, people.Select(p => p.FirstName).Distinct() },
                { 2, people.Select(p => p.LastName).Distinct() }
            };

            people = people.Skip(start).Take(length).ToList();

            var response = new DataTablesResponse
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = people,
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetPeopleUniqueValues()
        {
            var people = (await _peopleRepository.GetListAsync()).ToList();

            var uniqueValues = new Dictionary<string, List<string>>();

            // Get unique values for each column
            uniqueValues["ID"] = people.Select(p => p.Id.ToString()).Distinct().ToList();
            uniqueValues["First Name"] = people.Select(p => p.FirstName).Distinct().ToList();
            uniqueValues["Last Name"] = people.Select(p => p.LastName).Distinct().ToList();

            return Json(uniqueValues, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult GetPeople()
        //{
        //    var draw = int.Parse(Request["draw"]);
        //    var start = int.Parse(Request["start"]);
        //    var length = int.Parse(Request["length"]);
        //    var searchValue = Request["search[value]"];
        //    var sortColumn = Request["order[0][column]"];
        //    var sortDirection = Request["order[0][dir]"];

        //    // Individual column searches
        //    var columnSearches = Request.Form.AllKeys.Where(key => key.StartsWith("columns[") && key.EndsWith("][search][value]"))
        //        .ToDictionary(key => key, key => Request[key]);

        //    var people = GenerateDummyPersons();

        //    if (!string.IsNullOrEmpty(searchValue))
        //    {
        //        people = people.Where(p =>
        //            p.FirstName.Contains(searchValue) ||
        //            p.LastName.Contains(searchValue)
        //        ).ToList();
        //    }

        //    // Apply sorting
        //    if (!string.IsNullOrEmpty(sortColumn))
        //    {
        //        // Determine which column to sort by
        //        switch (sortColumn)
        //        {
        //            case "0":
        //                people = sortDirection == "asc" ? people.OrderBy(p => p.Id).ToList() : people.OrderByDescending(p => p.Id).ToList();
        //                break;
        //            case "1":
        //                people = sortDirection == "asc" ? people.OrderBy(p => p.FirstName).ToList() : people.OrderByDescending(p => p.FirstName).ToList();
        //                break;
        //            case "2":
        //                people = sortDirection == "asc" ? people.OrderBy(p => p.LastName).ToList() : people.OrderByDescending(p => p.LastName).ToList();
        //                break;
        //                // Add more cases for additional columns
        //        }
        //    }

        //    // Apply individual column searches
        //    foreach (var kvp in columnSearches)
        //    {
        //        var columnIndex = int.Parse(kvp.Key.Split('[', ']')[1]);
        //        var search = kvp.Value;

        //        // Apply search filter for the specific column
        //        switch (columnIndex)
        //        {
        //            case 0:
        //                people = people.Where(p => p.Id.ToString().Contains(search)).ToList();
        //                break;
        //            case 1:
        //                people = people.Where(p => p.FirstName.Contains(search)).ToList();
        //                break;
        //            case 2:
        //                people = people.Where(p => p.LastName.Contains(search)).ToList();
        //                break;
        //                // Add more cases for additional columns
        //        }
        //    }

        //    var totalRecords = people.Count;

        //    people = people.Skip(start).Take(length).ToList();

        //    var response = new DataTablesResponse
        //    {
        //        draw = draw,
        //        recordsTotal = totalRecords,
        //        recordsFiltered = totalRecords,
        //        data = people
        //    };

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}
    }
}