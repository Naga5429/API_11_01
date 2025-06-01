using API_11_01.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;
using System.Data;

using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_11_01.Controllers
{
    // [Authorize]
    //[SimpleErrorFilter]
   // [ServiceFilter(typeof(CustomAuthenticationFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ApiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: api/<ApiController>
        public string Conn = "Data Source=OCS-L00058\\SQLEXPRESS;Initial Catalog=Student;Integrated Security=True";

        [HttpGet("LinqAttemp1")]
        public Object LinqAttemp1()
        {
            List<Temp1> lst1 = new List<Temp1>();
            lst1.Add(new Temp1 { temp1name = "Nagaraj", temp1age = "25", temp1Id = 1 });
            lst1.Add(new Temp1 { temp1name = "araj", temp1age = "23", temp1Id = 2});

            List<Temp2> lst2 = new List<Temp2>();
            lst2.Add(new Temp2 { temp2name = "Nagaraj", temp2age = "25", temp2Id = 1 });
            lst2.Add(new Temp2 { temp2name = "araj", temp2age = "23", temp2Id = 2 });
            var test = lst1.Join(lst2, a => a.temp1Id, b => b.temp2Id, (a, b) => new { a, b }).ToList();
            var test2 = lst1.GroupJoin(lst2, a => a.temp1Id, b => b.temp2Id, (a, result)=> new {a,result}).ToList();
            return test2;


        }

        //[HttpGet("GenerateJwtToken")]
        //public string GenerateJwtToken(string username)
        //{
        //    var jwtSettings = _configuration.GetSection("Jwt");
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: jwtSettings["Issuer"],
        //        audience: jwtSettings["Audience"],
        //        claims: new[] { new Claim(ClaimTypes.Name, username) },
        //        expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
        //        signingCredentials: creds);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
        //[HttpGet("Test")]
        //public object Test() {
        //    var a = GenerateJwtToken("hh");
        //    var hh = new { Name = "Nagar" };
        //var table1= new List<object>();
        //    table1.Add(new { Name="Nagar",Age=25});
        //    table1.Add(new { Name = "Nagar", Age = 25 });
        //    table1.Add(hh);
        //    Object hu=JsonConvert.SerializeObject(table1);
        //    return hu;


         
        //}
       
        [HttpGet("Linq")]
        public string Linq()
        {
            var table1 = new List<Temp1>();
            table1.Add(new Temp1 { temp1Id = 1, temp1email = "Naga", temp1age = "23", temp1name = "Nagaraj" });
            table1.Add(new Temp1 { temp1Id = 2, temp1email = "raj", temp1age = "23", temp1name = "raj" });
            table1.Add(new Temp1 { temp1Id = 4, temp1email = "raj", temp1age = "23", temp1name = "rajhhh" });
            var Subject = new List<Subject>();
            Subject.Add(new Subject { Id = 1, Tamil = 98, English = 80, Maths =60 });
            Subject.Add(new Subject { Id = 2, Tamil = 88, English = 70, Maths = 70 });
            Subject.Add(new Subject { Id = 3, Tamil = 78, English = 60, Maths = 80 });
            

            var table2 = new List<Temp2>();
            table2.Add(new Temp2 { temp2Id = 1, temp2email = "Naga2", temp2age = "23", temp2name = "Nagaraj2" });
            table2.Add(new Temp2 { temp2Id = 2, temp2email = "raj2", temp2age = "23", temp2name = "raj2" });
            table2.Add(new Temp2 { temp2Id = 3, temp2email = "raj3", temp2age = "23", temp2name = "raj3" });
            var table3 = new[] { new { id = 3, Name = "Naga" } };
            var table4 = new List<object>();
            table4.Add(new { id = 3, Name = "Naga" });
            var hh = table3[0].Name;
            //ddf
            //var innerjoin = table1.Join(table2, a => a.temp1Id, b => b.temp2Id, (a, b) => new { a, b }).Join(table4, c => c.a.temp1Id, d => d.id, (c, d) => new { c,d}).ToList();
            //var innerjoin = from m in table1
            //                join t2 in Subject on m.temp1Id equals t2.Id into marks
            //                select new
            //                {
            //                    m.temp1name,
            //                    Score = marks.Select(x => new
            //                    {
            //                        x.Tamil,x.English,x.Maths
            //                    })
            //                };

            var innerjoin = table1.GroupJoin(Subject, a => a.temp1Id, b => b.Id, (a, result) => 
            
            new { a, Score = result.ToList(),totalmark=result.Sum(y=>(y.Tamil+y.English+y.Maths))}).ToList();

            string hu = JsonConvert.SerializeObject(innerjoin);
            return hu;



        }


        [HttpPost("InsertStudent")]
        public object InsertStudent(Nagaraj st)
        {

            SqlConnection conn = new SqlConnection(Conn);
          
            Response.Headers.Add("Authorization", "hhh");
            var jsondata = JsonConvert.SerializeObject(st);
            SqlCommand cmd = new SqlCommand("SpInsMultipleRow", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@JsonData", jsondata);
            conn.Open();
            int o = cmd.ExecuteNonQuery();
            return Response.Headers;
        }

        [HttpGet]
        public List<Student> GetData()
        {
           
            SqlConnection conn = new SqlConnection(Conn);
            conn.Open();
            List<Student> student = new List<Student>();
            SqlCommand cmd = new SqlCommand("SPGetStudentData", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            Response.Headers.Append("X-Custom-Header", "MyHeaderValue");
            while (dr.Read())
            {
                var dt = new Student
                {
                    Id = (int)dr["Id"],
                    Name = dr["Name"].ToString(),
                    Email = dr["Email"].ToString()
                   
                };
                student.Add(dt);
            }
            return student;  
        }
        //hh
        [HttpGet("GetDataById")]
        public List<Student> GetDataById(int id)
        {

            SqlConnection conn = new SqlConnection(Conn);
            conn.Open();
            List<Student> student = new List<Student>();
            SqlCommand cmd = new SqlCommand($"Select Name,Email from Tbl_student where id={id}", conn);
           // cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            Response.Headers.Append("X-Custom-Header", "MyHeaderValue");
            while (dr.Read())
            {
                var dt = new Student
                {
                    Name = dr["Name"].ToString(),
                    Email = dr["Email"].ToString()

                };
                student.Add(dt);
            }
            return student;
        }

        [HttpGet("MultipleRow")]
        public List<Object> GetMultipleRowData()
        {
            SqlConnection conn = new SqlConnection(Conn);
            conn.Open();
            List<Object> student = new List<Object>();
            SqlCommand cmd = new SqlCommand("SPGetStudentData", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            Da.Fill(ds);
            if (ds.Tables.Count > 0)
            {
                DataTable st1 = ds.Tables[0];
                DataTable st2 = ds.Tables[1];
                foreach (DataRow dr in st1.Rows) {

                    student.Add(new Student
                    {
                        Name = dr["Name"].ToString()
                    });
             }
                foreach (DataRow dr in st2.Rows)
                {

                    student.Add(new School
                    {
                        salary = Convert.ToInt32(dr["salary"])
                       
                    });
                }
            }
            return student;
           
        }

        [HttpGet("GetDataWithoutDataSet")]
        public string GetDataWithoutDataSet()
        {
            using (SqlConnection conn = new SqlConnection(Conn))
            {
                SqlCommand cmd = new SqlCommand("SPGetDataUsingJSON", conn); // You can replace this with your stored procedure
                conn.Open();
                cmd.CommandType= CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                string gg=JsonConvert.SerializeObject(dt,Formatting.Indented);
                Console.WriteLine(gg);
                return gg;
            }
           
        }


        [HttpGet("GetDynamicData")]
        public List<object[]> GetDataDynamic()
        {
            var result = new List<object[]>();

            using (SqlConnection conn = new SqlConnection(Conn))
            {
                SqlCommand cmd = new SqlCommand("SPGetStudentData", conn); // You can replace this with your stored procedure
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var row = new object[reader.FieldCount];

                    // Loop through columns dynamically and store values in an array
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[i] = reader.GetValue(i);
                    }

                    result.Add(row);
                }
            }

            return result;
        }


        // GET api/<ApiController>/5
        [HttpGet("{id}")]
        public List<Student> Getby(int id)
        {
            List<Student> stu = new List<Student>();
            SqlConnection con = new SqlConnection(Conn);
            con.Open();
            SqlCommand cmd = new SqlCommand($"Select * from Tbl_Student where id={id}", con);
            SqlDataReader dr = cmd.ExecuteReader();
           
            while (dr.Read())
            {
                 var st = new Student
                {
                    Name = dr["Name"].ToString(),
                    Email = dr["email"].ToString(),
                    phone = dr["Phone"].ToString()

                };
                stu.Add(st);

            }


            return stu;
        }

       

        // POST api/<ApiController>
        [HttpPost]
        public int InsertData(List<Student> st)
        {
            
            SqlConnection con = new SqlConnection(Conn);
            var jsondata = JsonConvert.SerializeObject(st);
            SqlCommand cmd = new SqlCommand("SpInsMultipleRow", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@JsonData", jsondata);
            con.Open();
            int o=cmd.ExecuteNonQuery();
            return 0;
        }

        // PUT api/<ApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        

    }
}
