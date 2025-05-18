using System.ComponentModel.DataAnnotations;

namespace API_11_01.Model
{
    public  class School
    {

        [Required]
        public  int Id { get; set; }
        public  int salary {  get; set; }
    }
}
