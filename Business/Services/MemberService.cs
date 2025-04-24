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
    Task<MemberResult> GetAllMembers();
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
    public async Task<MemberResult> GetAllMembers()
    {
        var result = await _memberRepository.GetAllAsync();
        return result.MapTo<MemberResult>();
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

