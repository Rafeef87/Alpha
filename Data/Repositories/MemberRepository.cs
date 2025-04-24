
using Data.Context;
using Data.Entities;
using Domain.Models;

namespace Data.Repositories;

public interface IMemberRepository : IBaseRepository<MemberEntity, Member>
{
}

public class MemberRepository(DataContext context) : BaseRepository<MemberEntity, Member>(context), IMemberRepository
{
}