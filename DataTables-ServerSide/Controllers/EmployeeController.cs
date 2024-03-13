using DataTables_ServerSide.DAL;
using DataTables_ServerSide.Models.Entities;
using DataTables_ServerSide.Repositories.Implementations;
using DataTables_ServerSide.Repositories.Interfaces;
using DataTables_ServerSide.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DataTables_ServerSide.Controllers
{
    public class EmployeeController : Controller
    {
        private IEmployeeRepository _peopleRepository;
        private AppDbContext _dbContext;

        public EmployeeController()
        {
            _dbContext = new AppDbContext();
            _peopleRepository = new EmployeeRepository(_dbContext);
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetEmployeeList()
        {
            var draw = int.Parse(Request["draw"]);
            var start = int.Parse(Request["start"]);
            var length = int.Parse(Request["length"]);
            var sortColumn = Request["order[0][column]"];
            var sortDirection = Request["order[0][dir]"];

            var columnSearches = Request.Form.AllKeys
                .Where(key => key.StartsWith("columns[") && key.EndsWith("][search][value]"))
                .ToDictionary(key => key, key => Request[key]);

            var spec = new BaseSpecification<TBL_EMPLOYEE>(x => true);
            spec.AddInclude(x => x.DEPARTMENT_DETAILS);

            var people = (await _peopleRepository.GetListBySpec(spec)).ToList();

            foreach (var kvp in columnSearches)
            {
                var columnIndex = int.Parse(kvp.Key.Split('[', ']')[1]);
                var search = kvp.Value;
                search = search.Trim('^', '$');

                if(string.IsNullOrEmpty(search))
                {
                    continue;
                }

                switch (columnIndex)
                {
                    case 0:
                        people = people.Where(p => p.ID.ToString().Contains(search)).ToList();
                        break;
                    case 1:
                        people = people.Where(p => (p.DEPT_CODE ?? "").ToString().Contains(search)).ToList();
                        break;
                    case 2:
                        people = people.Where(p => p.FIRST_NAME.Contains(search)).ToList();
                        break;
                    case 3:
                        people = people.Where(p => p.LAST_NAME.Contains(search)).ToList();
                        break;
                }
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "0":
                        people = sortDirection == "asc" ? people.OrderBy(p => p.ID).ToList() : people.OrderByDescending(p => p.ID).ToList();
                        break;
                    case "1":
                        people = sortDirection == "asc" ? people.OrderBy(p => p.DEPT_CODE).ToList() : people.OrderByDescending(p => p.DEPT_CODE).ToList();
                        break;
                    case "2":
                        people = sortDirection == "asc" ? people.OrderBy(p => p.FIRST_NAME).ToList() : people.OrderByDescending(p => p.FIRST_NAME).ToList();
                        break;
                    case "3":
                        people = sortDirection == "asc" ? people.OrderBy(p => p.LAST_NAME).ToList() : people.OrderByDescending(p => p.LAST_NAME).ToList();
                        break;
                }
            }

            var totalRecords = people.Count;

            people = people.Skip(start).Take(length).ToList();

            var formattedData = people.Select(item => new
            {
                ID = item.ID,
                DEPT_CODE = item.DEPARTMENT_DETAILS?.DEPT_NAME,
                FIRST_NAME = item.FIRST_NAME,
                LAST_NAME = item.LAST_NAME
            });

            var response = new 
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = people,
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetEmployeeListUniqueValues()
        {
            var people = (await _peopleRepository.GetListAsync()).ToList();

            var uniqueValues = new Dictionary<string, List<string>>();

            uniqueValues["ID"] = people.Select(p => p.ID.ToString()).Distinct().ToList();
            uniqueValues["DEPT_CODE"] = people.Select(p => p.DEPT_CODE).Distinct().ToList();
            uniqueValues["FIRST_NAME"] = people.Select(p => p.FIRST_NAME).Distinct().ToList();
            uniqueValues["LAST_NAME"] = people.Select(p => p.LAST_NAME).Distinct().ToList();

            return Json(uniqueValues, JsonRequestBehavior.AllowGet);
        }
    }
}