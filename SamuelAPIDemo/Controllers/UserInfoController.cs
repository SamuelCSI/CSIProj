using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using SamuelAPIDemo.Data;
using SamuelAPIDemo.Models;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SamuelAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly YourDbContext _context;

        public UserInfoController(YourDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// 取得所有使用者資料
        /// </summary>
        /// <returns>回傳使用者帳號/密碼/電話/Email</returns>
        /// <response code="200">成功</response>
        /// <response code="400">格式錯誤</response>
        /// <response code="401">授權失敗 請先進行登入</response>
        /// <response code="403">使用者權限不足</response>
        /// <response code="500">系統錯誤</response> 
        [HttpGet]
        public IActionResult Get()
        {
            string strResult = "No Data";
            try
            {
                // 驗証規則,從MYSQL
                // 連接到 MySQL 數據庫並驗證用戶名和密碼
                string strConn = "server=127.0.0.1;port=32768;database=UserInfo;user=myuser;password=mypassword";
                using (var connection = new MySqlConnection(strConn))
                {
                    connection.Open();
                    string strQuery = "SELECT * FROM User_table";

                    using (MySqlCommand cmd = new MySqlCommand(strQuery, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            strResult = JsonConvert.SerializeObject(dt);
                        }
                    }
                }
            }
            catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok(strResult);
        }

        // GET api/<UserInfoController>/5
        /// <summary>
        /// 取得單一使用者資料
        /// </summary>
        /// <param name="id">編號</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// 新增一位使用者
        /// </summary>
        /// <param name="SamuelCC">使用者名稱</param>
        /// <returns>User information added successfully</returns>
        // POST api/<UserInfoController>
        //[SwaggerOperation(Tags = new[] { "Front/Form" })]
        [HttpPost]
        // public void Post([FromBody] string value)
        public async Task<IActionResult> AddUserInfo([FromBody] UserInfo userInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.User_table.Add(userInfo);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok("User information added successfully");

        }

        /// <summary>
        /// 更新單一使用者名稱
        /// </summary>
        /// <param name="id">使用者ID代號</param>
        /// <param name="value">姓名</param>
        // PUT api/<UserInfoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// 刪除單一使用者資料
        /// </summary>
        /// <param name="id">使用者ID代號</param>
        // DELETE api/<UserInfoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

}
