using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

using System;

namespace SB_Content.OilMan
{
    internal sealed class Blank_User : IUser
    {
        public Blank_User() { }
        public string AvatarId => "";

        public string Discriminator => throw new NotImplementedException();

        public ushort DiscriminatorValue => throw new NotImplementedException();

        public bool IsBot => true;

        public bool IsWebhook => false;

        public string Username => "";

        public UserProperties? PublicFlags => throw new NotImplementedException();

        public string GlobalName => "";

        public DateTimeOffset CreatedAt => throw new NotImplementedException();

        public ulong Id => 0;

        public string Mention => throw new NotImplementedException();

        public UserStatus Status => throw new NotImplementedException();

        public IReadOnlyCollection<ClientType> ActiveClients => throw new NotImplementedException();

        public IReadOnlyCollection<IActivity> Activities => throw new NotImplementedException();

        public Task<IDMChannel> CreateDMChannelAsync(RequestOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public string GetAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        {
            throw new NotImplementedException();
        }

        public string GetDefaultAvatarUrl()
        {
            throw new NotImplementedException();
        }
    }
}
