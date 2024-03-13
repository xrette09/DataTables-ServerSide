using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTables_ServerSide.Models.Entities
{
    public class TBL_EMPLOYEE : BaseEntity
    {
        [Key]
        public int ID { get; set; }
        public string DEPT_CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }

        [ForeignKey("DEPT_CODE")]
        public TBL_DEPARTMENT DEPARTMENT_DETAILS { get; set; }
    }
}