using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineStore
{
    public interface IMagazine
    {
        Task<TokenData> GettokendataAsync();
        Task<string> GetSubscriberDatas();
        Task<string> GetMagazinecodeResult(List<string> subscriberIds, string token);
        Task<Subscribers> GetSubscribers(string token);
        Task<Magazines> GetMagazines(string token, string categories);
        Task<Category> GetCategories(string token);

    }
}
