using System.ComponentModel.DataAnnotations;

namespace DataTables_ServerSide.Models.Entities
{
    public class TBL_DEPARTMENT : BaseEntity
    {
        [Key]
        public string DEPT_CODE { get; set; }
        public string DEPT_NAME { get; set; }
    }
}