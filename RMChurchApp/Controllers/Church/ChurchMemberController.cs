using Microsoft.AspNetCore.Mvc;
using RMChurchApp.Data.Models;
using RMChurchApp.Data.Models.Church;
using RMChurchApp.Data.Repositories;

namespace RMChurchApp.Controllers.Church
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChurchMemberController : ControllerBase
    {
        private readonly ChurchMemberRepository _memberRepository;

        public ChurchMemberController(ChurchMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        // GET: api/ChurchMember
        [HttpGet]
        public async Task<IActionResult> GetAllMembers()
        {
            var members = await _memberRepository.GetAllMembersAsync();
            return Ok(new ApiResponse<IEnumerable<ChurchMember>>(members, "Members retrieved successfully"));
        }

        // GET: api/ChurchMember/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberById(int id)
        {
            var member = await _memberRepository.GetMemberByIdAsync(id);
            if (member == null)
                return NotFound(ApiResponse<ChurchMember>.Fail("Member not found", 404));

            return Ok(new ApiResponse<ChurchMember>(member, "Member retrieved successfully"));
        }

        // POST: api/ChurchMember
        [HttpPost]
        public async Task<IActionResult> AddMember([FromBody] ChurchMember member)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Invalid member data", 400));

            var newId = await _memberRepository.AddMemberAsync(member);
            member.member_id = (int)newId;

            return Ok(new ApiResponse<ChurchMember>(member, "Member added successfully"));
        }

        // PUT: api/ChurchMember/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] ChurchMember member)
        {
            if (id != member.member_id)
                return BadRequest(ApiResponse<string>.Fail("Member ID mismatch", 400));

            var result = await _memberRepository.UpdateMemberAsync(member);
            if (result == 0)
                return NotFound(ApiResponse<string>.Fail("Update failed. Member not found", 404));

            return Ok(new ApiResponse<ChurchMember>(member, "Member updated successfully"));
        }

        // DELETE: api/ChurchMember/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var result = await _memberRepository.DeleteMemberAsync(id);
            if (result == 0)
                return NotFound(ApiResponse<string>.Fail("Member not found or already deleted", 404));

            return Ok(ApiResponse<string>.Warning("Member deleted successfully"));
        }
    }
}
