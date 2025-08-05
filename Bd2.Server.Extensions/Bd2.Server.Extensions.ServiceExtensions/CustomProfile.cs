using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.Internal;
using Bd2.Server.Common.Proto.Net;
using Bd2.Server.Model.DAO;
using Bd2.Server.Model.DAO.Game;
using Google.Protobuf.Collections;

namespace Bd2.Server.Extensions.ServiceExtensions;

public class CustomProfile : Profile
{
	public CustomProfile()
	{
		CreateMap<NoticeInfoDao, NoticeDBInfo>();
		CreateMap<CostumeInfoDao, CostumeDBInfo>().ForMember((CostumeDBInfo x) => x.PotentialId, delegate(IMemberConfigurationExpression<CostumeInfoDao, CostumeDBInfo, RepeatedField<int>> opt)
		{
			opt.MapFrom((CostumeInfoDao src) => ConvertStringToIntArray(src.PotentialId));
		});
		CreateMap<EquipmentOption, EquipOptionInfo>();
		CreateMap<CharInfoDao, CharDBInfo>();
		CreateMap<ItemInfoDao, ItemDBInfo>();
		CreateMap<ItemSpecialInfoDao, ItemDBInfo>();
		CreateMap<UserInfoDao, UserDBInfo>();
		CreateMap<DeckInfoDao, DeckDBInfo>();
		this.Internal().ForAllMaps(delegate(TypeMap a, IMappingExpression b)
		{
			b.ForAllMembers(delegate(IMemberConfigurationExpression opt)
			{
				opt.Condition((object src, object dest, object sourceMember) => sourceMember != null);
			});
		});
	}

	private int[] ConvertStringToIntArray(string input)
	{
		input = input.Trim('[', ']');
		string[] array = input.Split(',');
		List<int> list = new List<int>();
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			if (int.TryParse(array2[i].Trim(), out var result))
			{
				list.Add(result);
				continue;
			}
			throw new ArgumentException("字符串包含非整数值", "input");
		}
		return list.ToArray();
	}
}
