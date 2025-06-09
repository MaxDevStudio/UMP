using Microsoft.AspNetCore.Authorization; // ��������� ��� �����������
using Microsoft.AspNetCore.Mvc; // ��������� ��� ��������� API-����������

namespace UMP_API.Controllers // ������ ���� ��� ����������
{
    // �������, ���� ������� ���� �� API-���������
    [ApiController]
    // �������, ���� ������� ������� ��� ��� ������ ����������
    [Route("api/[controller]")]
    public class MarketController : ControllerBase // ������� ���� ��� API-����������
    {
        // �������, ���� ������ ����������� ��� ������� �� ������
        [HttpGet]
        [Authorize]
        public IActionResult GetItems()
        {
            // ��������� ��������� ������ ������ (���� �� ��� �����)
            var items = new[] { "App1", "App2", "App3" };
            return Ok(items); // ��������� 200 OK �� ������
        }
    }
}