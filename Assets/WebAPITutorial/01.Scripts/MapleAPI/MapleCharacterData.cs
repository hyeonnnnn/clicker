using System;
using System.Collections.Generic;

namespace MapleAPI
{
    // ocid 조회 응답
    [Serializable]
    public class OcidResponse
    {
        public string ocid;
    }

    // 캐릭터 기본 정보 응답
    [Serializable]
    public class CharacterBasicResponse
    {
        public string date;
        public string character_name;
        public string world_name;
        public string character_gender;
        public string character_class;
        public string character_class_level;
        public int character_level;
        public long character_exp;
        public string character_exp_rate;
        public string character_guild_name;
        public string character_image;
        public string character_date_create;
        public bool access_flag;
        public int liberation_quest_clear_flag;
    }

    // 캐릭터 능력치 응답
    [Serializable]
    public class CharacterStatResponse
    {
        public string date;
        public string character_class;
        public List<StatDetail> final_stat;
        public int remain_ap;
    }

    [Serializable]
    public class StatDetail
    {
        public string stat_name;
        public string stat_value;
    }

    // 통합 캐릭터 데이터 (UI에서 사용)
    [Serializable]
    public class MapleCharacterData
    {
        public string Ocid;
        public CharacterBasicResponse Basic;
        public CharacterStatResponse Stat;

        // 편의 프로퍼티
        public string Name => Basic?.character_name ?? "";
        public string World => Basic?.world_name ?? "";
        public string Class => Basic?.character_class ?? "";
        public int Level => Basic?.character_level ?? 0;
        public string ImageUrl => Basic?.character_image ?? "";
        public string Guild => Basic?.character_guild_name ?? "";
        public string ExpRate => Basic?.character_exp_rate ?? "0";

        // 특정 스탯 값 가져오기
        public string GetStatValue(string statName)
        {
            if (Stat?.final_stat == null) return "0";

            foreach (var stat in Stat.final_stat)
            {
                if (stat.stat_name == statName)
                    return stat.stat_value;
            }
            return "0";
        }
    }
}
