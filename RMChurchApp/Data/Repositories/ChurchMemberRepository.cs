using RMChurchApp.Data.Models.Church;
using RMChurchApp.Data; // for IDBManager
using System.Data;

namespace RMChurchApp.Data.Repositories
{
    public class ChurchMemberRepository
    {
        private readonly IDBManager _dbManager;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ChurchMemberRepository(IDBManager dbManager, IConfiguration configuration)
        {
            _dbManager = dbManager;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DBCon")!;
        }

        // ✅ Get All Members
        public async Task<IEnumerable<ChurchMember>> GetAllMembersAsync()
        {
            var query = "SELECT * FROM church_members";
            return await _dbManager.QueryAsync<ChurchMember>(_connectionString, query);
        }

        // ✅ Get Member by ID
        public async Task<ChurchMember?> GetMemberByIdAsync(int memberId)
        {
            var query = "SELECT * FROM church_members WHERE member_id = @Id";
            return await _dbManager.QuerySingleAsync<ChurchMember>(_connectionString, query, new { Id = memberId });
        }

        // ✅ Add Member (Insert and return new ID)
        public async Task<long> AddMemberAsync(ChurchMember member)
        {
            var query = @"
                INSERT INTO church_members
                (member_name, sur_name, mobile_number, age, gender, marital_status, city, area, land_mark, occupation, religion,
                 reference_name, reference_type, reference_mobile, reference_area, membership, remarks, created_by, created_date,
                 followup_mode, is_followup_active)
                VALUES
                (@member_name, @sur_name, @mobile_number, @age, @gender, @marital_status, @city, @area, @land_mark, @occupation, @religion,
                 @reference_name, @reference_type, @reference_mobile, @reference_area, @membership, @remarks, @created_by, @created_date,
                 @followup_mode, @is_followup_active);
                SELECT LAST_INSERT_ID();";

            return await _dbManager.ExecuteScalarAsync(_connectionString, query, member);
        }

        // ✅ Update Member
        public async Task<int> UpdateMemberAsync(ChurchMember member)
        {
            var query = @"
                UPDATE church_members SET
                    member_name = @member_name,
                    sur_name = @sur_name,
                    mobile_number = @mobile_number,
                    age = @age,
                    gender = @gender,
                    marital_status = @marital_status,
                    city = @city,
                    area = @area,
                    land_mark = @land_mark,
                    occupation = @occupation,
                    religion = @religion,
                    reference_name = @reference_name,
                    reference_type = @reference_type,
                    reference_mobile = @reference_mobile,
                    reference_area = @reference_area,
                    membership = @membership,
                    remarks = @remarks,
                    updated_by = @updated_by,
                    updated_date = @updated_date,
                    followup_mode = @followup_mode,
                    is_followup_active = @is_followup_active
                WHERE member_id = @member_id";

            return await _dbManager.ExecuteAsync(_connectionString, query, member);
        }

        // ✅ Delete Member
        public async Task<int> DeleteMemberAsync(int memberId)
        {
            var query = "DELETE FROM church_members WHERE member_id = @Id";
            return await _dbManager.ExecuteAsync(_connectionString, query, new { Id = memberId });
        }
    }
}
