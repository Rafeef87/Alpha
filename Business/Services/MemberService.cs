using Business.Models;
using Data.Entities;
using Data.Extensions;
using Data.Repositories;
using Domain.Models;

namespace Business.Services;

public interface IMemberService
{
    Task<MemberResult> CreateMemberAsync(AddMemberFormData formData);
    Task<MemberResult> DeleteMemberAsync(string id);
    Task<MemberResult> GetAllMembersAsync();
    Task<MemberResult> UpdateMemberAsync(EditMemberFormData formData);
}

public class MemberService(IMemberRepository memberRepository) : IMemberService
{
    private readonly IMemberRepository _memberRepository = memberRepository;


    public async Task<MemberResult> CreateMemberAsync(AddMemberFormData formData)
    {
        if (formData == null)
            return new MemberResult { Succeeded = false, StatusCode = 400, Error = "Not all required field are supplied." };

        var memberEntity = formData.MapTo<MemberEntity>();
        var result = await _memberRepository.AddAsync(memberEntity);

        return result.Succeeded
            ? new MemberResult { Succeeded = true, StatusCode = 200 }
            : new MemberResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }
    public async Task<MemberResult> GetAllMembersAsync()
    {
        var result = await _memberRepository.GetAllAsync();
        if (!result.Succeeded)
            return new MemberResult { Succeeded = false, StatusCode = 500, Error = result.Error };
        
        if (result.Result == null)
            return new MemberResult { Succeeded = false, StatusCode = 500, Error = "No members found or result is null" };

        var members = result.Result.Select(m => m.MapTo<Member>()).ToList();
        return new MemberResult
        {
            Succeeded = true,
            StatusCode = 200,
            Result = members
        };
    }


    public async Task<MemberResult> UpdateMemberAsync(EditMemberFormData formData)
    {
        if (formData == null)
            return new MemberResult { Succeeded = false, StatusCode = 400, Error = "Not all required field are supplied." };

        var memberEntity = formData.MapTo<MemberEntity>();
        var result = await _memberRepository.UpdateAsync(memberEntity);

        return result.Succeeded
            ? new MemberResult { Succeeded = true, StatusCode = 200 }
            : new MemberResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }
    public async Task<MemberResult> DeleteMemberAsync(string id)
    {

        var memberEntity = new MemberEntity { Id = id };
        var result = await _memberRepository.DeleteAsync(memberEntity);
        return result.Succeeded
            ? new MemberResult { Succeeded = true, StatusCode = 200 }
            : new MemberResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }
}

