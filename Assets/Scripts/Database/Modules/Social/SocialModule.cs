using PlayFab;
using System.Collections;

public class SocialModule : Module
{
    public IEnumerator GetFriends()
    {
        yield return _manager.StartRequest();

        PlayFabClientAPI.GetFriendsList(new(), res =>
        {
            _manager.InvokeOnGetFriends(res.Friends);
            _manager.EndRequest();
        }, _manager.OnRequestError);
    }

    public IEnumerator AddFriend(string username)
    {
        yield return _manager.StartRequest();

        PlayFabClientAPI.AddFriend(new()
        {
            FriendTitleDisplayName = username
        }, res => _manager.EndRequest(), _manager.OnRequestError);
    }

    public IEnumerator RemoveFriend(string id)
    {
        yield return _manager.StartRequest();

        PlayFabClientAPI.RemoveFriend(new()
        {
            FriendPlayFabId = id
        }, res => _manager.EndRequest(), _manager.OnRequestError);
    }
}