using System.Collections.Concurrent;

namespace WatchMatchApi.Services
{
    public class GroupTrackerService
    {
        private readonly ConcurrentDictionary<string, HashSet<string>> _groups = new();
        private readonly DelayedActionScheduler _deletionScheduler;

        public event Action<string>? GroupDeleted;

        public GroupTrackerService(DelayedActionScheduler deletionScheduler)
        {
            _deletionScheduler = deletionScheduler;
            _deletionScheduler.ActionFired += group => GroupDeleted?.Invoke(group);
        }

        public void CreateGroup(string groupName, TimeSpan? time=null)
        {
            _groups.TryAdd(groupName, []);
            ScheduleDeletion(groupName, time ?? TimeSpan.FromSeconds(30));
        }

        public bool AddUserToGroup(string groupName, string userId)
        {
            if (!_groups.ContainsKey(groupName))
            {
                return false;
            }
            _groups.AddOrUpdate(
                groupName,
                _ =>
                {
                    _deletionScheduler.Cancel(groupName);
                    return [userId];
                },
                (_, existing) =>
                {
                    lock (existing) { existing.Add(userId); }
                    _deletionScheduler.Cancel(groupName);
                    return existing;
                });
            return true;
        }

        private void ScheduleDeletion(string groupName, TimeSpan? time=null)
        {
            _deletionScheduler.Schedule(groupName, () => _groups.TryRemove(groupName, out _), time ?? _deletionScheduler.DefaultDelay);
        }

        public void RemoveUserFromGroup(string groupName, string userId)
        {
            if (_groups.TryGetValue(groupName, out var set))
            {
                lock (set) { set.Remove(userId); }

                if (set.Count == 0)
                {
                    ScheduleDeletion(groupName);
                }
            }
        }

        public HashSet<string> GetUsersInGroup(string groupName)
        {
            if (_groups.TryGetValue(groupName, out var set))
                return [.. set];

            return [];
        }
    }

}
