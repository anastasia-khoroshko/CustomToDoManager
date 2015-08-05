using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Infrastructure;
using CustomToDoManager.SlowService;
using CustomToDoManager.FastService;
using System.Diagnostics;

namespace CustomToDoManager
{
    public class CustomTaskManager : ToDo.Infrastructure.IToDoManager
    {
        private static Service1Client fastClient;
        private static ToDoManagerClient slowClient;
        private static List<IToDoItem> items;
        private bool isAdded;
        
        public CustomTaskManager()
        {
            fastClient = new Service1Client();
            slowClient = new ToDoManagerClient();
            items = new List<IToDoItem>();
            isAdded = false;
        }
        public async void CreateToDoItem(IToDoItem todo)
        {
            var item = new CustomTask()
            {
                Name = todo.Name,
                UserId = todo.UserId,
                ToDoId = todo.ToDoId,
                IsCompleted = todo.IsCompleted
            };
            items.Add(item as IToDoItem);
            isAdded = true;
            await fastClient.SyncToDoItemsAsync(item);
        }

        public int CreateUser(string name)
        {
            if (items == null) items.Clear();
            return slowClient.CreateUserAsync(name).Result;
        }

        public async void DeleteToDoItem(int todoItemId)
        {
            int index = items.FindIndex(i => i.ToDoId == todoItemId);
            if (index != -1)
                items.RemoveAt(index);
            await fastClient.SyncDeleteItemAsync(todoItemId);
        }

        public List<IToDoItem> GetTodoList(int userId)
        {
            if (isAdded==true || items == null || items.Count() == 0)
            {
                var list = slowClient.GetTodoList(userId);
                if (list != null || list.Count() != 0)
                {
                    var newList = list.Select(i => i.ToCustomTask());
                    items=newList.Select(i => (IToDoItem)i).ToList();
                    isAdded = false;
                    return items;
                }
                else return null;
            }
            else return items;
        }

        public async void UpdateToDoItem(IToDoItem todo)
        {
            int index = items.FindIndex(i => i.ToDoId == todo.ToDoId);
            if (index != -1)
            {
                items.RemoveAt(index);
                items.Add(todo);
            }
            await fastClient.SyncUpdateItemAsync((CustomTask)todo);
        }

        
            

    }
}
