using Microsoft.AspNetCore.Mvc;
using RMChurchApp.Data.Models;
using RMChurchApp.Data.Models.Church;
using RMChurchApp.Data.Repositories;
using System.Net;

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

        [HttpGet]
        public async Task<IActionResult> GetAllMembers()
        {
            try
            {
                var members = await _memberRepository.GetAllMembersAsync();
                return Ok(new ApiResponse<IEnumerable<ChurchMember>>(members, "Members retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ApiResponse<string>.Fail($"Error retrieving members: {ex.Message}", 500));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberById(int id)
        {
            try
            {
                var member = await _memberRepository.GetMemberByIdAsync(id);
                if (member == null)
                    return NotFound(ApiResponse<ChurchMember>.Fail("Member not found", 404));

                return Ok(new ApiResponse<ChurchMember>(member, "Member retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ApiResponse<string>.Fail($"Error retrieving member: {ex.Message}", 500));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMember([FromBody] ChurchMember member)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Invalid member data", 400));

            try
            {
                var newId = await _memberRepository.AddMemberAsync(member);
                member.member_id = (int)newId;

                return Ok(new ApiResponse<ChurchMember>(member, "Member added successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ApiResponse<string>.Fail($"Error adding member: {ex.Message}", 500));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] ChurchMember member)
        {
            if (id != member.member_id)
                return BadRequest(ApiResponse<string>.Fail("Member ID mismatch", 400));

            try
            {
                var result = await _memberRepository.UpdateMemberAsync(member);
                if (result == 0)
                    return NotFound(ApiResponse<string>.Fail("Update failed. Member not found", 404));

                return Ok(new ApiResponse<ChurchMember>(member, "Member updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ApiResponse<string>.Fail($"Error updating member: {ex.Message}", 500));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            try
            {
                var result = await _memberRepository.DeleteMemberAsync(id);
                if (result == 0)
                    return NotFound(ApiResponse<string>.Fail("Member not found or already deleted", 404));

                return Ok(ApiResponse<string>.Warning("Member deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ApiResponse<string>.Fail($"Error deleting member: {ex.Message}", 500));
            }
        }
    }
}
