namespace Blazor.Server.Hubs
{
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connections =
            new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        //取得用户Key所有连接
        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }
        //取得用户Key所有连接（批量）
        public IEnumerable<string> GetConnections(IEnumerable<T> keys)
        {
            List<string> result = new List<string>();
            foreach (var key in keys)
            {
                HashSet<string> connections;
                if (_connections.TryGetValue(key, out connections))
                {
                    result.AddRange(connections);
                }
            }
            return result;
        }

        //根据connectionId取得用户key
        public KeyValuePair<T, HashSet<string>>? GetUserByConnectionId(string connectionId)
        {
            foreach (var item in _connections)
            {
                if (item.Value.Contains(connectionId))
                {
                    return item;
                }
            }
            return null;
        }

        //根据connectionId取得用户的其他connectionId
        public T GetUserConnectionId(string connectionId)
        {
            foreach (var item in _connections)
            {
                if (item.Value.Contains(connectionId))
                {
                    return item.Key;
                }
            }
            return default(T);
        }


        //取得在线用户
        public IEnumerable<T> GetAllUsers()
        {
            return _connections.Keys.ToList<T>();
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }

        //无UserID删除
        public void Remove(string connectionId)
        {
            lock (_connections)
            {
                //采用倒序，循环删除,如果用户没有connectionId，删除整个用户
                for (int i = _connections.Count - 1; i >= 0; i--)
                {
                    var key = _connections.ElementAt(i).Key;
                    HashSet<string> connections;
                    if (!_connections.TryGetValue(key, out connections))
                    {
                        return;
                    }

                    lock (connections)
                    {
                        connections.Remove(connectionId);

                        if (connections.Count == 0)
                        {
                            _connections.Remove(key);
                        }
                    }
                }
            }
        }
    }
}
