namespace Edelstein.Core.Gameplay.Social.Guild
{
    public enum GuildResultType : byte
    {
        LoadGuild_Done = 0x1C,
        CheckGuildName_Available = 0x1D,
        CheckGuildName_AlreadyUsed = 0x1E,
        CheckGuildName_Unknown = 0x1F,
        CreateGuildAgree_Reply = 0x20,
        CreateGuildAgree_Unknown = 0x21,
        CreateNewGuild_Done = 0x22,
        CreateNewGuild_AlreayJoined = 0x23,
        CreateNewGuild_GuildNameAlreayExist = 0x24,
        CreateNewGuild_Beginner = 0x25,
        CreateNewGuild_Disagree = 0x26,
        CreateNewGuild_NotFullParty = 0x27,
        CreateNewGuild_Unknown = 0x28,
        JoinGuild_Done = 0x29,
        JoinGuild_AlreadyJoined = 0x2A,
        JoinGuild_AlreadyFull = 0x2B,
        JoinGuild_UnknownUser = 0x2C,
        JoinGuild_Unknown = 0x2D,
        WithdrawGuild_Done = 0x2E,
        WithdrawGuild_NotJoined = 0x2F,
        WithdrawGuild_Unknown = 0x30,
        KickGuild_Done = 0x31,
        KickGuild_NotJoined = 0x32,
        KickGuild_Unknown = 0x33,
        RemoveGuild_Done = 0x34,
        RemoveGuild_NotExist = 0x35,
        RemoveGuild_Unknown = 0x36,
        InviteGuild_BlockedUser = 0x37,
        InviteGuild_AlreadyInvited = 0x38,
        InviteGuild_Rejected = 0x39,
        AdminCannotCreate = 0x3A,
        AdminCannotInvite = 0x3B,
        IncMaxMemberNum_Done = 0x3C,
        IncMaxMemberNum_Unknown = 0x3D,
        ChangeLevelOrJob = 0x3E,
        NotifyLoginOrLogout = 0x3F,
        SetGradeName_Done = 0x40,
        SetGradeName_Unknown = 0x41,
        SetMemberGrade_Done = 0x42,
        SetMemberGrade_Unknown = 0x43,
        SetMemberCommitment_Done = 0x44,
        SetMark_Done = 0x45,
        SetMark_Unknown = 0x46,
        SetNotice_Done = 0x47,
        InsertQuest = 0x48,
        NoticeQuestWaitingOrder = 0x49,
        SetGuildCanEnterQuest = 0x4A,
        IncPoint_Done = 0x4B,
        ShowGuildRanking = 0x4C,
        GuildQuest_NotEnoughUser = 0x4D,
        GuildQuest_RegisterDisconnected = 0x4E,
        GuildQuest_NoticeOrder = 0x4F,
        Authkey_Update = 0x50,
        SetSkill_Done = 0x51,
        ServerMsg = 0x52
    }
}